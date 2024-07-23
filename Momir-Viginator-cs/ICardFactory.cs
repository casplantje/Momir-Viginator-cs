using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Momir_Viginator_cs
{
    public interface ICardFactory
    {

        ICard makeRandom(int convertedManaCost);
        ICard makeByName(string name);
    }
}