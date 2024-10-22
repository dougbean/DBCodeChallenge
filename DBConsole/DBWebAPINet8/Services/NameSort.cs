using System.Collections.Generic;
using DBLibrary.Services;
using DBLibrary.Model;

namespace DBWebAPINet8.Services
{
    public class NameSort : SortSelector
    {
        private ISortService _sortService;
        public NameSort(ISortService sortService)
        {
            _sortService = sortService;
        }      

        public override IList<Person> GetGersons(IList<Person> unsortedList, string sortBy)
        {
            IList<Person> persons = new List<Person>();
            if (sortBy.Contains(DBWebAPINet8.Model.Constants.Name))
            {
                persons = _sortService.SortByLastNameDescending(unsortedList);
            }
            return persons;
        }
    }
}
