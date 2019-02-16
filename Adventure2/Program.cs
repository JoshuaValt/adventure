

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Adventure
{
    class Program
    {
        static void Main(string[] args)
        {

            int health = 100;
            int gold = 0;
            Random random = new Random();
            bool escaped = false;

            while (health > 0 && escaped == false)
            {

                int roomId = random.Next(8, 9);

                switch (roomId)
                {
                    case 1:
                        gold += 50;
                        Console.WriteLine("You found 50 gold.");
                        break;
                    case 2:
                        int damage = random.Next(1, 25);
                        health -= damage;
                        Console.WriteLine("You took " + damage + " damage.");
                        break;
                    case 3:
                        int heal = random.Next(1, 25);
                        health += heal;
                        if (health > 100)
                        {
                            health = 100;
                        }
                        Console.WriteLine("You heal " + heal + " damage.");
                        break;
                    case 4:
                        Console.WriteLine("You find nothing.");
                        break;
                    case 5:
                        Console.Write("You found a door, do you want to escape? (" + health + " health, " + gold + " gold) :");
                        string escape = Console.ReadLine();

                        if (escape == "yes")
                        {
                            escaped = true;
                        }

                        break;
                    case 6:
                        Console.Write("You found a sketchy chest, do you want to open it? (" + health + " health, " + gold + " gold) :");
                        string openChest = Console.ReadLine();

                        if (openChest == "yes")
                        {
                            int chestDamage = random.Next(0, 25);
                            health -= chestDamage;

                            int chestGold = random.Next(200, 500);
                            gold += chestGold;
                            Console.WriteLine("You took " + chestDamage + " damage and found " + chestGold + " gold.");


                        }
                        break;
                    case 7:
                        Console.Write("You found dice game, how much to wager? (" + health + " health, " + gold + " gold) :");
                        int wager = 0;
                        int.TryParse(Console.ReadLine(), out wager);

                        if (wager > gold)
                        {
                            health -= wager;
                            Console.WriteLine("You cheated and lost " + wager + " health!");
                        }
                        else
                        {

                            if (wager > 0) ;
                            {
                                int randomNumber = random.Next(1, 3);

                                if (randomNumber == 1)
                                {
                                    gold += wager;
                                    Console.WriteLine("You won " + wager + " gold!");
                                }
                                else
                                {
                                    gold -= wager;
                                    Console.WriteLine("You lost " + wager + " gold!  " + randomNumber + " ");
                                }

                            }
                        }
                        break;
                    case 8:
                        Console.Write("You found a mysterious potion, do you want to drink it? : ");
                            string drinkPotion = Console.ReadLine();
                        if (drinkPotion == "yes")
                        {
                            int potionDamage = random.Next(-25, 50);
                            health += potionDamage;
                            if (health > 100)
                            {
                                health = 100;
                            }
                            if (potionDamage >0)
                            {
                                Console.WriteLine("The potion heals you for " + potionDamage + " damage.");

                            }
                            else
                            {
                                Console.WriteLine("You take damage from the potion " + potionDamage);

                            }
                        }
                        




                        break;
                    default:

                        break;
                }

            }

            if (health <= 0)
            {
                Console.WriteLine("Your Dead");
            }
            else
            {
                Console.WriteLine("You escaped with " + gold + " gold.");
            }




            Console.ReadKey();


        }

        static void Test()
        {
            int howManyMathClasses = 3;
            bool DoesJoshLikeMathClasses = false;
            double HowMuchMoneyDoYouHave = 120.10;
            string JoshsLifeIdea = "Life is to live.";
            bool isJoshHappy = true;
            List<string> itemIHave = new List<string>();
            string whatUserTyped;

            itemIHave.Add("shirt");
            itemIHave.Add("knife");
            itemIHave.Add("shoes");

            itemIHave.Remove("shirt");

            if (itemIHave.Contains("knife"))
            {
                isJoshHappy = true;
            }

            foreach (var item in itemIHave)
            {
                Console.WriteLine(item);
            }

            Console.Write("How much money does Josh have? : ");
            whatUserTyped = Console.ReadLine();
            HowMuchMoneyDoYouHave = double.Parse(whatUserTyped);

            if (HowMuchMoneyDoYouHave < 150)
            {
                isJoshHappy = false;
            }


            if (howManyMathClasses >= 4)
            {
                isJoshHappy = false;
            }

            JoshsLifeIdea = JoshsLifeIdea + " Excellent!";

            if (isJoshHappy == true)
            {
                Console.WriteLine("Josh is happy");
            }
            else
            {
                Console.WriteLine("Josh is not happy");
            }
            Console.ReadKey();
        }
    }
}


