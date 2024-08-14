using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePrintinator_cs
{
    internal class ESCPOSRichTextBuilder : IRichTextBuilder, IBufferBuilder
    {
        private List<byte> m_buffer = new List<byte>();

        public byte[] buffer
        {
            get => m_buffer.ToArray();
        }

        public IRichTextBuilder Text(string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            Append(data);
            return this;
        }

        private void MasterSelectFont(byte font)
        {
            Append(0x1B); // ESC
            Append('!');
            Append(font);
        }

        public IRichTextBuilder CancelFont()
        {
            MasterSelectFont(0x00);
            return this;
        }

        public IRichTextBuilder Bold()
        {
            MasterSelectFont(0x08);
            return this;
        }

        public IRichTextBuilder Italic()
        {
            MasterSelectFont(0x40);
            return this;
        }

        public IRichTextBuilder Underline()
        {
            MasterSelectFont(0x80);
            return this;
        }

        public IRichTextBuilder NewLine()
        {
            Append(0x0A);
            return this;
        }

        public void Append(byte[] data)
        {
            if (data == null)
                return;

            m_buffer.AddRange(data);
        }
        public void Append(char c)
        {
            m_buffer.Add((byte)c);
        }
        public void Append(byte b)
        {
            m_buffer.Add(b);
        }

        private byte MarkupToMasterSelectFont(RichTextMarkup markup)
        {
            switch (markup)
            {
                case RichTextMarkup.Large:
                    return 0x10;
                case RichTextMarkup.Bold:
                    return 0x08;
                case RichTextMarkup.Italic:
                    return 0x40;
                case RichTextMarkup.Underline:
                    return 0x80;
                case RichTextMarkup.Header:
                    return 0x30;
                default:
                    return 0x00;
            }
        }

        public IRichTextBuilder Markup(RichTextMarkup[] markup)
        {
            byte result = 0x0;
            for (int i = 0; i < markup.Length; i++)
            {
                result |= MarkupToMasterSelectFont(markup[i]);
            }
            MasterSelectFont(result);
            return this;
        }

        public IRichTextBuilder Invert(bool invert)
        {
            Append(0x1D);
            Append('B');
            Append(invert ? (byte)0x01 : (byte)0x00);
            return this;
        }

        public IRichTextBuilder RotateCharacter(bool rotate)
        {
            Append(0x1B);
            Append('V');
            Append(rotate ? (byte)0x01 : (byte)0x00);
            return this;
        }
    }
}
