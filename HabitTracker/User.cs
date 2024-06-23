using System.Globalization;

namespace Habit_Tracker
{
    internal class User
    {
        readonly DatabaseHandler DB = new DatabaseHandler();
        public User()
        {

        }

        public void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMain Menu");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to Initalize New Habit");
                Console.WriteLine("Type 2 to Delete A Habit");
                Console.WriteLine("Type 3 to View All Records.");
                Console.WriteLine("Type 4 to Insert Record.");
                Console.WriteLine("Type 5 to Delete Record.");
                Console.WriteLine("Type 6 to Update Record.");
                Console.WriteLine("-------------------------------------\n");

                string? userInput = Console.ReadLine();


                switch (userInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        InsertHabit();
                        break;
                    case "2":
                        DeleteHabit();
                        break;
                    case "3":
                        Read();
                        break;
                    case "4":
                        Insert();
                        break;
                    case "5":
                        Delete();
                        break;
                    case "6":
                        Update();
                        break;
                    default:
                        Console.WriteLine("\nNot Valid Option. Please Enter a number from 0 to 4\n");
                        break;
                }

            }
        }

        private void InsertHabit()
        {
            Console.Clear();
            DB.GetHabits();
            string habit = GetStringInput("\n\nInsert the name of habit you want to add\n\n");
            string unit = GetStringInput("\n\nInsert the unit to measure this habit\n\n");

            if (!DB.CheckIfunitExists(unit))
            {
                DB.InsertUnit(unit);
            }


            if (DB.CheckIfHabitExists(habit))
            {
                Console.WriteLine($"\n\nHabit {habit} already Exists\n\n");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                GetUserInput();
            }
            else
            {
                DB.InsertHabit(habit, unit);
            }

        }

        private void DeleteHabit()
        {
            DB.GetHabits();

            Console.WriteLine("Type 0 to return to main menu");
            var recordId = GetNumberInput("\n\nEnter the Id you want to DELETE:\n\n");

            if (DB.CheckIfHabitExists(recordId))
            {
                DB.DeleteHabit(recordId);
                Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
            }
            else
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                Console.WriteLine($"Press Enter to continue");
                Console.ReadLine();
                this.DeleteHabit();
            }

        }

        private void Read()
        {
            Console.Clear();
            DB.GetAllRecords();
        }

        private void Insert()
        {
            Console.Clear();
            DB.GetHabits();

            Console.WriteLine("Type 0 to return to main menu");
            var recordId = GetNumberInput("\n\nEnter the Id of the habit you completed:\n\n");


            if (DB.CheckIfHabitExists(recordId))
            {
                string date = GetDateInput();
                int quantity = GetNumberInput("\n\nInsert quantity you achieved:\n\n");
                DB.InsertRecord(recordId, date, quantity);
            }
            else
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                Console.WriteLine($"Press Enter to continue");
                Console.ReadLine();
                this.Insert();
            }



        }

        private void Delete()
        {
            DB.GetAllRecords();

            Console.WriteLine("Type 0 to return to main menu");
            var recordId = GetNumberInput("\n\nEnter the Id you want to DELETE:\n\n");

            if (DB.CheckIfRowExists(recordId))
            {
                DB.DeleteRecord(recordId);
                Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
            }
            else
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                Console.WriteLine($"Press Enter to continue");
                Console.ReadLine();
                this.Delete();
            }

        }

        private void Update()
        {
            Console.Clear();
            DB.GetAllRecords();

            Console.WriteLine("\n\nType 0 to return to main menu");
            var recordId = GetNumberInput("\n\nEnter the Id you want to Update:\n\n");

            if (DB.CheckIfRowExists(recordId))
            {
                Console.Clear();
                Console.WriteLine("\n\nUpdate Menu\n");
                Console.WriteLine("Type 0 to return to Main Menu");
                Console.WriteLine("Type 1 to Update date");
                Console.WriteLine("Type 2 to Update quantity");
                Console.WriteLine("Type 3 to Update Both Date and Quantity");
                Console.WriteLine("-------------------------------------\n");

                string? userInput = Console.ReadLine();
                string date = null;
                int quantity = 0;

                switch (userInput)
                {
                    case "0":
                        this.GetUserInput();
                        break;
                    case "1":
                        date = GetDateInput();
                        DB.UpdateRecord(recordId, date);
                        break;
                    case "2":
                        quantity = GetNumberInput("\n\nInsert the new quantity:\n\n");
                        DB.UpdateRecord(recordId, quantity);
                        break;
                    case "3":
                        date = GetDateInput();
                        quantity = GetNumberInput("\n\nInsert the new quantity:\n\n");
                        DB.UpdateRecord(recordId, date, quantity);
                        break;
                    default:
                        Console.WriteLine("\nNot Valid Option. Please Enter a number from 0 or 1\n");
                        this.Update();
                        break;
                }

                Console.WriteLine($"\n\nRecord with Id {recordId} was updated. \n\n");
            }
            else
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                Console.WriteLine($"Press Enter to continue");
                Console.ReadLine();
                this.Update();

            }

        }


        private string GetDateInput()
        {
            Console.WriteLine("Type 0 to return to main menu");
            Console.WriteLine("\n\nInsert Date (dd-MM-yy): ");

            string? dateInput = Console.ReadLine();

            if (dateInput == "0") this.GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. Formate: (dd-MM-yy)\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;

        }

        private int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") this.GetUserInput();


            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number.\n\n");
                numberInput = Console.ReadLine();
            }

            int result = Convert.ToInt32(numberInput);

            return result;

        }

        private string GetStringInput(string message)
        {
            Console.WriteLine(message);

            string stringInput = Console.ReadLine();

            if (stringInput == "0") this.GetUserInput();

            while (stringInput.Any(char.IsDigit))
            {
                Console.WriteLine("\n\nCan't contain digits. Try again\n\n");
                stringInput = Console.ReadLine();
            }

            return stringInput;

        }
    }
}
