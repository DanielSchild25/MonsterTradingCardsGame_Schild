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

        public Card(string name, float damage, ElementType type, CardGroup group)
        {
            this.name = name;
            this.damage = damage;
            this.Etype = type;
            this.Group = group;
        }

        /*public async Task<Card?> Create(string id, string name, float damage, int package = -1)
        {
            Card card = Build(id, name, damage);
            card.package = package;
            Dictionary<string, object> data = new() { { "id", id }, { "name", name }, { "damage", damage } };
            if (package >= 0)
                data["package"] = package;
            bool success = await Database.self.Write("card", data);
            if (!success)
                return null;
            return card;
        }*/
    }
}
