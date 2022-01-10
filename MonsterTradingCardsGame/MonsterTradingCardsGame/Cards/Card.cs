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
        public ElementType Etype;
        public string id;
        public int package;
        public CardType Ctype;
        public CardGroup Group;

        public Card(string id,string name, float damage, ElementType Etype, CardType Ctype, CardGroup group)
        {
            this.id = id;
            this.name = name;
            this.damage = damage;
            this.Etype = Etype;
            this.Group = group;
            this.Ctype = Ctype;
        }

        public async static Task<Card?> Create(string id, string name, float damage, ElementType EType, CardType CType, CardGroup Group, int package = -1)
        {
            Card card = Build(id, name, damage, EType, CType, Group);
            card.package = package;
            Dictionary<string, object> data = new() { { "id", id }, { "name", name }, { "damage", damage }, { "elementtype", EType }, { "cardtype", CType }, { "gruppe", Group } };
            if (package >= 0)
                data["package"] = package;
            bool success = await Database.Base.Write("card", data);
            if (!success)
                return null;
            return card;
        }

        private  static Card Build(string id, string name, float damage, ElementType EType, CardType CType, CardGroup Group)
        {
            Card card = new(id, name, damage, EType, CType, Group);

            return card;
        }

        public static async Task<int> GetPackageID()
        {
            var data = await Database.Base.Read("MAX(package)", "card");
            return data == null ? 0 : data["max"] is System.DBNull ? 0 : (int)data["max"] + 1;
        }

        public static async Task<Card[]?> BuyPackage()
        {
            var result = await Database.Base.Read("package", "card", new() { { "username", "" } }/*, "RANDOM ()"*/);
            if (result == null)
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
