using System;
using System.Collections.Generic;
using DBLibrary.Model;
using DBLibrary.Services;
using DBLibrary.Wrappers;

namespace DBConsole
{
    class Program
    {
        private static IParser _parserService;
        private static ISortService _sortService; 

        static void Main(string[] args)
        {
            try
            {
                InitializeServices();

                while (true)
                {
                    Parse(_parserService, _sortService);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("press any key to exit..");
                Console.ReadKey();
            }           
        }

        private static void InitializeServices()
        {
            List<IFileFormatGetter> formatGetters = GetFormatGetters();
            Dictionary<FormatEnum, char> delimiters = GetDelimiters();
            IStreamReader streamReader = new StreamReaderWrapper();
            IFileSystem fileSystemWrapper = new FileSystemWrapper();
            _parserService = new ParserService(streamReader, fileSystemWrapper, formatGetters, delimiters);            
            _sortService = new SortService();
        }

        private static List<IFileFormatGetter> GetFormatGetters()
        {
            return new List<IFileFormatGetter>()
                  { new CommaFormatGetter(), new PipeFormatGetter(), new SpaceFormatGetter() };
        }

        private static Dictionary<FormatEnum, char> GetDelimiters()
        {
            Dictionary<FormatEnum, char> delimiters = new Dictionary<FormatEnum, char>();
            delimiters.Add(FormatEnum.comma, ',');
            delimiters.Add(FormatEnum.pipe, '|');
            delimiters.Add(FormatEnum.space, ' ');
            return delimiters;
        }

        private static void Parse(IParser _parserService, ISortService sortService)
        {
            Console.WriteLine("Enter file and path with file format specified as part of the name, " +
                           "such as 'comma', 'pipe' or 'space':");
            string input = Console.ReadLine();

            IList<Person> unsortedList = _parserService.GetPersons(input);
            Sort(sortService, unsortedList);
        }

        private static void Sort(ISortService sortService, IList<Person> unsortedList)
        {
            SortByGenderAndLastNameAscending(sortService, unsortedList);
            SortByBirthDateAscending(sortService, unsortedList);
            SortByLastNameDescending(sortService, unsortedList);
        }

        private static void SortByLastNameDescending(ISortService sortService, IList<Person> unsortedList)
        {
            IList<Person> sortedBylastName = sortService.SortByLastNameDescending(unsortedList);
            Console.WriteLine(" ");
            Console.WriteLine("Sorted by last name descending.");
            PrintList(sortedBylastName);
        }

        private static void SortByBirthDateAscending(ISortService sortService, IList<Person> unsortedList)
        {
            IList<Person> sortedByBirthdate = sortService.SortByBirthDateAscending(unsortedList);
            Console.WriteLine(" ");
            Console.WriteLine("Sorted by date of birth ascending.");
            PrintList(sortedByBirthdate);
        }

        private static void SortByGenderAndLastNameAscending(ISortService sortService, IList<Person> unsortedList)
        {
            IList<Person> sortedByGender = sortService.SortByGenderAndLastNameAscending(unsortedList);
            Console.WriteLine(" ");
            Console.WriteLine("Sorted by gender and by last name ascending.");
            PrintList(sortedByGender);
        }       

        private static void PrintList(IList<Person> persons)
        {
            foreach (var item in persons)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}",
                    item.LastName,
                    item.FirstName,
                    item.Gender,
                    item.FavoriteColor,
                    item.DateOfBirth.ToString("d"));
            }
        }
    }
}
