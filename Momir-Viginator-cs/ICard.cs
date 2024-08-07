using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Momir_Viginator_cs
{
    public interface ICard
    {
        string defense { get; }
        string power { get; }
        string flavourText { get; }
        string manaCost { get; }
        string name { get; }
        string oracleText { get; }
        ICard otherSide { get; }
        string imageUrl { get; }
        Microsoft.Maui.Graphics.IImage picture();
    }
}