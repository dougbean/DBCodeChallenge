using System.Collections.Generic;
using DBLibrary.Services;
using DBLibrary.Model;

namespace DBWebAPI.Services
{
    public class GenderSort : ISortSelector
    {
        private ISortService _sortService;
        public GenderSort(ISortService sortService)
        {
            _sortService = sortService;
        }

        public IList<Person> GetGersons(IList<Person> unsortedList, string sortBy)
        {
            IList<Person> persons = new List<Person>();
            if (sortBy.Contains(Model.Constants.Gender))
            {
                persons = _sortService.SortByGenderAndLastNameAscending(unsortedList);
            }
            return persons;
        }
    }
}
