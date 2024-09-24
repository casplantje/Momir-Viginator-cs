#if ANDROID
using Java.Util;
using MobilePrintinator.Platforms.Android;
#endif
using MobilePrintinator_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobilePrintinator
{
    public class MobilePrintinatorService : IMobilePrintinatorService
    {
        private IMobilePrinter m_printer;
        IESCPOSDriver m_driver;
        public MobilePrintinatorService()
        {
            LoadConfiguration();
        }
        public void Configure(PrinterProperties properties)
        {
            if (properties is ESCPOSPrinterProperties)
            {
#if ANDROID
                m_driver = new MobilePrintinator.Platforms.Android.ESCPOSDriver(((ESCPOSPrinterProperties)properties).printerName);
#elif WINDOWS
                m_driver = new MobilePrintinator.Platforms.Windows.ESCPOSDriver();
#endif
                if (m_driver != null)
                {
                    m_printer = new ESCPOSPrinter(m_driver, (ESCPOSPrinterProperties)properties);
                }
            }
        }

        public void InitConfiguration()
        {
            Configure(new ESCPOSPrinterProperties
            {
                marginLeftInmm = 5,
                marginRightInmm = 5,
                paperWidthInmm = 57,
                horizontalDpi = 203,
                verticalDpi = 203,
                imageRowSize = 24,
                dotDensity = 33,
                printerName = "PT-230_676C"
            });
        }

        public void LoadConfiguration()
        {
            var properties = new ESCPOSPrinterProperties();
            properties.marginLeftInmm = Preferences.Get("marginLeftInmm", 5F);
            properties.marginRightInmm = Preferences.Get("marginRightInmm", 5F);
            properties.paperWidthInmm = Preferences.Get("paperWidthInmm", 57F);
            properties.horizontalDpi = Preferences.Get("horizontalDpi", 203);
            properties.verticalDpi = Preferences.Get("verticalDpi", 203);
            properties.imageRowSize = (byte)Preferences.Get("imageRowSize", 24);
            properties.dotDensity = (byte)Preferences.Get("dotDensity", 33);
            properties.printerName = Preferences.Get("printerName", "PT-230_676C");

            Configure(properties);
        }

        public void SaveConfiguration()
        {
            if (m_printer == null)
                return;

            var properties = m_printer.PrinterProperties;

            Preferences.Set("marginLeftInmm", properties.marginLeftInmm);
            Preferences.Set("marginRightInmm", properties.marginRightInmm);
            Preferences.Set("paperWidthInmm", properties.paperWidthInmm);
            Preferences.Set("horizontalDpi", properties.horizontalDpi);
            Preferences.Set("verticalDpi", properties.verticalDpi);
            if (properties is ESCPOSPrinterProperties)
            {
                var escposProperties = (ESCPOSPrinterProperties)properties;
                Preferences.Set("imageRowSize", escposProperties.imageRowSize);
                Preferences.Set("dotDensity", escposProperties.dotDensity);
                Preferences.Set("printerName", escposProperties.printerName);
            }
        }

        public IEnumerable<string> GetBluetoothPrinterNames()
        {
            return m_driver.GetBluetoothPrinterNames();
        }

        public IMobilePrinter Printer()
        {
            return m_printer;
        }
    }
}