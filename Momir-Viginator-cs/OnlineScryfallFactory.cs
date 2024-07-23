using System;
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
            public float power { get; set; }
            public float toughness { get; set; }
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
            ScryfallCard card = new ScryfallCard();

            card.name = face.name;
            card.oracleText = face.oracle_text;
            card.flavourText = face.flavor_text;
            card.power = face.power;
            card.defense = face.toughness;
            card.manaCost = face.mana_cost;
            card.picture = getPicture(face.image_uris.border_crop);

            return card;
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
                    card.otherSide = faceToCard(resultingObject.card_faces[1]);
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

        private string MakeScryfallRequest(string path, string parameters)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.scryfall.com/"+path);
            client.DefaultRequestHeaders.Add("User-Agent", "Momir-viginator-cs");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("?"+parameters).Result;

            if (response.IsSuccessStatusCode)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                return task.Result;
            }
            else
            {
                return null;
            }
        }

        public ICard makeRandom(int convertedManaCost)
        {
            string cardJson = MakeScryfallRequest("cards/random", "q=t:creature+cmc:" + convertedManaCost.ToString());
            if (cardJson != null)
            {
                return JsonToCard(cardJson);
            } else
            {
                return null;
            }
        }

        public ICard makeByName(string name)
        {
            name = name.Replace(' ', '+');
            string cardJson = MakeScryfallRequest("cards/named", "exact=" + name);
            if (cardJson != null)
            {
                return JsonToCard(cardJson);
            }
            else
            {
                return null;
            }
        }

        private System.Drawing.Image getPicture(string url)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = client.GetAsync(url).Result;
            System.Drawing.Image image;
            if (response.IsSuccessStatusCode)
            {
                image = System.Drawing.Image.FromStream(response.Content.ReadAsStream());
                return image;
            } else
            {
                return null;
            }
        }
    }
}