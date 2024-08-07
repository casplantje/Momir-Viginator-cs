using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Momir_Viginator_cs
{
    public interface ICardFactory
    {
        Task<ICard?> makeRandomAsync(int convertedManaCost);
        Task<ICard?> makeByNameAsync(string name);
    }
}