using System.Collections.Generic;
using DBLibrary.Services;
using DBLibrary.Model;

namespace DBWebAPI.Services
{
    public class NameSort : ISortSelector
    {
        private ISortService _sortService;
        public NameSort(ISortService sortService)
        {
            _sortService = sortService;
        }

        public IList<Person> GetGersons(IList<Person> unsortedList, string sortBy)
        {
            IList<Person> persons = new List<Person>();
            if (sortBy.Contains(Model.Constants.Name))
            {
                persons = _sortService.SortByLastNameDescending(unsortedList);
            }
            return persons;
        }
    }
}
