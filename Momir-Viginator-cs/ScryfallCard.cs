using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Momir_Viginator_cs
{
    public class ScryfallCard : ICard
    {
        private float m_defense;
        private string m_flavourText;
        private string m_manaCost;
        private string m_name;
        private string m_oracleText;
        private Image m_picture;
        private float m_power;
        private ICard m_otherCard;

        public ScryfallCard()
        {
            m_flavourText = "";
            m_name = "";
            m_oracleText = "";
            m_manaCost = "";
            m_otherCard = null;
        }

        public float? defense
        {
            get => m_defense;
            set
            {
                if (value != null)
                {
                    m_defense = value.Value;
                }
            }
        }

        public float? power
        {
            get => m_power;
            set
            {
                if (value != null)
                {
                    m_power = value.Value;
                }
            }
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

        public Image picture
        {
            get => m_picture;
            set => m_picture = value;
        }
        public ICard otherSide 
        { 
            get => m_otherCard;
            set => m_otherCard = value;
        }
    }
}