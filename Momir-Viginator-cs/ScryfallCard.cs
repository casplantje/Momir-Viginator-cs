using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Maui.Graphics.Platform;

namespace Momir_Viginator_cs
{
    public class ScryfallCard : ICard
    {
        private string m_defense;
        private string m_flavourText;
        private string m_manaCost;
        private string m_name;
        private string m_oracleText;
        private string m_power;
        private ICard m_otherCard;
        private string m_imageUrl;
        private Microsoft.Maui.Graphics.IImage m_cachedPicture;

        public ScryfallCard()
        {
            m_flavourText = "";
            m_name = "";
            m_oracleText = "";
            m_manaCost = "";
            m_otherCard = null;
            m_cachedPicture = null;
        }

        public string defense
        {
            get => m_defense;
            set => m_defense = value;
        }

        public string power
        {
            get => m_power;
            set => m_power = value;
        }

        public string flavourText
        {
            get => m_flavourText;
            set => m_flavourText = value;
        }

        public string manaCost
        {
            get => m_manaCost;
            set => m_manaCost = value;
        }

        public string name
        {
            get => m_name;
            set => m_name = value;
        }

        public string oracleText
        {
            get => m_oracleText;
            set => m_oracleText = value;
        }

        public ICard otherSide
        {
            get => m_otherCard;
            set => m_otherCard = value;
        }

        public string imageUrl
        {
            get => m_imageUrl;
            set => m_imageUrl = value;
        }

        public Microsoft.Maui.Graphics.IImage picture()
        {
            if (m_cachedPicture != null) return m_cachedPicture;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(m_imageUrl);
            HttpResponseMessage response = client.GetAsync(m_imageUrl).Result;
            System.Drawing.Image image;
            if (response.IsSuccessStatusCode)
            {
                var imageStream = response.Content.ReadAsStream();
                m_cachedPicture = PlatformImage.FromStream(imageStream);
                return m_cachedPicture;
            }
            else
            {
                return null;
            }
        }
    }
}