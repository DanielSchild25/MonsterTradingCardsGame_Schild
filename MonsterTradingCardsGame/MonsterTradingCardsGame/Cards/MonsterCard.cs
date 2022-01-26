using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Cards
{
    class MonsterCard : Card
    {
        public MonsterCard(string id, string name, float damage) : base(id, name, damage)
        {
            this.name = name;
            this.damage = damage;
            this.id = id;
        }
    }
}
