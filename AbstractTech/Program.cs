using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AbstractTech
{
    class Program
    {
        static Vector2 position;
        static Vector2 size;
        static Direction facing;
        static List<string> commands;

        static bool running;
        static bool executionFailed;

        static void Main(string[] args)
        {
            position = new Vector2(1, 1);
            size = new Vector2(100, 100);
            facing = Direction.SOUTH;
            commands = new List<string>();

            string input;
            bool executeCommands = false;
            executionFailed = false;
            running = true;

            DisplayLocation();
            DisplayBeginningMessage();

            //main program loop
            while (running)
            {
                if (executeCommands)
                {
                    foreach (string command in commands)
                    {
                        if (executionFailed) continue;
                        switch (command)
                        {
                            case "left":
                                TurnLeft();
                                break;
                            case "right":
                                TurnRight();
                                break;
                            default:
                                int distance = Convert.ToInt32(command.Split('m')[0]);
                                Move(distance);
                                break;
                        }
                    }
                    if(executionFailed) Console.WriteLine("Attempt to Move Out of Bounds, Stopping Execution.");

                    executeCommands = false;
                    executionFailed = false;
                    DisplayLocation();
                    DisplayBeginningMessage();
                }
                else
                {
                    input = Console.ReadLine();
                    input = input.ToLower();

                    if (input == "exit")
                    {
                        running = false;
                        continue;
                    }

                    if (ValidateCommand(input))
                    {
                        commands.Add(input);
                        if (commands.Count == 5) executeCommands = true;
                    }
                    else if (input == "") executeCommands = true;
                    else Console.WriteLine("Command Not Recognised, Please Enter A New Command:");
                }
            }
        }

        private static void Move(int distance)
        {
            switch (facing)
            {
                case Direction.NORTH:
                    position.Y -= distance;
                    break;
                case Direction.EAST:
                    position.X += distance;
                    break;
                case Direction.SOUTH:
                    position.Y += distance;
                    break;
                case Direction.WEST:
                    position.X -= distance;
                    break;
            }
            CheckBounds();
        }

        /*checks if the rover has been moved out of bounds,
        moves it to the edge and then sets the execution
        failed flag to true to stop further command execution*/
        static void CheckBounds()
        {
            if(position.X > size.X)
            {
                position.X = 100;
                executionFailed = true;
            }
            else if(position.X < 1)
            {
                position.X = 1;
                executionFailed = true;
            }
            else if(position.Y < 1)
            {
                position.Y = 1;
                executionFailed = true;
            }
            else if(position.Y > size.Y)
            {
                position.Y = 100;
                executionFailed = true;
            }
        }

        //simple regex to make sure nothing other than left,right or distances are being entered as commands
        private static bool ValidateCommand(string input)
        {
            Regex validCommand = new Regex("left|right|(\\d+m{1})");

            return validCommand.Match(input).Success;
        }

        //calculates the current grid reference based on the vector position of the rover
        static void DisplayLocation()
        {
            int gridRef = (((int)position.Y - 1) * 100) + (int)position.X;
            string location = gridRef.ToString();
            switch (facing)
            {
                case Direction.NORTH:
                    location += " north";
                    break;
                case Direction.EAST:
                    location += " east";
                    break;
                case Direction.SOUTH:
                    location += " south";
                    break;
                case Direction.WEST:
                    location += " west";
                    break;
                default:
                    location = "Error";
                    break;
            }

            Console.WriteLine(location);
        }

        static void DisplayBeginningMessage() { Console.WriteLine("Please Enter a Command or Enter Blank Command to Exectute:"); }

        static void TurnLeft()
        {
            int parsedDirection = (int)facing;
            parsedDirection--;
            if (parsedDirection == -1) facing = Direction.WEST;
            else facing = (Direction)parsedDirection;
        }

        static void TurnRight()
        {
            int parsedDirection = (int)facing;
            parsedDirection++;
            if (parsedDirection == 4) facing = Direction.NORTH;
            else facing = (Direction)parsedDirection;
        }
    }
}
