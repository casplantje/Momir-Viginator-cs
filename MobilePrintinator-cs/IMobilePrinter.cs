using SkiaSharp;

namespace MobilePrintinator_cs
{
    public enum BarCodeType
    {
        UPC_A,
        UPC_E,
        EAN13,
        EAN8,
        CODE39,
        ITF,
        CODE128
    }
    public class PrinterProperties
    {
        public int pixelWidth { get {
                float printableWidthInmm = paperWidthInmm - marginLeftInmm - marginRightInmm;
                float printableWidthInInch = printableWidthInmm / 25.4F;
                return (int)Math.Round(printableWidthInInch * horizontalDpi);
            }
        }
        public int horizontalDpi;
        public int verticalDpi;
        public float paperWidthInmm;
        public float marginLeftInmm;
        public float marginRightInmm;
    }
    public interface IMobilePrinter
    {
        void InitPage();
        void Image(SKBitmap image, bool dither = false);
        void Write(string text);
        void NewLine();
        void Cut();
        Task PrintBufferAsync();
        PrinterProperties PrinterProperties { get; }

        IRichTextBuilder RichText();
        void RichText(IRichTextBuilder builder);

        void RotateText(int degrees);

        void QRCode(byte[] data);
        void BarCode(BarCodeType type, byte[] data);

    }
}