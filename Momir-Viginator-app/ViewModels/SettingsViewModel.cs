using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using MobilePrintinator;
using MobilePrintinator_cs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momir_Viginator_app.ViewModels
{
    public partial class SettingsViewModel : MobilePrintinator.ViewModels.SettingsViewModel
    {
        public SettingsViewModel() {
            m_printinatorService = App.Current!.MainPage!.Handler!.MauiContext!.Services.GetRequiredService<IMobilePrintinatorService>();
            LoadSettings();
            PrintOnRandomlyGenerated = Preferences.Get("printOnRandomlyGenerated", false);
        }

        [ObservableProperty]
        private bool m_printOnRandomlyGenerated;

        partial void OnPrintOnRandomlyGeneratedChanged(bool value)
        {
            //TODO: maybe this is something that should be kept out of viewmodels
            Preferences.Set("printOnRandomlyGenerated", value);
        }

        [RelayCommand]
        private async Task TestPrint()
        {
            var printer = m_printinatorService.Printer();
            //TODO: extract to IMobilePrinter
            printer.InitPage();
            //printer.Write("Hello World");
            //printer.RichText(printer.RichText().NewLine().Bold().Text("bold text").CancelFont().NewLine().Italic().Text("italic text").NewLine().CancelFont().Underline().Text("underlined text"));
            printer.RichText(printer.RichText().Markup(new[] {RichTextMarkup.Large}).Text("Hello World"));
            printer.Cut();
            await printer.PrintBufferAsync();
        }
    }
}
