using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilePrintinator_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePrintinator.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string m_printerName;

        [ObservableProperty]
        private float m_paperWidthInmm;

        [ObservableProperty]
        private float m_marginLeftInInmm;

        [ObservableProperty]
        private float m_marginRightInInmm;

        [ObservableProperty]
        private int m_dpi;

        [ObservableProperty]
        private byte m_imageRowSize;

        [ObservableProperty]
        private byte m_dotDensity;

        [RelayCommand]
        private void ApplySettings()
        {
            ESCPOSPrinterProperties properties = new ESCPOSPrinterProperties { 
                paperWidthInmm = PaperWidthInmm,
                marginLeftInmm = MarginLeftInInmm,
                marginRightInmm = MarginRightInInmm,
                horizontalDpi = Dpi,
                verticalDpi = Dpi,
                imageRowSize = ImageRowSize,
                dotDensity = DotDensity,
                printerName = PrinterName,
            };
            m_printinatorService.Configure(properties);
            m_printinatorService.SaveConfiguration();
        }

        protected void LoadSettings()
        {
            var properties = m_printinatorService.Printer().PrinterProperties;

            PaperWidthInmm = properties.paperWidthInmm;
            MarginLeftInInmm = properties.marginLeftInmm;
            MarginRightInInmm = properties.marginRightInmm;
            Dpi = properties.horizontalDpi;
            if (properties is ESCPOSPrinterProperties)
            {
                var escposProperties = (ESCPOSPrinterProperties)properties;
                ImageRowSize = escposProperties.imageRowSize;
                DotDensity = escposProperties.dotDensity;
                PrinterName = escposProperties.printerName;
            }
        }

        protected IMobilePrintinatorService m_printinatorService = null;

    }
}
