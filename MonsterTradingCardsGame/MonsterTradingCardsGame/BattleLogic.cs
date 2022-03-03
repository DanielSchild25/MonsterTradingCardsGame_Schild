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
            Card[] CardArray = new Card[8];

            P1Ids = await Database.Base.Read("*", "decks", new() { { "username", player1 } });
            P2Ids = await Database.Base.Read("*", "decks", new() { { "username", player2 } });

            result = await Database.Base.Read("*", "cards", new() { { "id", P1Ids["card10"] } });
            Card P1Card1 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray[0] = P1Card1;

            result = await Database.Base.Read("*", "cards", new() { { "id", P1Ids["card20"] } });
            Card P1Card2 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray[1] = P1Card2;

            result = await Database.Base.Read("*", "cards", new() { { "id", P1Ids["card30"] } });
            Card P1Card3 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray[2] = P1Card3;

            result = await Database.Base.Read("*", "cards", new() { { "id", P1Ids["card40"] } });
            Card P1Card4 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray[3] = P1Card4;

            result = await Database.Base.Read("*", "cards", new() { { "id", P2Ids["card10"] } });
            Card P2Card1 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray[4] = (P2Card1);

            result = await Database.Base.Read("*", "cards", new() { { "id", P2Ids["card20"] } });
            Card P2Card2 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray[5] = P2Card2;

            result = await Database.Base.Read("*", "cards", new() { { "id", P2Ids["card30"] } });
            Card P2Card3 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray[6] = P2Card3;

            result = await Database.Base.Read("*", "cards", new() { { "id", P2Ids["card40"] } });
            Card P2Card4 = new((string)result["id0"], (string)result["name0"], (int)result["damage0"]);
            CardArray[7] = P2Card4;

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


            List<Card> P1 = new()
            {
                P1Card1,
                P1Card2,
                P1Card3,
                P1Card4
            };

            List<Card> P2 = new()
            {
                P2Card1,
                P2Card2,
                P2Card3,
                P2Card4
            };


            while (true)
            {
                Random RandomCardIndex = new();
                
                //Card Last = Player1.deck.Last();
                //int LastIndex1 = Player1.deck.LastIndexOf(Last);
                Console.WriteLine(LastIndex1);
                int Index1 = RandomCardIndex.Next(0, LastIndex1);
                Console.WriteLine(Index1);
                Card Player1Card = P1[Index1];

                //Last = Player1.deck.Last();
                //int LastIndex2 = Player1.deck.LastIndexOf(Last);
                Console.WriteLine(LastIndex2);
                int Index2 = RandomCardIndex.Next(0, LastIndex2);
                Console.WriteLine(Index2);
                Card Player2Card = P2[Index2];

                float P1Dmg = Player1Card.damage;
                float P2Dmg = Player2Card.damage;

                //unique feature 
                Random DmgChange = new();
                float Change1 = DmgChange.Next(75, 125);
                Change1 /= 100;
                P1Dmg *= Change1;

                float Change2 = DmgChange.Next(75, 125);
                Change2 /= 100;
                P2Dmg *= Change2;


                if (Player1Card.CType == CardType.Spell)
                {
                    if (Player1Card.EType == ElementType.Water && Player2Card.EType == ElementType.Fire)
                        P1Dmg = Player1Card.damage * 2;
                    else if (Player2Card.EType == ElementType.Water && Player1Card.EType == ElementType.Fire)
                        P1Dmg = Player1Card.damage / 2;
                    else if (Player1Card.EType == ElementType.Fire && Player2Card.EType == ElementType.Normal)
                        P1Dmg = Player1Card.damage * 2;
                    else if (Player2Card.EType == ElementType.Fire && Player1Card.EType == ElementType.Normal)
                        P1Dmg = Player1Card.damage / 2;
                    else if (Player1Card.EType == ElementType.Normal && Player2Card.EType == ElementType.Water)
                        P1Dmg = Player1Card.damage * 2;
                    else if (Player2Card.EType == ElementType.Normal && Player1Card.EType == ElementType.Water)
                        P1Dmg = Player1Card.damage / 2;
                }

                if (Player2Card.CType == CardType.Spell)
                {
                    if (Player2Card.EType == ElementType.Water && Player1Card.EType == ElementType.Fire)
                        P2Dmg = Player2Card.damage * 2;
                    else if (Player1Card.EType == ElementType.Water && Player2Card.EType == ElementType.Fire)
                        P2Dmg = Player2Card.damage / 2;
                    else if (Player2Card.EType == ElementType.Fire && Player1Card.EType == ElementType.Normal)
                        P2Dmg = Player2Card.damage * 2;
                    else if (Player1Card.EType == ElementType.Fire && Player2Card.EType == ElementType.Normal)
                        P2Dmg = Player2Card.damage / 2;
                    else if (Player2Card.EType == ElementType.Normal && Player1Card.EType == ElementType.Water)
                        P2Dmg = Player2Card.damage * 2;
                    else if (Player1Card.EType == ElementType.Normal && Player2Card.EType == ElementType.Water)
                        P2Dmg = Player2Card.damage / 2;
                }

                if (Player1Card.CGroup == CardGroup.Goblin && Player2Card.CGroup == CardGroup.Dragon)
                    P1Dmg = 0;
                if (Player2Card.CGroup == CardGroup.Goblin && Player1Card.CGroup == CardGroup.Dragon)
                    P2Dmg = 0;
                if (Player1Card.CGroup == CardGroup.Wizzard && Player2Card.CGroup == CardGroup.Orks)
                    P2Dmg = 0;
                if (Player2Card.CGroup == CardGroup.Wizzard && Player1Card.CGroup == CardGroup.Orks)
                    P1Dmg = 0;
                if (Player1Card.CGroup == CardGroup.Knights && Player2Card.CType == CardType.Spell && Player2Card.EType == ElementType.Water)
                    P1Dmg = 0;
                if (Player2Card.CGroup == CardGroup.Knights && Player1Card.CType == CardType.Spell && Player1Card.EType == ElementType.Water)
                    P2Dmg = 0;
                if (Player1Card.CGroup == CardGroup.Kraken && Player2Card.CType == CardType.Spell)
                    P2Dmg = 0;
                if (Player2Card.CGroup == CardGroup.Kraken && Player1Card.CType == CardType.Spell)
                    P1Dmg = 0;
                if (Player1Card.CGroup == CardGroup.FireElves && Player2Card.CGroup == CardGroup.Dragon)
                    P2Dmg = 0;
                if (Player2Card.CGroup == CardGroup.FireElves && Player1Card.CGroup == CardGroup.Dragon)
                    P1Dmg = 0;
                

                Console.WriteLine($"Round {round}: P1: '{Player1Card.name}' ({P1Dmg}) VS P2: '{Player2Card.name}' ({P2Dmg}) ");

                if (P1Dmg == P2Dmg)
                {
                    Console.WriteLine($"Round {round}: Draw!");
                }
                else if (P1Dmg > P2Dmg)
                {

                    P1.Add(P2[Index2]);
                    P2.Remove(P2[Index2]);
                    LastIndex1++;
                    LastIndex2--;
                    Console.WriteLine($"Round {round} Result: Player 1 won! Card '{Player2Card.name}' moved from Player2s deck to Player 1s deck.");
                }
                else if (P1Dmg < P2Dmg)
                {
                    P2.Add(P1[Index1]);
                    P1.Remove(P1[Index1]);
                    LastIndex1--;
                    LastIndex2++;
                    Console.WriteLine($"Round {round} Result: Player 2 won! Card '{Player1Card.name}' moved from Player 1s deck to Player 2s deck.");
                }
                round++;

                if (LastIndex1 < 0)
                {
                    await User.BattleWin(player2);
                    await User.BattleLose(player1);
                    Console.WriteLine($"{player2} Wins! Match END!");
                    return $"{player2} Wins! Match END!";
                }

                if (LastIndex2 < 0)
                {
                    await User.BattleWin(player1);
                    await User.BattleLose(player2);
                    Console.WriteLine($"{player1} Wins! Match END!");
                    return $"{player1} Wins! Match END!";
                }

                if (round > 100)
                {
                    await User.BattleDraw(player1);
                    await User.BattleDraw(player2);
                    Console.WriteLine("Round Limit reached! Match END!");
                    return "Round Limit reached! Match END!";
                }


            }


        }
    }
}
