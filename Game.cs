﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{
    struct player
    {
        public string Name;
        public int Health;
        public int Damage;
        public int Defense;
    }
    class Game      //WIP: switching between enemies.
    {
        player player;
        bool _gameOver = false;
        string _playerName = "Hero";
        int _playerHealth = 120;
        int _playerDamage = 20;
        int _playerDefense = 10;
        int levelScaleMax = 5;
        //Run the game
        public void Run()
        {
            Start();

            while (_gameOver == false)
            {
                Update();
            }

            End();
        }
        //This function handles the battles for our ladder. roomNum is used to update our opponent to be the enemy in the current room.
        //turnCount is used to keep track of how many turns it took the player to beat the enemy
        bool StartBattle(int roomNum, ref int turnCount)
        {
            //initialize default enemy stats
            int enemyHealth = 0;
            int enemyAttack = 0;
            int enemyDefense = 0;
            string enemyName = "";
            //Changes the enemy's default stats based on our current room number.
            //This is how we make it seem as if the player is fighting different enemies
            switch (roomNum)
            {
                case 0:
                    {
                        enemyHealth = 100;
                        enemyAttack = 20;
                        enemyDefense = 5;
                        enemyName = "Wizard";
                        break;
                    }
                case 1:
                    {
                        enemyHealth = 80;
                        enemyAttack = 30;
                        enemyDefense = 5;
                        enemyName = "Troll";
                        break;
                    }
                case 2:
                    {
                        enemyHealth = 200;
                        enemyAttack = 40;
                        enemyDefense = 10;
                        enemyName = "Giant";
                        break;
                    }
                case 3:
                    {
                        enemyHealth = 20000;
                        enemyAttack = 100;
                        enemyDefense = 20;
                        enemyName = "Goliath";
                        break;
                    }
            }

            //Loops until the player or the enemy is dead
            while (player.Health > 0 && enemyHealth > 0)
            {
                //Displays the stats for both charactersa to the screen before the player takes their turn
                PrintStats(player);
                PrintStats(enemyName, enemyHealth, enemyAttack, enemyDefense);

                //Get input from the player
                char input;
                GetInput(out input, "Attack", "Defend", "what do you do?");
                //If input is 1, the player wants to attack. By default the enemy blocks any incoming attack
                if (input == '1')
                {
                    BlockAttack(ref enemyHealth, player.Damage, enemyDefense);
                    Console.WriteLine("\nYou dealt " + (player.Damage - enemyDefense) + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    if (enemyHealth <= 0)
                    {
                        turnCount++;
                        Console.Clear();
                        Console.WriteLine(enemyName + " has been slain.");
                        Console.ReadKey();
                        break;
                    }
                }
                //If the player decides to defend the enemy just takes their turn. However this time the block attack function is
                //called instead of simply decrementing the health by the enemy's attack value.
                else if (input == '2')
                {
                    BlockAttack(ref player.Health, enemyAttack, player.Defense);
                    Console.WriteLine("\n" + enemyName + " dealt " + (enemyAttack - player.Defense) + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                //After the player attacks, the enemy takes its turn. Since the player decided not to defend, the block attack function is not called.
                player.Health -= enemyAttack;
                Console.WriteLine(enemyName + " dealt " + enemyAttack + " damage.");
                Console.Write("> ");
                Console.ReadKey();
                Console.Clear();
                turnCount++;

            }
            //Return whether or not our player died
            return _playerHealth != 0;

        }
        //Decrements the health of a character. The attack value is subtracted by that character's defense
        int BlockAttack(ref int opponentHealth, int attackVal, int opponentDefense)
        {
            int damage = attackVal - opponentDefense;
            if (damage <= 0)
            {
                damage = 0;
            }
            return opponentHealth -= damage;
        }
        //Scales up the player's stats based on the amount of turns it took in the last battle
        void UpgradeStats(int turnCount)
        {
            //Subtract the amount of turns from our maximum level scale to get our current level scale
            int scale = levelScaleMax - turnCount;
            if (scale <= 0)
            {
                scale = 1;
            }
            player.Health += 10 * scale;
            player.Damage *= scale;
            player.Defense *= scale;
        }
        void UpgradeStats(int turnCount, string query)
        {
            Random random;
            random = new Random();
            int randomNumber = random.Next(0, 10);
            Console.WriteLine(query);
            //Subtract the amount of turns from our maximum level scale to get our current level scale
            int scale = levelScaleMax - turnCount;
            if (scale <= 0)
            {
                scale = 1;
            }
            char input;
            GetInput(out input, "Attack Up", "Defense Up", "Heal", "what would you like to buy?");
            if (input == '1')
            {
                player.Damage *= 2 * scale;
            }
            else if (input == '2')
            {
                player.Defense *= 2 * scale;
            }
            else if (input == '3')
            {
                if (randomNumber > 5)
                {
                    Console.Clear();
                    player.Health += 50;
                    Console.WriteLine("you healed 50 health");
                    Console.ReadKey();
                }
                else
                {
                    Console.Clear();
                    player.Health += 25;
                    Console.WriteLine("you healed 25 health");
                    Console.ReadKey();
                }
            }
            player.Health += 10 * scale;
            Console.Clear();
        }
        //Gets input from the player
        //Out's the char variable given. This variables stores the player's input choice.
        //The parameters option1 and option 2 displays the players current chpices to the screen
        void GetInput(out char input, string option1, string option2, string query)
        {
            //Initialize input
            input = ' ';
            Console.WriteLine(query);
            //Loop until the player enters a valid input
            while (input != '1' && input != '2')
            {
                Console.WriteLine("1." + option1);
                Console.WriteLine("2." + option2);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
            }
        }
        void GetInput(out char input, string option1, string option2, string option3, string query)
        {
            //Initialize input
            input = ' ';
            Console.WriteLine(query);
            //Loop until the player enters a valid input
            while (input != '1' && input != '2' && input != '3')
            {
                Console.WriteLine("1." + option1);
                Console.WriteLine("2." + option2);
                Console.WriteLine("3." + option3);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
            }
        }

        //Prints the stats given in the parameter list to the console
        void PrintStats(string name, int health, int damage, int defense)
        {
            Console.WriteLine("\n" + name);
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Damage: " + damage);
            Console.WriteLine("Defense: " + defense);
        }

        void PrintStats(player player)
        {
            Console.WriteLine("\n" + player.Name);
            Console.WriteLine("Health: " + player.Health);
            Console.WriteLine("Damage: " + player.Damage);
            Console.WriteLine("Defense: " + player.Defense);
        }

        //This is used to progress through our game. A recursive function meant to switch the rooms and start the battles inside them.
        void ClimbLadder(int roomNum)
        {
            //Displays context based on which room the player is in
            switch (roomNum)
            {
                case 0:
                    {
                        Console.WriteLine("A wizard blocks your path");
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("A troll stands before you");
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("A giant has appeared!");
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("A goliath has appeared!");
                        break;
                    }
                default:
                    {
                        _gameOver = true;
                        return;
                    }
            }
            int turnCount = 0;
            //Starts a battle. If the player survived the battle, level them up and then proceed to the next room.
            if (StartBattle(roomNum, ref turnCount))
            {
                if (roomNum == 3)
                {
                    return;
                }
                //char input;
                GetInput(out char input, "continue", "visit shop", "before continuing on, would you like to visit the shop?");
                if (input == '1')
                {
                    Console.Clear();
                    UpgradeStats(turnCount);
                }
                else if (input == '2')
                {
                    Console.Clear();
                    UpgradeStats(turnCount, "you enter the shop");
                }
                ClimbLadder(roomNum + 1);
            }
            _gameOver = true;

        }

        //Displays the character selection menu. 
        void SelectCharacter()
        {
            char input = ' ';
            //Loops until a valid option is choosen
            while (input != '1' && input != '2' && input != '3')
            {
                //Prints options
                Console.WriteLine("Welcome! Please select a character.");
                Console.WriteLine("1.Sir Kibble");
                Console.WriteLine("2.Gnojoel");
                Console.WriteLine("3.Joedazz");
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
                //Sets the players default stats based on which character was picked
                switch (input)
                {
                    case '1':
                        {
                            player.Name = "Sir Kibble";
                            player.Health = 120;
                            player.Defense = 10;
                            player.Damage = 40;
                            break;
                        }
                    case '2':
                        {
                            player.Name = "Gnojoel";
                            player.Health = 40;
                            player.Defense = 2;
                            player.Damage = 70;
                            break;
                        }
                    case '3':
                        {
                            player.Name = "Joedazz";
                            player.Health = 200;
                            player.Defense = 5;
                            player.Damage = 25;
                            break;
                        }
                    //If an invalid input is selected display and input message and input over again.
                    default:
                        {
                            Console.WriteLine("Invalid input. Press any key to continue.");
                            Console.Write("> ");
                            Console.ReadKey();
                            break;
                        }
                }
                Console.Clear();
            }
            //Prints the stats of the choosen character to the screen before the game begins to give the player visual feedback
            PrintStats(player);
            Console.WriteLine("Press any key to continue.");
            Console.Write("> ");
            Console.ReadKey();
            Console.Clear();
        }

        //Performed once when the game begins
        public void Start()
        {
            SelectCharacter();
        }

        //Repeated until the game ends
        public void Update()
        {
            ClimbLadder(0);
        }

        //Performed once when the game ends
        public void End()
        {
            //If the player died print death message
            if (player.Health <= 0)
            {
                Console.WriteLine("Failure");
                return;
            }
            //Print game over message
            Console.WriteLine("Congrats");
        }
    }
}