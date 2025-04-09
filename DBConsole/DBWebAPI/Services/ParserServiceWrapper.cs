using System.Collections.Generic;
using DBLibrary.Model;
using DBLibrary.Services;
using DBLibrary.Wrappers;

namespace DBWebAPI.Services
{
    public class ParserServiceWrapper
    {
        public IParser ParserService { get; set; }
        public IList<Person> PersonCache { get; set; }

        private ParserServiceWrapper()
        {
            InitializeParserService();
            PersonCache = new List<Person>();
        }

        private static volatile ParserServiceWrapper _instance = null;
        public static ParserServiceWrapper GetInstance()
        {
            if (_instance == null)
            {
                lock (typeof(ParserServiceWrapper))
                {
                    _instance = new ParserServiceWrapper(); 
                }
            }
            return _instance;
        }

        private void InitializeParserService()
        {
            List<IFileFormatGetter> formatGetters = GetFormatGetters();
            Dictionary<FormatEnum, char> delimiters = GetDelimiters();
            IStreamReader streamReader = new StreamReaderWrapper();
            IFileSystem fileSystemWrapper = new FileSystemWrapper();
            ParserService = new ParserService(streamReader, fileSystemWrapper, formatGetters, delimiters);
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
    }
}

