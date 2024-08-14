using Momir_Viginator_cs;
using System.Drawing;

namespace Momir_Viginator_app
{
    public partial class Generator : ContentPage
    {
        public Generator()
        {
            InitializeComponent();
        }

        private void cmcStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            cmcLabel.Text = "CMC: " + cmcStepper.Value.ToString();
        }
    }

}
