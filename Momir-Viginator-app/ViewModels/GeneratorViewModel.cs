using Momir_Viginator_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Momir_Viginator_app.ViewModels
{   
    public class GeneratorViewModel: CardViewModel
    {
        private int m_convertedManaCost;

        public ICommand generateCommand { get; private set; }
        public float convertedManaCost
        {
            set => m_convertedManaCost = (int)value;
            get => m_convertedManaCost;
        }

        public GeneratorViewModel(ICardFactory factory)
            : base(factory)
        {
            generateCommand = new Command(async () => {
                card = await cardFactory.makeRandomAsync(m_convertedManaCost);
            });

            clearCard();
        }
    }
}
