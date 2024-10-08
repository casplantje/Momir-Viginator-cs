﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Momir_Viginator_cs.ScryfallJson;
using Newtonsoft.Json;

namespace Momir_Viginator_cs
{
    namespace ScryfallJson {
        public class ImageUrls
        {
            public string small { get; set; }
            public string normal { get; set; }
            public string large { get; set; }
            public string png { get; set; }
            public string art_crop { get; set; }
            public string border_crop { get; set; }
        }

        public class Face
        {
            public string name { get; set; }
            public ImageUrls image_uris { get; set; }
            public string mana_cost { get; set; }
            public string oracle_text { get; set; }
            public string flavor_text { get; set; }
            public string power { get; set; }
            public string toughness { get; set; }
        }
        public class Card : Face
        {
            public string id { get; set; }
            public IList<Face> card_faces { get; set; }
        }
    }
    public class OnlineScryfallFactory : ICardFactory
    {
        private ScryfallCard faceToCard(ScryfallJson.Face face)
        {
            try
            {
                ScryfallCard card = new ScryfallCard();

                card.name = face.name;
                card.oracleText = face.oracle_text;
                card.flavourText = face.flavor_text;
                card.power = face.power;
                card.defense = face.toughness;
                card.manaCost = face.mana_cost;
                card.imageUrl = face.image_uris.border_crop;

                return card;
            } catch (Exception ex)
            {
                return null;
            }
        }

        private ScryfallCard JsonToCard(string json)
        {
            var resultingObject = JsonConvert.DeserializeObject<ScryfallJson.Card>(json);
            if (resultingObject != null)
            {
                ScryfallCard card = null;

                if (resultingObject.card_faces != null)
                {
                    card = faceToCard(resultingObject.card_faces[0]);
                    if (card != null)
                    {
                        card.otherSide = faceToCard(resultingObject.card_faces[1]);
                    }
                }
                else
                {
                    card = faceToCard(resultingObject);
                }

                return card;
            }
            else
            {
                return null;
            }
        }

        private async Task<string> MakeScryfallRequestAsync(string path, string parameters)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.scryfall.com/" + path);
            client.DefaultRequestHeaders.Add("User-Agent", "Momir-viginator-cs");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("?" + parameters).Result;

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<ICard?> makeRandomAsync(int convertedManaCost)
        {
            var cardJson = await MakeScryfallRequestAsync("cards/random", "q=t:creature+cmc:" + convertedManaCost.ToString());
            if (cardJson != null)
            {
                return JsonToCard(cardJson);
            }
            else
            {
                return null;
            }
        }

        public async Task<ICard?> makeByNameAsync(string name)
        {
            name = name.Replace(' ', '+');
            string cardJson = await MakeScryfallRequestAsync("cards/named", "exact=" + name);
            if (cardJson != null)
            {
                return JsonToCard(cardJson);
            }
            else
            {
                return null;
            }
        }
    }
}