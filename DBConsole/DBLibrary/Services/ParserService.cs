using System;
using System.Collections.Generic;
using System.Linq;
using DBLibrary.Model;
using DBLibrary.Wrappers;

namespace DBLibrary.Services
{    
    public class ParserService : IParser
    {
        private readonly List<IFileFormatGetter> _formatGetters;
        private readonly Dictionary<FormatEnum, char> _delimiters;
        private readonly IStreamReader _streamReaderWrapper;
        private readonly IFileSystem _fileSystemWrapper;
       
        public ParserService(IStreamReader streamReaderWrapper, IFileSystem fileSystemWrapper, 
            List<IFileFormatGetter> formatGetters, Dictionary<FormatEnum, char> delimiters)
        {
            _streamReaderWrapper = streamReaderWrapper;
            _fileSystemWrapper = fileSystemWrapper;
            _formatGetters = formatGetters;
            _delimiters = delimiters;
        }
      
        public FormatEnum GetFormat(string input)
        {            
            CheckFormatters();

            var result = new FormatEnum();
            foreach (var getter in _formatGetters)
            {
                result = getter.GetFileFormat(input);
                if (result != FormatEnum.none)
                {
                    break;
                }
            }
            return result;
        }

        private void CheckFormatters()
        {
            if (AreFormatGettersMissing())
            {
                throw new Exception("FileFormatGetters are missing.");
            }
        }

        private bool AreFormatGettersMissing()
        {
            return (_formatGetters == null || _formatGetters.Count == 0);            
        }      

        public IList<Person> GetPersons(string fileName)
        {
            List<Person> persons = new List<Person>();
            try
            {
                FormatEnum format = GetFormat(fileName);                 
                CheckFormat(format);               
                KeyValuePair<FormatEnum, char> delimiter = GetDilimiter(format);
                persons = GetPersons(fileName, delimiter.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }            
            return persons;           
        }

        private void CheckFormat(FormatEnum format)
        {
            if (IsSpecifiedFormatNone(format))
            {
                throw new Exception("Valid file format is not specified in file name.");
            }
        }

        private bool IsSpecifiedFormatNone(FormatEnum format)
        {
            return (FormatEnum.none == format);
        }

        private KeyValuePair<FormatEnum, char> GetDilimiter(FormatEnum fileFormat)
        {          
            CheckDelimiters();

            return _delimiters.Where(x => x.Key == fileFormat).FirstOrDefault();                
        }

        private void CheckDelimiters()
        {
            if (AreDelimitersMissing())
            {
                throw new Exception("Delimiters are missing.");
            }
        }

        private bool AreDelimitersMissing()
        {
            return (_delimiters == null || _delimiters.Count == 0);
        }      

        private List<Person> GetPersons(string path, char delimiter)
        {
            var persons = new List<Person>();

            try
            {
                CheckForFile(path);
                _streamReaderWrapper.CreateReader(path);
                ReadLines(delimiter, persons);
            }
            finally
            {
                _streamReaderWrapper.DisposeReader();
            }            

            return persons;
        }

        private void CheckForFile(string path)
        {
            if (IsSpecifiedFileNotFound(path))
            {
                throw new Exception("Specified file is not found.");
            }
        }

        private bool IsSpecifiedFileNotFound(string path)
        {
            return (!_fileSystemWrapper.FileExists(path));
        }

        private void ReadLines(char delimiter, List<Person> persons)
        {
            string line;
            while ((line = _streamReaderWrapper.ReadLine()) != null)
            {
                string[] parsedRecord = line.Split(delimiter);                
                CheckArraySize(parsedRecord);
                Person person = GetPerson(parsedRecord);
                persons.Add(person);
            }
        }

        private void CheckArraySize(string[] parsedRecord)
        {
            if (IsParsedRecordArrayWrongSize(parsedRecord))
            {
                throw new Exception("Parsing of record failed. Parsed record array does not have five elements.");
            }
        }

        private bool IsParsedRecordArrayWrongSize(string[] parsedRecord)
        {
            return (parsedRecord.Length != 5);
        }

        private static Person GetPerson(string[] parsedRecord)
        {
            var person = new Person()
            {
                LastName = parsedRecord[0],
                FirstName = parsedRecord[1],
                Gender = parsedRecord[2],
                FavoriteColor = parsedRecord[3],
            };                
            SetDateOfBirth(parsedRecord, person);

            return person;
        }

        private static void SetDateOfBirth(string[] parsedRecord, Person person)
        {
            DateTime result;
            bool success = DateTime.TryParse(parsedRecord[4], out result);
            if (success)
            {
                person.DateOfBirth = result;
            }
        }

        public Person GetPerson(FormatEnum format, string record)
        {
            Person person;
            try
            {
                KeyValuePair<FormatEnum, char> delimiter = GetDilimiter(format);

                string[] parsedRecord = record.Split(delimiter.Value);
                CheckArraySize(parsedRecord);
                person = GetPerson(parsedRecord);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return person;
        }
    }
}
