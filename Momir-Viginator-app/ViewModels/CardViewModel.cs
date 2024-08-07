using Momir_Viginator_cs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Momir_Viginator_app.ViewModels
{
    public partial class CardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ImageSource m_cardSource;
        private ICardFactory m_factory;
        private ICard? m_card;

        public ICommand printCardCommand { get; private set; }

        public ImageSource cardSource {
            get => m_cardSource;
            private set
            {
                m_cardSource = value;
                onPropertyChanged();
            }
        }

        protected ICard? card
        {
            get => m_card;
            set
            {
                m_card = value;
                if (m_card != null)
                {
                    cardSource = ImageSource.FromStream(() => m_card.picture().AsStream());
                } else
                {
                    clearCard();
                }
                onPropertyChanged();
            }
        }

        protected void clearCard()
        {
            cardSource = ImageSource.FromFile("magic_card_back.png");
        }

        protected ICardFactory cardFactory {
            get => m_factory;
        }

        public CardViewModel(ICardFactory factory) {
            m_factory = factory;
            printCardCommand = new Command(() => { });
        }

        public void onPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
