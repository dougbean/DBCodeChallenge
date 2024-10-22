using System.Collections.Generic;
using DBLibrary.Services;
using DBLibrary.Model;

namespace DBWebAPINet8.Services
{
    public class GenderSort : SortSelector
    {
        private ISortService _sortService;
        public GenderSort(ISortService sortService)
        {
            _sortService = sortService;
        }
      
        public override IList<Person> GetGersons(IList<Person> unsortedList, string sortBy)
        {
            IList<Person> persons = new List<Person>();
            if (sortBy.Contains(DBWebAPINet8.Model.Constants.Gender))
            {
                persons = _sortService.SortByGenderAndLastNameAscending(unsortedList);
            }
            return persons;
        }
    }
}
