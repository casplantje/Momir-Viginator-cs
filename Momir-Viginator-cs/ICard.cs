using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Momir_Viginator_cs
{
    public interface ICard
    {
        float? defense { get; }
        float? power { get; }
        string flavourText { get; }
        string manaCost { get; }
        string name { get; }
        string oracleText { get; }
        System.Drawing.Image picture { get; }
        ICard otherSide { get; }
    }
}