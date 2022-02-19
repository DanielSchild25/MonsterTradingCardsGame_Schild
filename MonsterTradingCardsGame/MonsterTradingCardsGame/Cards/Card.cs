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
        public ElementType EType;
        public CardType CType;
        public CardGroup CGroup;

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
            return data == null ? 0 : data["max0"] is System.DBNull ? 0 : (int)data["max0"] + 1;
        }

        public static async Task<int> BuyPackage(string username)
        {
            var result = await Database.Base.Read("coins", "users", new() { { "username", username } });
            if (result == null)
                return -1;
            var coins = result["coins0"];
            if ((int)coins < 5)
                return -2;
            
            result = await Database.Base.Read("packageid", "cards", new() { { "username", "NULL" } }, true);
            if (result == null)
                return -1;
            var packageid = result["packageid0"];
            bool success = await Database.Base.Update("cards", new() { { "username", username } }, new() { { "packageid", packageid } });
            if (!success)
                return -1;

            coins = (int)coins - 5;
            success = await Database.Base.Update("users", new() { { "coins", coins } }, new() { { "username", username } });
            if (!success)
                return -1;

            return 0;
            
        }

        public static async Task<Dictionary<string, object>> GetCardsId(string username)
        {
            var result = await Database.Base.Read("id", "cards", new() { { "username", username } });
            return result;
        }

        public static async Task<Dictionary<string, object>> ShowDeck(string username)
        {
            var result = await Database.Base.Read("*", "decks", new() { { "username", username } });
            return result;
        }

        public static async Task<bool> CreateDeck(string username, string card1, string card2, string card3, string card4)
        {
            Dictionary<string, object> data = new() { { "username", username }, { "card1", card1 }, { "card2", card2 }, { "card3", card3 }, { "card4", card4 } };
            bool success = await Database.Base.Write("decks", data);
            if (!success)
                return false;
            return true;
        }

        public static async Task<bool> EditDeck(string username, string card1, string card2, string card3, string card4)
        {
            Dictionary<string, object> data = new() { { "card1", card1 }, { "card2", card2 }, { "card3", card3 }, { "card4", card4 } };
            var result = await Database.Base.Read("*", "cards", new() { { "id", card1 }, { "username", username } });
            if (result == null)
                return false;
            result = await Database.Base.Read("*", "cards", new() { { "id", card2 }, { "username", username } });
            if (result == null)
                return false;
            result = await Database.Base.Read("*", "cards", new() { { "id", card3 }, { "username", username } });
            if (result == null)
                return false;
            result = await Database.Base.Read("*", "cards", new() { { "id", card4 }, { "username", username } });
            if (result == null)
                return false;
            bool success = await Database.Base.Update("decks",  data, new() { { "username", username } });
            if (!success)
                return false;
            return true;
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
