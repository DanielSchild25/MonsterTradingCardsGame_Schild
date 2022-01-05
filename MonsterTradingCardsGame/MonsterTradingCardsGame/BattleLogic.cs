using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Cards;

namespace MonsterTradingCardsGame
{
    class BattleLogic
    {
        

        public void Battle()
        {
            int round = 1;
            Player Player1 = new("user1");
            Player Player2 = new("user2");

            MonsterCard Monster1 = new("Monster1",1, ElementType.Normal);
            MonsterCard Monster2 = new("Monster2", 2, ElementType.Normal);
            MonsterCard Monster3 = new("Monster3", 3, ElementType.Normal);
            MonsterCard Monster4 = new("Monster4", 4, ElementType.Normal);
            SpellCard Spell1 = new("Spell1", 1, ElementType.Normal);
            SpellCard Spell2 = new("Spell2", 2, ElementType.Normal);
            SpellCard Spell3 = new("Spell3", 3, ElementType.Normal);
            SpellCard Spell4 = new("Spell4", 4, ElementType.Normal);

            List<Card> P1 = new()
            {
                Monster1,
                Monster2,
                Spell1,
                Spell2
            };

            List<Card> P2 = new()
            {
                Monster3,
                Monster4,
                Spell3,
                Spell4
            };

            Player1.deck = P1;
            Player2.deck = P2;


            while(true)
            {
                Random RandomCardIndex = new();

                int Index1 = RandomCardIndex.Next(0, 3);
                Card Player1Card = Player1.deck[Index1];

                int Index2 = RandomCardIndex.Next(0, 3);
                Card Player2Card = Player2.deck[Index2];

                if(Player1Card.damage == Player2Card.damage)
                {
                    Console.WriteLine($"Round {round}: Draw!");
                }
                else if(Player1Card.damage > Player2Card.damage)
                {
                    Player1.deck.Add(Player2.deck[Index2]);
                    Player2.deck.Remove(Player2.deck[Index2]);
                    Console.WriteLine($"Round {round}: Player 1 won! Card {Player2.deck[Index2].name} move from Player1s deck to Player 2s deck.");
                }
                else if (Player1Card.damage < Player2Card.damage)
                {
                    Player2.deck.Add(Player1.deck[Index1]);
                    Player1.deck.Remove(Player1.deck[Index1]);
                    Console.WriteLine($"Round {round}: Player 2 won! Card {Player1.deck[Index1].name} move from Player 2s deck to Player 1s deck.");
                }
                round++;


            }


        }
    }
}
