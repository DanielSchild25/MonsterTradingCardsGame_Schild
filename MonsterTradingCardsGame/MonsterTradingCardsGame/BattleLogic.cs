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
        public static async Task<string> Battle(string player1, string player2)
        {
            int round = 1;
            int LastIndex1 = 3;
            int LastIndex2 = 3;

            Dictionary<string, object> P1Ids;
            Dictionary<string, object> P2Ids;
            Dictionary<string, object> result;
            Card[] CardArray = new Card[10];

            P1Ids = await Database.Base.Read("*", "decks", new() { { "username", player1 } });
            P2Ids = await Database.Base.Read("*", "decks", new() { { "username", player2 } });

            result = await Database.Base.Read("*", "cards", new() { { "id", P1Ids["card10"] } });
            Card P1Card1 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray.Append(P1Card1);

            result = await Database.Base.Read("*", "cards", new() { { "id", P1Ids["card20"] } });
            Card P1Card2 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray.Append(P1Card2);

            result = await Database.Base.Read("*", "cards", new() { { "id", P1Ids["card30"] } });
            Card P1Card3 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray.Append(P1Card3);

            result = await Database.Base.Read("*", "cards", new() { { "id", P1Ids["card40"] } });
            Card P1Card4 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray.Append(P1Card4);

            result = await Database.Base.Read("*", "cards", new() { { "id", P2Ids["card10"] } });
            Card P2Card1 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray.Append(P2Card1);

            result = await Database.Base.Read("*", "cards", new() { { "id", P2Ids["card20"] } });
            Card P2Card2 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray.Append(P2Card2);

            result = await Database.Base.Read("*", "cards", new() { { "id", P2Ids["card30"] } });
            Card P2Card3 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray.Append(P2Card3);

            result = await Database.Base.Read("*", "cards", new() { { "id", P2Ids["card40"] } });
            Card P2Card4 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray.Append(P2Card4);

            foreach(Card C in CardArray)
            {
                if (C.name.Contains("Fire"))
                    C.EType = ElementType.Fire;
                else if (C.name.Contains("Water"))
                    C.EType = ElementType.Water;
                else
                    C.EType = ElementType.Normal;

                if (C.name.Contains("Spell"))
                    C.CType = CardType.Spell;
                else
                    C.CType = CardType.Monster;

                if (C.name.Contains("Goblin"))
                    C.CGroup = CardGroup.Goblin;
                else if (C.name.Contains("Dragon"))
                    C.CGroup = CardGroup.Dragon;
                else if (C.name.Contains("Wizzard"))
                    C.CGroup = CardGroup.Wizzard;
                else if (C.name.Contains("Ork"))
                    C.CGroup = CardGroup.Orks;
                else if (C.name.Contains("Knight"))
                    C.CGroup = CardGroup.Knights;
                else if (C.name.Contains("Kraken"))
                    C.CGroup = CardGroup.Kraken;
                else if (C.name.Contains("FireElf"))
                    C.CGroup = CardGroup.FireElves;
                else
                    C.CGroup = CardGroup.Normal;
            }







            User Player1 = new("user1");
            User Player2 = new("user2");

            MonsterCard Monster1 = new("1", "Monster1", 1);
            MonsterCard Monster2 = new("2", "Monster2", 2);
            MonsterCard Monster3 = new("3", "Monster3", 3);
            MonsterCard Monster4 = new("4", "Monster4", 4);
            SpellCard Spell1 = new("5", "Spell1", 1);
            SpellCard Spell2 = new("6", "Spell2", 2);
            SpellCard Spell3 = new("7", "Spell3", 3);
            SpellCard Spell4 = new("8", "Spell4", 4);
            
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

                /*
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
                */

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
                    return "Player 2 Wins! Match END!";
                }

                if (LastIndex2 < 0)
                {
                    Console.WriteLine("Player 1 Wins! Match END!");
                    return "Player 1 Wins! Match END!";
                }

                if (round > 100)
                {
                    Console.WriteLine("Round Limit reached! Match END!");
                    return "Round Limit reached! Match END!";
                }


            }


        }
    }
}
