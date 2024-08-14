using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobilePrintinator_cs
{
    public enum RichTextMarkup
    {
        Bold,
        Italic,
        Underline,
        Large,
        Header
    }
    public interface IRichTextBuilder
    {
        IRichTextBuilder Text(string text);

        IRichTextBuilder CancelFont();

        IRichTextBuilder Bold();

        IRichTextBuilder Italic();

        IRichTextBuilder Underline();

        IRichTextBuilder NewLine();

        IRichTextBuilder Markup(RichTextMarkup[] markup);

        IRichTextBuilder RotateCharacter(bool rotate);

        IRichTextBuilder Invert(bool invert);

    }
}