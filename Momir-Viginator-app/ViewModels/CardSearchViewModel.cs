using Momir_Viginator_cs.ScryfallJson;
using Momir_Viginator_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Momir_Viginator_app.ViewModels
{
    public partial class CardSearchViewModel : CardViewModel
    {
        [ObservableProperty]
        private string m_searchName;

        [RelayCommand]
        private async Task Search()
        {
            Card = await cardFactory.makeByNameAsync(SearchName);
        }

        public CardSearchViewModel(ICardFactory factory)
            : base(factory)
        {
            SearchName = "";

            clearCard();
        }
    }
}
