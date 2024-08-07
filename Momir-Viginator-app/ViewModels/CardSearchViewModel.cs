using Momir_Viginator_cs.ScryfallJson;
using Momir_Viginator_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Momir_Viginator_app.ViewModels
{
    public class CardSearchViewModel : CardViewModel
    {
        private string m_name;

        public string searchName
        {
            get => m_name;
            set => m_name = value;
        }

        public ICommand searchCommand { get; private set; }

        public CardSearchViewModel(ICardFactory factory)
            : base(factory)
        {
            m_name = "";
            searchCommand = new Command(async () => {
                card = await cardFactory.makeByNameAsync(m_name);
            });

            clearCard();
        }
    }
}
