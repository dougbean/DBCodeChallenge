using DBLibrary.Services;

namespace DBWebAPINet8.Services
{
    public class SortServiceWrapper
    {
        public ISortService SortService { get; set; }

        private SortServiceWrapper()
        {
            SortService = new SortService();
        }

        private static volatile SortServiceWrapper _instance = null;
        public static SortServiceWrapper GetInstance()
        {
            if (_instance == null)
            {
                lock (typeof(SortServiceWrapper))
                {
                    _instance = new SortServiceWrapper();
                }
            }
            return _instance;
        }
    }
}
