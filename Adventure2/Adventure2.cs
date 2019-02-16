

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace Adventure
{
    class Program
    {
        static int luck = 1;
        static int health = 100;
        static int gold = 0;
        static Random random = new Random();
        static bool escaped = false;
        static Collection<string> inventory = new Collection<string>();
        static Collection<string> conditions = new Collection<string>();

        static void Main(string[] args)
        {
            inventory.Add("matches");
            inventory.Add("fish hook");
            //conditions.Add("bleeding");
            //conditions.Add("wet");
            //conditions.Add("cold");
            //conditions.Add("dry");
            //conditions.Add("catatonic");


            while (health > 0 && escaped == false)
            {

                int roomId = random.Next(1, 17);

                switch (roomId)
                {
                    case 1: GoldRoom(); break;
                    case 2: TrapedDoor(); break;
                    case 3: MedicalRoom(); break;
                    case 4: EmptyRoom(); break;
                    case 5: EscapeRoom(); break;
                    case 6: TrapedChest(); break;
                    case 7: DiceRoom(); break;
                    case 8: PotionRoom(); break;
                    case 9: ExplosiveTrapRoom(); break;
                    case 10: SwitchRoom(); break;
                    case 11: PrivateStudy(); break;
                    case 12: LockedDoor(); break;
                    case 13: BloodAlterRoom(); break;
                    case 14: ArenaRoom(); break;
                    case 15: StreamRoom(); break;
                    case 16: MerchantRoom(); break;
                    default: break;
                }

                if (health <= 0 && inventory.Contains("medical supplies"))
                {
                    inventory.Remove("medical supplies");
                    ModifyHealth(50);
                    conditions.Remove("bleeding");
                    IncreaseLuck();
                }


            }

            if (health <= 0)
            {
                Console.WriteLine("Your Dead");
            }
            else
            {
                Console.WriteLine($"You escaped with {gold} gold congradulations.");
            }

            Console.ReadKey();
        }

        private static void StreamRoom()
        {
            Collection<string> itemsToLose = new Collection<string>();

            foreach (var item in inventory)
            {
                int chanceToLoose = random.Next(1, 11);
                if (chanceToLoose <= 2)
                {
                    Console.WriteLine("While crossing the stream the current swept away a {0}", item);
                    itemsToLose.Add(item);
                }
            }

            foreach (var item in itemsToLose)
            {
                inventory.Remove(item);
            }

        }

        private static void MerchantRoom()
        {
            string buyItems = GetUserInput(
             "You found a merchants room do you wish to buy? [item number] \r\n" +
             "1: knife (20gp)\r\n" +
             "2: Blood potion (25gp)\r\n" +
             "3: medical supplies(100gp)\r\n" +
             "4: key (100gp)");
             
            if (buyItems == "1")
            {
                if (gold > 20)
                {
                    gold -= 20;
                }
                else
                {
                    health -= 20 - gold;
                    gold = 0;
                    Console.WriteLine($"Because you are missing {20 - gold} we took it out of yor flesh.");
                }
                inventory.Add("knife");

            }
            else if (buyItems == "2")
            {
                if (gold > 25)
                {
                    ModifyGold(-25);
                }
                else
                {
                    health -= 25 - gold;
                    gold = 0;
                    Console.WriteLine($"Because you are missing { 25 - gold} we took it out of yor flesh.");
                }
                inventory.Add("Blood potion");
                Console.WriteLine("you have bought a blood potion.");
            }
            else if (buyItems == "3")
            {
                if (gold > 100)
                {
                    ModifyGold(-100);
                }
                else
                {
                    health -= 100 - gold;
                    gold = 0;
                    //Console.WriteLine("Because you are missing {0} we took it out of yor flesh.", 100 - gold);
                }
                inventory.Add("medical supplies");
                Console.WriteLine("you have bought medical supplies.");
            }
            else if (buyItems == "4")
            {
                if (gold > 100)
                {
                    ModifyGold(-100);
                }
                else
                {
                    health -= 100 - gold;
                    gold = 0;
                    Console.WriteLine($"Because you are missing {20 - gold} we took it out of yor flesh.");
                }
                inventory.Add("key");
                Console.WriteLine("you have bought a key.");
            }
        }

        private static void ArenaRoom()
        {

            string fightToDeath = GetUserInput("You found an arena room do you want to fight to the death? [yes]/[no] :");
            if (fightToDeath == "yes")
            {
                int enemyHealth = random.Next(20, 100);
                while (enemyHealth >= 0 && health >= 0)
                {
                    int enemyAttack = GenerateRandomDamage(5, 15);
                    int attackDamage = random.Next(5, 15);
                    if (enemyAttack == -14)
                    {
                        if (!conditions.Contains("bleeding"))
                        {
                            conditions.Add("bleeding");
                            Console.WriteLine("you have been cut and now have bleeding");
                        }
                    }
                    if (inventory.Contains("knife"))
                    {
                        attackDamage += 10;
                    }
                    if (conditions.Contains("bleeding"))
                    {
                        enemyAttack -= 10;
                        Console.WriteLine("the enemy sees that you are bleeding");
                    }
                    enemyHealth -= attackDamage;
                    ModifyHealth(enemyAttack);
                    Console.WriteLine($"The enemy attacks for {enemyAttack} damage you did {attackDamage} damage.");
                    if (enemyHealth > 0)
                    {

                        Console.WriteLine($"Your health is {health}, the enemies is {enemyHealth}.");
                       GetUserInput("press enter to start a new round");
                        
                    }

                }
                if (enemyHealth <= 0)
                {
                    int goldWon = random.Next(200, 300);
                    ModifyGold(goldWon);
                    ModifyHealth(100);
                    Console.WriteLine($"You have won {goldWon} gold And they nursed you back to health.");
                    IncreaseLuck();
                }
            }
        }

        static void GoldRoom()
        {
            ModifyGold(50);
            GetUserInput("You found 50 gold. You move to the next room. ");
            IncreaseLuck();
        }
        static void TrapedDoor()
        {
            int damage = GenerateRandomDamage(10, 25);
            ModifyHealth(damage);
            GetUserInput($"You took {damage} damage from a trapped door. You move to the next room. ");
            DecreaseLuck();
        }

        static void MedicalRoom()
        {
            int heal = GenerateRandomHealth(25, 50);
            ModifyHealth(heal);

            GetUserInput($"You find medical supplies and heal {heal} damage. You move to the next room.");
            conditions.Remove("bleeding");
            IncreaseLuck();
        }
        static void EmptyRoom()
        {
            GetUserInput("You find nothing. You move to the next room.");
            DecreaseLuck();
        }
        static void EscapeRoom()
        {
            string escape = GetUserInput("You found a door, do you want to escape? [yes]/[no] ");
            if (escape == "yes")
            {
                escaped = true;
            }
            IncreaseLuck();
        }
        static void TrapedChest()
        {
            string openChest = GetUserInput("You found a sketchy chest, do you want to open it? [yes]/[no] ");
            if (openChest == "yes")
            {
                int chestDamage = GenerateRandomDamage(25, 35);
                ModifyHealth(chestDamage);

                int chestGold = random.Next(75, 200);
                ModifyGold(chestGold);
                Console.WriteLine($"The chest was trapped. You took {chestDamage} damage and found {chestGold} gold. You move to the next room. ");

            }



        }
        static void DiceRoom()
        {
            string answer = GetUserInput("You found dice game, how much to wager? [type amount] ");
            int wager = 0;
            int.TryParse(answer, out wager);

            if (wager > gold)
            {
                ModifyHealth(wager * -1);

                Console.WriteLine($"You cheated and lost   {wager} health!");
                DecreaseLuck();
            }
            else
            {

                if (wager > 0)
                {
                    int randomNumber = random.Next(1, 3);

                    if (randomNumber == 1)
                    {
                        ModifyGold(wager);
                        Console.WriteLine($"You won {wager} gold!");
                        IncreaseLuck();
                    }
                    else
                    {
                        ModifyGold(wager * -1);
                        Console.WriteLine($"You lost {wager} gold! {randomNumber} ");
                        DecreaseLuck();
                    }

                }
            }



        }








        static void PotionRoom()
        {
            string drinkPotion = GetUserInput("You found a mysterious potion, do you want to drink it? [yes]/[no]");




            if (drinkPotion == "yes")
            {
                int potionDamage = GenerateRandomDamage(-50, 50);
                ModifyHealth(potionDamage);
                if (potionDamage > 0)
                {
                    Console.WriteLine($"The potion heals you for {potionDamage} damage.");
                    IncreaseLuck();
                }
                else
                {
                    Console.WriteLine($"You take {potionDamage} damage from the potion.");
                    DecreaseLuck();
                }
            }
            else
            {
                inventory.Add("potion");
                Console.WriteLine("You took the potion");
            }


        }
        static void BloodAlterRoom()
        {


            string sacrificeBlood = GetUserInput("You found a blood altar, do you want to [sacrifice] or use a [blood potion]?");
            if (sacrificeBlood == "sacrifice")
            {
                int bloodDamage = GenerateRandomDamage(30, 50);
                ModifyHealth(bloodDamage);
                inventory.Add("knife");
                int hiddenGold = random.Next(50, 125);
                ModifyGold(hiddenGold);
                Console.WriteLine($"You took  {bloodDamage} damage but the alter revealed {hiddenGold} hidden gold, and a knife. You move to the next room.");
                conditions.Add("bleeding");

            }
            else if (sacrificeBlood == "blood potion")
            {
                if (inventory.Contains("blood potion"))
                {
                    Console.WriteLine("You pour the blood potion and the alter reveals 150 gold. ");

                    ModifyGold(150);
                    inventory.Remove("blood potion");
                }
            }




        }
        static void SwitchRoom()
        {
            string flipSwitch = GetUserInput("you find yourself on a wetfloor, you look around to see a switch. Do you want to flip the switch? [yes]/[no]");
            if (flipSwitch == "yes")
            {
                int switchDamage = GenerateRandomDamage(10, 29);
                ModifyHealth(switchDamage);
                int urnGold = random.Next(100, 200);
                ModifyGold(urnGold);
                Console.WriteLine($"You flip the switch electocuting yourself fortunatly you get to a dry area on the floor. You take {switchDamage} damage. Thankfully a light turns on revealing more dry areas on the floor. you look to see urns. You smash the urns to find {urnGold} gold. You move to the next room.");

            }

        }
        static void PrivateStudy()
        {

            string inspectBook = GetUserInput(" You open the door to find yourself in a private study. You rummage through the books finding a suspiciously large book. do you wish to inspect the book?");
            if (inspectBook == "yes")
            {
                int mechanismBreakChance = random.Next(1, 3);
                if (mechanismBreakChance == 1)
                {
                    int bookGold = random.Next(60, 100);
                    ModifyGold(bookGold);
                    Console.WriteLine($" You inspect the book and find a mechanism attached. You pull the book. You hear a mechanism activate, revealing a secret compartmet with {gold} gold!");
                    IncreaseLuck();
                }
                else
                {
                    Console.WriteLine("You inspect the book and find a mechanism attached. You pull the book too hard and break the mechanism. You find nothing move to the next room.");
                    DecreaseLuck();
                }
            }

        }

        static void IncreaseLuck()
        {
            luck += 1;
            Console.WriteLine("You feel lucky");
        }

        static void DecreaseLuck()
        {

            luck -= 1;
            Console.WriteLine("You feel unlucky ");

        }

        static void LockedDoor()
        {
            string searchForKey = GetUserInput("You enter the room and find a locked door. Do you wish to search for the key?");
            if (searchForKey == "yes")
            {
                int findKey = random.Next(1, 6);
                if (findKey == 1)
                {
                    int doorGold = random.Next(100, 300);
                    ModifyGold(doorGold);
                    Console.WriteLine($"You unlock the door to find {doorGold} gold in the room! You move to the next room.");

                }
                else
                {
                    Console.WriteLine("You failed to find the key and could not unlock the door. You move to the next room.");

                }
            }

        }
        static void ExplosiveTrapRoom()
        {
            string disarmTrap = GetUserInput("You enter a room and step on a trap. Do you want to attemtp to [disarm] the trap or [run] in hope it activates after you are a safe disance away?");

            if (disarmTrap == "disarm")
            {
                int disarmTrapChance = random.Next(1, 3);
                if (disarmTrapChance == 2)
                {
                    int roomTrapGold = random.Next(100, 250);
                    ModifyGold(roomTrapGold);
                    Console.WriteLine($"You successfully disarm the trap and search the room to find {roomTrapGold} Gold! You move to the next room.");
                }
                else
                {
                    int roomTrapDamage = GenerateRandomDamage(10, 30);
                    ModifyHealth(roomTrapDamage);
                    Console.WriteLine($"You fail to disarm the trap and take {roomTrapDamage} damage.");
                }
            }
            else if (disarmTrap == "run")
            {
                int runChance = random.Next(1, 2);
                if (runChance == 1)
                {
                    Console.WriteLine("You successfully run from the trap. Unfortunantly the trap collapsed the doorway and you find nothing ");

                }
                else
                {
                    int trapBlastDamage = GenerateRandomDamage(10, 30);
                    ModifyHealth(trapBlastDamage);
                    Console.WriteLine($"You run from the trap but get caught in the blast damaging you for {trapBlastDamage}.");


                }

            }
        }
        static void ModifyHealth(int changeAmount)
        {
            health += changeAmount;
            if (health > 100)
            {
                health = 100;
            }
        }
        static int GenerateRandomDamage(int min, int max)
        {
            int damage = random.Next(min, max);

            return damage * -1;
        }
        static int GenerateRandomHealth(int min, int max)
        {
            int health = random.Next(min, max);
            return health;
        }
        static void ModifyGold(int changeAmount)
        {
            gold += changeAmount;
            if (gold > 5000)
            {
                gold = 5000;
                Console.WriteLine("Your pouch of gold is full you can no longer pick up more gold.");
            }
        }
        static void DrinkPotion()
        {
            int potionDamage = GenerateRandomDamage(-50, 50);
            ModifyHealth(potionDamage);
            if (potionDamage > 0)
            {
                Console.WriteLine($"The potion heals you for {potionDamage} damage. You throw the bottle away.");
                IncreaseLuck();
            }
            else
            {
                Console.WriteLine($"You take {potionDamage} damage from the potion. You throw the bottle away.");
                DecreaseLuck();
            }
        }
        static string GetUserInput(string question)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"[gold: {gold} ");

            if (health > 30)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write($"health: {health}] ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(question + " : ");

            string answer = Console.ReadLine();

            if (answer == "inventory")
            {
                Console.WriteLine("You have the following items: ");
                foreach (var item in inventory)
                {
                    Console.WriteLine(item);
                }

                answer = GetUserInput(question);
            }
            else if(answer == "save")
            {
                File.WriteAllText("health.txt", health.ToString());
                File.WriteAllText("gold.txt", gold.ToString());
                string items = string.Empty;
                foreach (var item in inventory)
                {
                    items += item + ";";
                }
                File.WriteAllText("items.txt", items);

            }
            else if (answer == "load")
            {
                string loadedHealth = File.ReadAllText("health.txt");
                health = int.Parse(loadedHealth);

                string loadedGold = File.ReadAllText("gold.txt");
                gold = int.Parse(loadedGold);

                string loadedItems = File.ReadAllText("items.txt");

                string[] splitItems = loadedItems.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                inventory.Clear();
                foreach (var item in splitItems)
                {
                    inventory.Add(item);
                }

                Console.Clear();
                answer = GetUserInput(question);

            }
            else if (answer.Contains("use "))
            {
                if (answer.Contains(" medical supplies"))
                {
                    if (inventory.Contains("medical supplies"))
                    {
                        ModifyHealth(50);
                        inventory.Remove("medical suplies");

                    }
                   
                }
                if (answer.Contains(" potion"))
                {
                    if (inventory.Contains("potion"))
                    {
                        DrinkPotion();
                        inventory.Remove("potion");

                    }
                }
            }
            
                return answer;
            
        }






        
    }
}


