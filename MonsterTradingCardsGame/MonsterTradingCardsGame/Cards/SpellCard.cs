using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Cards
{
    class SpellCard : Card
    {
        public SpellCard(string id,  string name, float damage, ElementType Etype, CardType Ctype, CardGroup group) : base(id, name, damage, Etype, Ctype, group)
        {
            this.name = name;
            this.damage = damage;
            this.Etype = Etype;
            this.Ctype = CardType.Spell;
            this.Group = group;
            this.id = id;
        }
    }
}
