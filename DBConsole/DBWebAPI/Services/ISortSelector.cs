using System.Collections.Generic;
using DBLibrary.Model;

namespace DBWebAPI.Services
{
    public interface ISortSelector
    {
        public IList<Person> GetGersons(IList<Person> unsortedList, string sortBy);
    }
}
