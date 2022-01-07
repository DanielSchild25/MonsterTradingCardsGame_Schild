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
            int LastIndex1 = 3;
            int LastIndex2 = 3;
            Player Player1 = new("user1");
            Player Player2 = new("user2");

            MonsterCard Monster1 = new("Monster1", 1, ElementType.Normal, CardGroup.Dragon);
            MonsterCard Monster2 = new("Monster2", 2, ElementType.Water, CardGroup.FireElves);
            MonsterCard Monster3 = new("Monster3", 3, ElementType.Fire, CardGroup.Goblin);
            MonsterCard Monster4 = new("Monster4", 4, ElementType.Normal, CardGroup.Knights);
            SpellCard Spell1 = new("Spell1", 1, ElementType.Normal, CardGroup.Kraken);
            SpellCard Spell2 = new("Spell2", 2, ElementType.Fire, CardGroup.Orks);
            SpellCard Spell3 = new("Spell3", 3, ElementType.Normal, CardGroup.Wizzard);
            SpellCard Spell4 = new("Spell4", 4, ElementType.Water, CardGroup.Normal);

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


            while (true)
            {
                Random RandomCardIndex = new();

                //Card Last = Player1.deck.Last();
                //int LastIndex1 = Player1.deck.LastIndexOf(Last);
                Console.WriteLine(LastIndex1);
                int Index1 = RandomCardIndex.Next(0, LastIndex1);
                Console.WriteLine(Index1);
                Card Player1Card = Player1.deck[Index1];

                //Last = Player1.deck.Last();
                //int LastIndex2 = Player1.deck.LastIndexOf(Last);
                Console.WriteLine(LastIndex2);
                int Index2 = RandomCardIndex.Next(0, LastIndex2);
                Console.WriteLine(Index2);
                Card Player2Card = Player2.deck[Index2];

                float P1Dmg = Player1Card.damage;
                float P2Dmg = Player2Card.damage;


                if (Player1Card.Ctype == CardType.Spell)
                {
                    if (Player1Card.Etype == ElementType.Water && Player2Card.Etype == ElementType.Fire)
                        P1Dmg = Player1Card.damage * 2;
                    else if (Player2Card.Etype == ElementType.Water && Player1Card.Etype == ElementType.Fire)
                        P1Dmg = Player1Card.damage / 2;
                    else if (Player1Card.Etype == ElementType.Fire && Player2Card.Etype == ElementType.Normal)
                        P1Dmg = Player1Card.damage * 2;
                    else if (Player2Card.Etype == ElementType.Fire && Player1Card.Etype == ElementType.Normal)
                        P1Dmg = Player1Card.damage / 2;
                    else if (Player1Card.Etype == ElementType.Normal && Player2Card.Etype == ElementType.Water)
                        P1Dmg = Player1Card.damage * 2;
                    else if (Player2Card.Etype == ElementType.Normal && Player1Card.Etype == ElementType.Water)
                        P1Dmg = Player1Card.damage / 2;
                }

                if (Player2Card.Ctype == CardType.Spell)
                {
                    if (Player2Card.Etype == ElementType.Water && Player1Card.Etype == ElementType.Fire)
                        P2Dmg = Player2Card.damage * 2;
                    else if (Player1Card.Etype == ElementType.Water && Player2Card.Etype == ElementType.Fire)
                        P2Dmg = Player2Card.damage / 2;
                    else if (Player2Card.Etype == ElementType.Fire && Player1Card.Etype == ElementType.Normal)
                        P2Dmg = Player2Card.damage * 2;
                    else if (Player1Card.Etype == ElementType.Fire && Player2Card.Etype == ElementType.Normal)
                        P2Dmg = Player2Card.damage / 2;
                    else if (Player2Card.Etype == ElementType.Normal && Player1Card.Etype == ElementType.Water)
                        P2Dmg = Player2Card.damage * 2;
                    else if (Player1Card.Etype == ElementType.Normal && Player2Card.Etype == ElementType.Water)
                        P2Dmg = Player2Card.damage / 2;
                }

                if (Player1Card.Group == CardGroup.Goblin && Player2Card.Group == CardGroup.Dragon)
                    P1Dmg = 0;
                if (Player2Card.Group == CardGroup.Goblin && Player1Card.Group == CardGroup.Dragon)
                    P2Dmg = 0;
                if (Player1Card.Group == CardGroup.Wizzard && Player2Card.Group == CardGroup.Orks)
                    P2Dmg = 0;
                if (Player2Card.Group == CardGroup.Wizzard && Player1Card.Group == CardGroup.Orks)
                    P1Dmg = 0;
                if (Player1Card.Group == CardGroup.Knights && Player2Card.Ctype == CardType.Spell && Player2Card.Etype == ElementType.Water)
                    P1Dmg = 0;
                if (Player2Card.Group == CardGroup.Knights && Player1Card.Ctype == CardType.Spell && Player1Card.Etype == ElementType.Water)
                    P2Dmg = 0;
                if (Player1Card.Group == CardGroup.Kraken && Player2Card.Ctype == CardType.Spell)
                    P2Dmg = 0;
                if (Player2Card.Group == CardGroup.Kraken && Player1Card.Ctype == CardType.Spell)
                    P1Dmg = 0;
                if (Player1Card.Group == CardGroup.FireElves && Player2Card.Group == CardGroup.Dragon)
                    P2Dmg = 0;
                if (Player2Card.Group == CardGroup.FireElves && Player1Card.Group == CardGroup.Dragon)
                    P1Dmg = 0;


                Console.WriteLine($"Round {round}: P1: '{Player1Card.name}' ({P1Dmg}) VS P2: '{Player2Card.name}' ({P2Dmg}) ");

                if (P1Dmg == P2Dmg)
                {
                    Console.WriteLine($"Round {round}: Draw!");
                }
                else if (P1Dmg > P2Dmg)
                {

                    Player1.deck.Add(Player2.deck[Index2]);
                    Player2.deck.Remove(Player2.deck[Index2]);
                    LastIndex1++;
                    LastIndex2--;
                    Console.WriteLine($"Round {round} Result: Player 1 won! Card '{Player2Card.name}' moved from Player2s deck to Player 1s deck.");
                }
                else if (P1Dmg < P2Dmg)
                {
                    Player2.deck.Add(Player1.deck[Index1]);
                    Player1.deck.Remove(Player1.deck[Index1]);
                    LastIndex1--;
                    LastIndex2++;
                    Console.WriteLine($"Round {round} Result: Player 2 won! Card '{Player1Card.name}' moved from Player 1s deck to Player 2s deck.");
                }
                round++;

                if (LastIndex1 < 0)
                {
                    Console.WriteLine("Player 2 Wins! Match END!");
                    return;
                }

                if (LastIndex2 < 0)
                {
                    Console.WriteLine("Player 1 Wins! Match END!");
                    return;
                }

                if (round > 100)
                {
                    Console.WriteLine("Round Limit reached! Match END!");
                    return;
                }


            }


        }
    }
}
