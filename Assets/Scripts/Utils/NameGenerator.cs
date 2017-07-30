using UnityEngine;

namespace LD39.Utils
{
    public class NameGenerator
    {
        private static readonly string[] firstNames =
        {
            "Bette",
            "Lawanda",
            "Brett",
            "Jane",
            "Elanor",
            "Adelina",
            "Randy",
            "Zulema",
            "Carylon",
            "Santiago",
            "Hipolito",
            "Abbie",
            "Ilse",
            "Moon",
            "Clair",
            "Merideth",
            "Alayna",
            "Shavonda",
            "Shala",
            "Sona",
            "Emerald",
            "Rubi",
            "Corrina",
            "Art",
            "Arianna",
            "Raphael",
            "Annamaria",
            "Cecile",
            "Melody",
            "Hae",
            "Carlo",
            "Marhta",
            "Mallie",
            "Ophelia",
            "Merilyn",
            "Kay",
            "Bambi",
            "Breanne",
            "Janet",
            "Marcy",
            "Mercedes",
            "Twila",
            "John",
            "Vivien",
            "Glady",
            "Lon",
            "Rudolf",
            "Rachael",
            "Jack",
            "Twana",
        };

        private static readonly string[] lastNames =
        {
            "Solie",
            "Toon",
            "Ewing",
            "Avery",
            "Ferriera",
            "Manganaro",
            "Picou",
            "Mcgrath",
            "Templeman",
            "Renard",
            "Axley",
            "Zahradnik",
            "Duffey",
            "Selle",
            "Gilland",
            "Pond",
            "Serafini",
            "Elwood",
            "Draughn",
            "Gover",
            "Stiver",
            "Heidecker",
            "Pipkins",
            "Buckland",
            "Mccullen",
            "Piermarini",
            "Brandes",
            "Alexis",
            "Kroner",
            "Villines",
            "Jarosz",
            "Causey",
            "Cannon",
            "Talbot",
            "Shontz",
            "Maron",
            "Spaulding",
            "Bill",
            "Laxson",
            "Crowner",
            "Foston",
            "Schafer",
            "Gagner",
            "Cyr",
            "Faulcon",
            "Whitting",
            "Vasconcellos",
            "Hildreth",
            "Claggett",
            "Vega"
        };

        public static string Generate()
        {
            return string.Format("{0} {1}", firstNames[Random.Range(0, firstNames.Length)], lastNames[Random.Range(0, lastNames.Length)]);
        }

        public static string GenerateEmailAddress(string name)
        {
            name = name.Replace(" ", ".");
            name = name.ToLower();
            name += "@powerweb.com";

            return name;
        }
    }
}
