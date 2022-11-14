using Coding_Tracker.models;

namespace Coding_Tracker
{
    internal class Controller
    {
        // is this OK?  how to start Initialize() method, i.e. from constructor?
        Database _database = new();

        internal MenuOption GetMenuOption(string input)
        {
            int inputAsNumber = int.Parse(input);

            return inputAsNumber switch
            {
                0 => MenuOption.Exit,
                1 => MenuOption.Add,
                2 => MenuOption.Update,
                3 => MenuOption.Delete,
                4 => MenuOption.DisplayAll,
                _ => MenuOption.Invalid,
            };
        }

        internal void AddHabit(DateTime startDate, DateTime endDate)
        {
            CodingHabit habit = new CodingHabit { StartDate = startDate, EndDate = endDate };
            _database.Insert(habit);
        }

        internal bool UpdateHabit(int id, DateTime startDate, DateTime endDate)
        {
            var updatedHabit = new CodingHabit
            {
                Id = id,
                StartDate = startDate,
                EndDate = endDate,
            };

            return _database.Update(updatedHabit);
        }

        internal bool DeleteHabit(int id)
        {
            int rowsDeleted = _database.Delete(id);
            if (rowsDeleted > 0) return true;
            return false;
        }

        internal List<CodingHabit> SelectOne(int id)
        {
            return _database.SelectOne(id);
        }

        // benefits to having this pass-through each time?
        internal List<CodingHabit> SelectAll()
        {
            return _database.SelectAll();
        }
    }
}
