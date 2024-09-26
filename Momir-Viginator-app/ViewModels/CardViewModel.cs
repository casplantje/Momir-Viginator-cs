using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilePrintinator;
using MobilePrintinatorMaui;
using Momir_Viginator_cs;
using SkiaSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Momir_Viginator_app.ViewModels
{
    public partial class CardViewModel : ObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        IMobilePrinter m_printer;

        private ICardFactory m_factory;
        private ICard? m_card;

        [ObservableProperty]
        private ImageSource m_cardSource;
        protected ICard? Card
        {
            get => m_card;
            set
            {
                m_card = value;
                if (m_card != null)
                {
                    CardSource = ImageSource.FromStream(() => m_card.picture().AsStream());
                } else
                {
                    clearCard();
                }
                onPropertyChanged();
            }
        }

        [RelayCommand]
        protected async Task PrintCard()
        {
            if (Card == null)
            {
                return;
            }

            if (m_printer != null)
            {
                m_printer.Image(SKBitmap.Decode(Card.picture().AsStream()), true);
                m_printer.Cut();
                await m_printer.PrintBufferAsync();
            }

        }

        protected void clearCard()
        {
            CardSource = ImageSource.FromFile("magic_card_back.png");
        }

        protected ICardFactory cardFactory {
            get => m_factory;
        }

        public CardViewModel(ICardFactory factory) {
            m_factory = factory;
            m_printer = App.Current!.MainPage!.Handler!.MauiContext!.Services.GetRequiredService<IMobilePrintinatorService>().Printer();
        }

        public void onPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
