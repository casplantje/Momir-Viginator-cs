using CommunityToolkit.Mvvm.Input;
using Momir_Viginator_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Momir_Viginator_app.ViewModels
{   
    public partial class GeneratorViewModel: CardViewModel
    {
        private int m_convertedManaCost;

        [RelayCommand]
        private async Task Generate()
        {
            Card = await cardFactory.makeRandomAsync(m_convertedManaCost);
            if (Preferences.Get("printOnRandomlyGenerated", false))
            {
                await PrintCard();
            }
        }
        public float convertedManaCost
        {
            set => m_convertedManaCost = (int)value;
            get => m_convertedManaCost;
        }

        public GeneratorViewModel(ICardFactory factory)
            : base(factory)
        {
            clearCard();
        }
    }
}
