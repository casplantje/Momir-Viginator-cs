using MobilePrintinator_cs;
using Moq;
using SkiaSharp;
using System.Text;

namespace MobilePrintinator_Tests
{
    [TestFixture]
    public class UnitTests_ESCPOSPrinter
    {
        private Mock<IESCPOSDriver> m_mockESCPOSDriver;
        private IMobilePrinter m_printer;
        private ESCPOSPrinterProperties m_properties = new ESCPOSPrinterProperties
        {
            marginLeftInmm = 5,
            marginRightInmm = 5,
            paperWidthInmm = 57,
            horizontalDpi = 203,
            verticalDpi = 203,
            imageRowSize = 24,
            dotDensity = 33,
            printerName = "PT-230_676C"
        };
        public UnitTests_ESCPOSPrinter() {


            m_mockESCPOSDriver = new Mock<IESCPOSDriver>();
            m_printer = new ESCPOSPrinter(m_mockESCPOSDriver.Object, m_properties);

            m_mockESCPOSDriver.Setup(driver => driver.isConnected).Returns(true);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void WhenWriteIsCalled_ThenTheBufferIsFilledWithThatExactData()
        {
            m_printer.Write("abcdefghijklmnopqrstuvwxyz");
            m_printer.PrintBufferAsync().Wait();

            m_mockESCPOSDriver.Verify(static driver => driver.SendDataAsync(new byte[] {
                (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', (byte)'f', (byte)'g', (byte)'h', (byte)'i', (byte)'j', (byte)'k', (byte)'l', (byte)'m', (byte)'n', (byte)'o', (byte)'p', (byte)'q', (byte)'r', (byte)'s', (byte)'t', (byte)'u', (byte)'v', (byte)'w', (byte)'x', (byte)'y', (byte)'z'
            }));
        }

        [Test]
        public void WhenNewlineIsCalled_ThenTheBufferIsFilledCarriageReturn()
        {
            m_printer.NewLine();
            m_printer.PrintBufferAsync().Wait();

            m_mockESCPOSDriver.Verify(static driver => driver.SendDataAsync(new byte[] {
                (byte)'\r'
            }));
        }

        [Test]
        public void WhenInitPageIsCalled_ThenTheBufferIsFilledCarriageReturn()
        {
            m_printer.InitPage();
            m_printer.PrintBufferAsync().Wait();

            m_mockESCPOSDriver.Verify(static driver => driver.SendDataAsync(new byte[] {
                (byte)0x1B, (byte)'@'
            }));
        }

        [Test]
        public void WhenCutIsCalled_ThenTheBufferIsFilledCarriageReturn()
        {
            m_printer.Cut();
            m_printer.PrintBufferAsync().Wait();

            m_mockESCPOSDriver.Verify(static driver => driver.SendDataAsync(new byte[] {
                (byte)0x1B, (byte)'m'
            }));
        }

        [Test]
        public void WhensRichTexCalledWithVariousStyles_ThenTheBufferIsFilledWithCorrespondingData()
        {
            m_printer.RichText(m_printer.RichText().Italic().Text("test").Markup(new[] { RichTextMarkup.Large, RichTextMarkup.Bold }).Text("bold"));
            m_printer.PrintBufferAsync().Wait();

            m_mockESCPOSDriver.Verify(static driver => driver.SendDataAsync(new byte[] {
                (byte)0x1B, (byte)'!', (byte)0x40, (byte)'t', (byte)'e', (byte)'s', (byte)'t', (byte)0x1B, (byte)'!', (byte)0x18, (byte)'b', (byte)'o', (byte)'l', (byte)'d'
            }));
        }

        [Test]
        public void WhenImageIsCalled_ThenTheBufferIsFilledWithCorrespondingData()
        {
            SKImageInfo info = new SKImageInfo(24, 24);
            SKSurface surface = SKSurface.Create(info);
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.White);
            canvas.Flush();

            SkiaSharp.SKBitmap sKBitmap = SkiaSharp.SKBitmap.FromImage(surface.Snapshot());
            sKBitmap.SetPixel(0, 0, SkiaSharp.SKColors.Black);
            sKBitmap.SetPixel(23, 23, SkiaSharp.SKColors.Black);
            sKBitmap.SetPixel(0, 23, SkiaSharp.SKColors.Black);
            sKBitmap.SetPixel(23, 0, SkiaSharp.SKColors.Black);

            m_printer.Image(sKBitmap, false);
            m_printer.PrintBufferAsync().Wait();

            m_mockESCPOSDriver.Verify(driver => driver.SendDataAsync(new byte[] {
                (byte)0x1B, (byte)'@', (byte)0x1B, (byte)'3', m_properties.imageRowSize, 
                (byte)0X1b, (byte)'*', m_properties.dotDensity, (byte)24, (byte)0x00,
                (byte)0x80, (byte)0x00, (byte)0x01, 
                (byte)0x00, (byte)0x00, (byte)0x00, 
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x00, (byte)0x00, (byte)0x00,
                (byte)0x80, (byte)0x00, (byte)0x01,
                (byte)0x0A, (byte)0x1B, (byte)'3', (byte)30
            }));
        }

        [Test, Ignore("TODO")]
        public void WhenBarcodeIsCalled_ThenTheBufferIsFilledWithABarcodeCommand()
        {
            m_printer.BarCode(BarCodeType.EAN13, Encoding.UTF8.GetBytes("1234567890123"));
            m_printer.PrintBufferAsync().Wait();

            //todo: finish from here
            m_mockESCPOSDriver.Verify(static driver => driver.SendDataAsync(new byte[] {
                (byte)0x1C, (byte)'}'
            }));
        }
    }
}