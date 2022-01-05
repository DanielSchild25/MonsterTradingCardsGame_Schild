using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Cards
{
    public enum ElementType { Fire, Water, Normal }
    class Card
    {
        
        public string name;
        public float damage;
        public ElementType type;
        public string id;
        public int package;

        public Card(string name, float damage, ElementType type)
        {
            this.name = name;
            this.damage = damage;
            this.type = type;
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
