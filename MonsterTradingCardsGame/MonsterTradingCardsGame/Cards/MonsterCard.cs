﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Cards
{
    class MonsterCard : Card
    {
        public MonsterCard(string name, float damage, ElementType type, CardGroup group) : base(name, damage, type, group)
        {
            this.name = name;
            this.damage = damage;
            this.Etype = type;
            this.Ctype = CardType.Monster;
            this.Group = group;
        }
    }
}
