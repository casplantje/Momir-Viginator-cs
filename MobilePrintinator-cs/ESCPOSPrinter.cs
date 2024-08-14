using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;

namespace MobilePrintinator_cs
{
    public class ESCPOSPrinterProperties : PrinterProperties
    {
        public string printerName;
        public byte imageRowSize;
        public byte dotDensity;
    }
    public class ESCPOSPrinter : IMobilePrinter, IBufferBuilder
    {
        private IESCPOSDriver m_driver;
        private List<byte> m_buffer;
        private ESCPOSPrinterProperties m_printerProperties;

        public byte[] buffer {
            get => m_buffer.ToArray();
        }

        public PrinterProperties PrinterProperties
        {
            get => m_printerProperties;
        }

        public void Append(byte[] data)
        {
            if (data == null)
                return;

            m_buffer.AddRange(data);
        }
        public void Append(char c)
        {
            m_buffer.Add((byte)c);
        }
        public void Append(byte b)
        {
            m_buffer.Add(b);
        }

        public ESCPOSPrinter(IESCPOSDriver driver, ESCPOSPrinterProperties printerProperties)
        {
            if (driver == null)
                throw new ArgumentNullException();

            m_driver = driver;
            m_buffer = new List<byte>();

            m_printerProperties = printerProperties;
        }

        private void SetLineSpacingCommand(byte lineSpacing)
        {
            Append(0x1B);
            Append('3');
            Append(lineSpacing);
        }

        private void StartTransmissionCommand()
        {
            Append(0x1B);
            Append('@');
        }

        private void ImageRowStartCommand(int width)
        {
            Append(0x1B);
            Append('*');
            Append(m_printerProperties.dotDensity);
            var widthInBytes = BitConverter.GetBytes(width);
            Append(widthInBytes[0]);
            Append(widthInBytes[1]);
        }

        private void ImageRowEndCommand()
        {
            Append(0x0A);
        }

        // One of the fastest bit reversal tricks for c#
        // source of benchmark https://stackoverflow.com/questions/3587826/is-there-a-built-in-function-to-reverse-bit-order
        // source of snippet http://graphics.stanford.edu/~seander/bithacks.html
        private static byte ReverseBitsWith7Operations(byte b)
        {
            return (byte)(((b * 0x0802u & 0x22110u) | (b * 0x8020u & 0x88440u)) * 0x10101u >> 16);
        }

        private static readonly byte[,] m_bayerMatrix =
        {
            {   0, 128, 32, 160,  8, 136, 40, 168,  2, 130, 34, 162, 10, 138, 42, 170 },
            { 192,  64, 224,  96, 200,  72, 232, 104, 194,  66, 226,  98, 202,  74, 234, 106 },
            {  48, 176,  16, 144,  56, 184,  24, 152,  50, 178,  18, 146,  58, 186,  26, 154 },
            { 240, 112, 208,  80, 248, 120, 216,  88, 242, 114, 210,  82, 250, 122, 218,  90 },
            {  12, 140,  44, 172,  4, 132,  36, 164,  14, 142,  46, 174,  6, 134,  38, 166 },
            { 204,  76, 236, 108, 196,  68, 228, 100, 206,  78, 238, 110, 198,  70, 230, 102 },
            {  60, 188,  28, 156,  52, 180,  20, 148,  62, 190,  30, 158,  54, 182,  22, 150 },
            { 252, 124, 220,  92, 244, 116, 212,  84, 254, 126, 222,  94, 246, 118, 214,  86 },
            {   3, 131,  35, 163,  11, 139,  43, 171,  1, 129,  33, 161,  9, 137,  41, 169 },
            { 195,  67, 227,  99, 203,  75, 235, 107, 193,  65, 225,  97, 201,  73, 233, 105 },
            {  51, 179,  19, 147,  59, 187,  27, 155,  49, 177,  17, 145,  57, 185,  25, 153 },
            { 243, 115, 211,  83, 251, 123, 219,  91, 241, 113, 209,  81, 249, 121, 217,  89 },
            {  15, 143,  47, 175,  7, 135,  39, 167,  13, 141,  45, 173,  5, 133,  37, 165 },
            { 207,  79, 239, 111, 199,  71, 231, 103, 205,  77, 237, 109, 197,  69, 229, 101 },
            {  63, 191,  31, 159,  55, 183,  23, 151,  61, 189,  29, 157,  53, 181,  21, 149 },
            { 255, 127, 223,  95, 247, 119, 215,  87, 253, 125, 221,  93, 245, 117, 213,  85 }
        };

        public void Image(SKBitmap image, bool dither = false)
        {
            byte rowSize = m_printerProperties.imageRowSize;
            const byte threshold = 127;
            const byte contrast = 50;
            const byte lowerThreshold = threshold - contrast;
            const byte upperThreshold = threshold + contrast;

            int height = image.Height;
            int width = image.Width;

            ResizeImageToWidthAndAdjustHeight(ref image, ref height, ref width);

            StartTransmissionCommand();

            SetLineSpacingCommand(rowSize);

            for (var offset = 0; offset < height; offset += rowSize)
            {
                ImageRowStartCommand(width);

                for (var x = 0; x < width; x++)
                {
                    BitArray column = new BitArray(rowSize);

                    for (var y = offset; y < Math.Min(offset + rowSize, height); y++)
                    {
                        var color = image.GetPixel(x, y);
                        var luminance = (byte)((color.Red + color.Green + color.Blue) / 3);
                        var posInColumn = y - offset;
                        byte t = dither ? m_bayerMatrix[y & 0x0f, x & 0x0f] : threshold;
                        
                        column[posInColumn] = (luminance < lowerThreshold) || ((luminance < t) && (luminance < upperThreshold));
                    }
                    byte[] columnBytes = new byte[rowSize / 8];
                    column.CopyTo(columnBytes, 0);
                    for (int i = 0; i < columnBytes.Length; i++)
                    {
                        columnBytes[i] = ReverseBitsWith7Operations(columnBytes[i]);
                    }
                    Append(columnBytes);
                }
                ImageRowEndCommand();
            }

            SetLineSpacingCommand(30);
        }

        private void ResizeImageToWidthAndAdjustHeight(ref SKBitmap skbitmap, ref int height, ref int width)
        {
            int pixelWidth = m_printerProperties.pixelWidth;

            if (width > pixelWidth)
            {
                float scale = pixelWidth / (float)width;
                height = (int)(height * scale);
                width = pixelWidth;

                skbitmap = skbitmap.Resize(new SkiaSharp.SKImageInfo(width, height), SkiaSharp.SKFilterQuality.High);
            }
        }

        public void NewLine()
        {
            Append('\r');
        }

        public void Write(string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            Append(data);
        }

        public IRichTextBuilder RichText()
        {
            return new ESCPOSRichTextBuilder();
        }

        public void RichText(IRichTextBuilder builder)
        {
            if (builder is ESCPOSRichTextBuilder)
            {
                Append(((ESCPOSRichTextBuilder)builder).buffer);
            } else
            {
                throw new Exception("Unsupported rich text builder");
            }
        }

        public async Task PrintBufferAsync()
        {
            if (!m_driver.isConnected)
            {
                await m_driver.Connect();
            }

            byte[] bytes = m_buffer.ToArray();
            await m_driver.SendDataAsync(bytes);
            m_buffer.Clear();
        }

        public void Cut()
        {
            //Todo: figure out where this comes from
            /*Append(0x1D); //GS
            Append('V');
            Append(48);*/
            Append(0x1B);
            Append('m');
        }

        public void InitPage()
        {
            Append(0x1B);
            Append('@');
        }

        private void Rotate90(bool on)
        {
            Append(0x1B);
            Append('V');
            Append(on ? (byte)0x01 : (byte)0x00);
        }

        private void UpsideDown(bool on)
        {
            Append(0x1B);
            Append('{');
            Append(on ? (byte)0x01 : (byte)0x00);
        }

        public void RotateText(int degrees)
        {
            switch (degrees)
            {
                case 90:
                    Rotate90(true);
                    break;
                case 180:
                    UpsideDown(true);
                    break;
                case 270:
                    Rotate90(true);
                    UpsideDown(true);
                    break;
                default:
                    Rotate90(false);
                    UpsideDown(false);
                    break;
            }
        }

        public void QRCode(byte[] data)
        {
            //TODO: if possible look up version to verify length capabilities
            // https://escpos.readthedocs.io/en/latest/imaging.html#d-barcode-generator-1c-7d-25-k-d1-dk-rel
            if (data.Length > byte.MaxValue)
            {
                throw new Exception("Data too large"); 
            }

            Append(0x1C);
            Append('}');
            Append((byte)data.Length);
            Append(data);
        }

        public void BarCode(BarCodeType type, byte[] data)
        {
            int maxLength = byte.MaxValue;
            byte typecode;
            switch (type)
            {
                case BarCodeType.CODE128:
                    maxLength = 80;
                    typecode = 0x06;
                    break;
                case BarCodeType.CODE39:
                    maxLength = 39;
                    typecode = 0x05;
                    break;
                case BarCodeType.EAN13:
                    maxLength = 13;
                    typecode = 0x00;
                    break;
                case BarCodeType.EAN8:
                    maxLength = 8;
                    typecode = 0x01;
                    break;
                case BarCodeType.UPC_A:
                    maxLength = 12;
                    typecode = 0x03;
                    break;
                case BarCodeType.UPC_E:
                    maxLength = 8;
                    typecode = 0x04;
                    break;

                default:
                    throw new Exception("Unsupported barcode type");
            }

            if (data.Length > maxLength)
            {
                throw new Exception("Data too large");
            }

            Append(0x1B);
            Append('(');
            Append('B');
            Append((byte)(data.Length % 256));
            Append((byte)(data.Length / 256));
            Append(0x02);
            Append(0x00);
            Append(typecode);
            Append(data);
        }
    }
}