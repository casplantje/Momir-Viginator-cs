using Momir_Viginator_cs;
using System.Drawing;

namespace Momir_Viginator_app
{
    public partial class Generator : ContentPage
    {
        ICardFactory m_factory;

        private void setCardPicture(ICard card)
        {
            if (card == null)
            {
                return;
            }
            cardPicture.Source = ImageSource.FromUri(new Uri(card.imageUrl));
        }

        public Generator()
        {
            m_factory = new OnlineScryfallFactory();
            InitializeComponent();

            var momirVigAvatar = m_factory.makeByName("Momir+Vig+Simic+Visionary+Avatar");
            if (momirVigAvatar != null)
            {
                setCardPicture(momirVigAvatar);
            }
        }

        private void OnGenerateClicked(object sender, EventArgs e)
        {
            var card = m_factory.makeRandom((int)cmcStepper.Value);
            if (card != null)
            {
                setCardPicture(card);
            }
        }

        private void cmcStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            cmcLabel.Text = "CMC: " + cmcStepper.Value.ToString();
        }
    }

}
