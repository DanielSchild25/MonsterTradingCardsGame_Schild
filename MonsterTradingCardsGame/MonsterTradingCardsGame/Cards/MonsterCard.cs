﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Cards
{
    class MonsterCard : Card
    {
        public MonsterCard(string name, float damage, ElementType type) : base(name, damage, type)
        {
            this.name = name;
            this.damage = damage;
            this.type = type;
        }
    }
}
