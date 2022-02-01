using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Cards
{
    public enum ElementType { Fire, Water, Normal }

    public enum CardType { Monster, Spell }

    public enum CardGroup { Goblin, Dragon, Wizzard, Orks, Knights, Kraken, FireElves, Normal }
    class Card
    {

        public string name;
        public float damage;
        public string id;
        public int package;

        public Card(string id,string name, float damage)
        {
            this.id = id;
            this.name = name;
            this.damage = damage;

        }

        public async static Task<Card?> Create(string id, string name, float damage, int package = -1)
        {
            Card card = Build(id, name, damage);
            card.package = package;
            Dictionary<string, object> data = new() { { "id", id }, { "name", name }, { "damage", damage } };
            if (package >= 0)
                data["packageid"] = package;
            bool success = await Database.Base.Write("cards", data);
            if (!success)
                return null;
            return card;
        }

        private  static Card Build(string id, string name, float damage)
        {
            Card card = new(id, name, damage);

            return card;
        }

        public static async Task<int> GetPackageID()
        {
            var data = await Database.Base.Read("MAX(packageid)", "cards");
            return data == null ? 0 : data["max"] is System.DBNull ? 0 : (int)data["max"] + 1;
        }

        public static async Task<Card[]?> BuyPackage(string username)
        {
            var result = await Database.Base.Read("packageid", "cards", new() { { "username", "NULL" } }, true);
            if (result == null)
                return null;
            var packageid = result["packageid"];
            bool success = await Database.Base.Update("cards", new() { { "username", username } }, new() { { "packageid", packageid } });
            if (!success)
                return null;
            return null;
            
        }

        /*private static void RegisterCard<T>(string name) where T : Card
        {
            if (cards.ContainKey(name))
                return;

            cards[name] = (string id, string name, float damage) =>
            {
                Card? card = (T?)Activator.CreateInstance(typeof(T), new object[] { id, name, damage });
                if (card == null)
                    throw new NullReferenceException("Something went wrong, unable to create card!");
                return card;
            };

        }*/
    }
}
