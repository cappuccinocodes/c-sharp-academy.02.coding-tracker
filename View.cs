using Coding_Tracker.models;
using ConsoleTableExt;
using System.Globalization;

namespace Coding_Tracker
{
    internal class View
    {
        Controller _controller;
        Validation _validation;
        internal View(Controller controller, Validation validation)
        {
            _controller = controller;
            _validation = validation;
        }

        internal void Start()
        {
            while(true)
            {
                Console.Clear();
                DisplayMessage("Welcome to the coding tracker!");
                // is there a more elegant way of handling this other than asking more than once?
                string? input = "";
                do
                {
                    DisplayMenuOptions();
                    input = GetUserInput("Choose an option: ");
                    if (!_validation.IsValidNumber(input)) DisplayMessage("Invalid option. Please try again.");
                } while (!_validation.IsValidNumber(input));

                MenuOption response = _controller.GetMenuOption(input);

                Console.Clear();
                switch (response)
                {
                    case MenuOption.Add:
                        Add();
                        break;
                    case MenuOption.Update:
                        Update();
                        break;
                    case MenuOption.DisplayAll:
                        DisplayAll();
                        break;
                    case MenuOption.Delete:
                        Delete();
                        break;
                    case MenuOption.Exit:
                        Environment.Exit(0);
                        break;
                    case MenuOption.Invalid:
                        GetUserInput("Invalid option. Please press any key to try again.");
                        break;
                }
            }
        }

        internal void DisplayMenuOptions() {
            // is the type object because the external library "doesn't know" the object type so this keeps it generic?
            var tableData = new List<List<object>>
            {
                new List<object> {"1", "Add coding habit"},
                new List<object> {"2", "Update coding habit"},
                new List<object> {"3", "Remove coding habit"},
                new List<object> {"4", "View all coding habits"},
                new List<object> {"0", "Exit program"},
            };

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Menu")
                .ExportAndWriteLine();
        }

        internal void Add()
        {
            DateTime startDate;
            DateTime endDate;

            DisplayMessage("Add habit:");
            string? startDateInput;
            do
            {
                startDateInput = GetUserInput("Enter start date in the form dd/mm/yyyy hh:mm:ss: ");
            } while (!DateTime.TryParseExact(startDateInput, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB"), DateTimeStyles.None, out startDate));

            string? endDateInput;
            do
            {
                endDateInput = GetUserInput("Enter end date in the form dd/mm/yyyy hh:mm:ss: ");
            } while (!DateTime.TryParseExact(endDateInput, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB"), DateTimeStyles.None, out endDate));

            if (!_validation.IsValidTimeSpan(startDate, endDate))
            {
                DisplayMessage("The end date must follow the start date!");
                GetUserInput("Press any key to return to main menu.");
                return;
            }

            // is this redundant when I could/should call the DB method directly? Is there anything in this program that should be in the controller?
            _controller.AddHabit(startDate, endDate);

            DisplayMessage("Habit successfully added!");
            GetUserInput("Press any key to return to main menu.");
        }

        internal void Update()
        {
            DateTime startDate;
            DateTime endDate;

            DisplayMessage("Update habit:");
            string? input;
            do
            {
                input = GetUserInput("Enter an id: ");
            } while (!_validation.IsValidNumber(input));

            int id = Int32.Parse(input);

            var originalHabitList = _controller.SelectOne(id);

            if (originalHabitList.Count() == 0)
            {
                DisplayMessage("That habit wasn't found!");
                GetUserInput("Press any key to return to main menu.");
                return;
            }

            var originalHabit = originalHabitList.First();

            string? startDateInput;
            do
            {
                DisplayMessage($"Start date is currently: {originalHabit.StartDate}");
                startDateInput = GetUserInput("Enter start date in the form dd/mm/yyyy hh:mm:ss: ");
            } while (!DateTime.TryParseExact(startDateInput, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB"), DateTimeStyles.None, out startDate));

            string? endDateInput;
            do
            {
                DisplayMessage($"End date is currently: {originalHabit.StartDate}");
                endDateInput = GetUserInput("Enter end date in the form dd/mm/yyyy hh:mm:ss: ");
            } while (!DateTime.TryParseExact(endDateInput, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB"), DateTimeStyles.None, out endDate));

            if (!_validation.IsValidTimeSpan(startDate, endDate))
            {
                DisplayMessage("The end date must follow the start date!");
                GetUserInput("Press any key to return to main menu.");
                return;
            }

            bool isSuccess = _controller.UpdateHabit(originalHabit.Id, startDate, endDate);

            if (isSuccess)
            {
                DisplayMessage("Habit successfully updated!");
                GetUserInput("Press any key to return to main menu.");
            }
            else
            {
                DisplayMessage("There was an error updating the habit!");
                GetUserInput("Press any key to return to main menu.");
            }
        }

        internal void Delete()
        {
            DisplayMessage("Delete habit:");
            string? input;
            do
            {
                input = GetUserInput("Enter an id: ");
            } while (!_validation.IsValidNumber(input));

            int id = Int32.Parse(input);
            bool isSuccess = _controller.DeleteHabit(id);

            if (isSuccess)
            {
                DisplayMessage("Habit successfully deleted!");
                GetUserInput("Press any key to return to main menu.");
            }
            else
            {
                DisplayMessage("There was an error deleting the habit!");
                GetUserInput("Press any key to return to main menu.");
            }
        }

        internal void DisplayAll()
        {
            var codingHabits = _controller.SelectAll();

            // is this the best way to create the objects?
            var tableData = new List<List<object>>();

            foreach (CodingHabit habit in codingHabits)
            {
                List<object> habitToAdd = new List<object> { habit.Id, habit.StartDate, habit.EndDate, habit.TimeSpan };
                tableData.Add(habitToAdd);
            };

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("All habits")
                .WithColumn("Id", "Start date", "End date", "Duration")
                .ExportAndWriteLine();

            GetUserInput("Press any key to return to main menu.");
        }

        internal void DisplayMessage(string message) {
            Console.WriteLine(message);
        }

        internal string? GetUserInput(string request)
        {
            Console.Write(request);
            return Console.ReadLine();
        }
    }
}
