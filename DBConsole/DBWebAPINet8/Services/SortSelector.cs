using System.Collections.Generic;
using DBLibrary.Model;

namespace DBWebAPINet8.Services
{
    public abstract class SortSelector
    {        
        public abstract IList<Person> GetGersons(IList<Person> unsortedList, string sortBy);
    }
}
