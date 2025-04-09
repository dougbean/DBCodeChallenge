using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using DBLibrary.Model;
using DBLibrary.Services;
using DBLibrary.Wrappers;
using Moq;

namespace DBUnitTest
{
    [TestClass]
    public class ParserUnitTest
    {        
        private ParserService _parserService;
        private IStreamReader _streamReader;
        private IFileSystem _fileSystemWrapper;
        private List<IFileFormatGetter> _formatGetters;
        private Dictionary<FormatEnum, char> _delimiters; 

        [TestInitialize]
        public void Initialize()
        {
            _formatGetters = GetFormatGetters();
            _delimiters = GetDelimiters();
            _streamReader = new StreamReaderWrapper();
            _fileSystemWrapper = new FileSystemWrapper();
            _parserService = new ParserService(_streamReader, _fileSystemWrapper, _formatGetters, _delimiters);
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

        [TestMethod]
        public void ParserServiceShouldGetCommaFileFormatFromFileName()
        {
            //arrange
            string fileName = @"C:\gtr\gtr-comma.txt";

            //act          
            FormatEnum actual = _parserService.GetFormat(fileName); 

            //assert
            FormatEnum expected = FormatEnum.comma;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParserServiceShouldGetPipeFileFormatFromFileName()
        {
            //arrange
            string fileName = @"C:\gtr\gtr-pipe.txt";

            //act            
            FormatEnum actual = _parserService.GetFormat(fileName);

            //assert
            FormatEnum expected = FormatEnum.pipe;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParserServiceShouldGetSpaceFileFormatFromFileName()
        {
            //arrange
            string fileName = @"C:\gtr\gtr-space.txt";

            //act          
            FormatEnum actual = _parserService.GetFormat(fileName);

            //assert
            FormatEnum expected = FormatEnum.space;
            Assert.AreEqual(expected, actual);
        }       

        [TestMethod]
        public void ParserServiceShouldGetPersonFromCommaDelimitedLine()
        {
            //arrange
            //the "comma" string in the file name tells the parser to use a comma delimiter
            string fileName = @"C:\gtr\gtr-comma.txt"; 

            var mockStreamReader = new Mock<IStreamReader>();            
            mockStreamReader.Setup(s => s.ReadLine())
                   .Returns(new Queue<string>(new[] { "Gibbe,Candace,Female,Crimson,3/28/2010", null }).Dequeue);
            
            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>())).Verifiable();
                        
            var mockFileSystemWrapper = new Mock<IFileSystem>();
            mockFileSystemWrapper.Setup(s => s.FileExists(It.IsAny<String>())).Returns(true);            
            
            var parserService = new ParserService(mockStreamReader.Object, mockFileSystemWrapper.Object, _formatGetters, _delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);

            var person = persons.FirstOrDefault();

            //assert
            string expected = "Gibbe";           
            Assert.AreEqual(expected, person.LastName);
            mockStreamReader.VerifyAll();
        }

        [TestMethod]
        public void ParserServiceShouldCorrectlyMapPersonProperties()
        {
            //arrange
            //the "comma" string in the file name tells the parser to use a comma delimiter
            string fileName = @"C:\gtr\gtr-comma.txt";

            string lastName = "Whiteside";
            string firstName = "Zachary";
            string gender = "Male";
            string favoriteColor = "Teal";
            DateTime dateOfBirth = DateTime.Parse("5/25/1977");

            var builder = new StringBuilder();
            builder.Append(lastName).Append(",");
            builder.Append(firstName).Append(",");
            builder.Append(gender).Append(",");
            builder.Append(favoriteColor).Append(",");
            builder.Append(dateOfBirth);

            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                   .Returns(new Queue<string>(new[] { builder.ToString(), null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>())).Verifiable();
                        
            var mockFileSystemWrapper = new Mock<IFileSystem>();
            mockFileSystemWrapper.Setup(s => s.FileExists(It.IsAny<String>())).Returns(true);            
            
            var parserService = new ParserService(mockStreamReader.Object, mockFileSystemWrapper.Object, _formatGetters, _delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);

            var person = persons.FirstOrDefault();

            //assert            
            Assert.AreEqual(lastName, person.LastName);
            Assert.AreEqual(firstName, person.FirstName);
            Assert.AreEqual(gender, person.Gender);
            Assert.AreEqual(favoriteColor, person.FavoriteColor);
            Assert.AreEqual(dateOfBirth, person.DateOfBirth);
            mockStreamReader.VerifyAll();
        }

        [TestMethod]
        public void ParserServiceShouldGetPersonFromPipeDelimitedLine()
        {
            //arrange
            //the "pipe" string in the file name tells the parser to use a pipe delimiter
            string fileName = @"C:\gtr\gtr-pipe.txt";

            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                 .Returns(new Queue<string>(new[] { "Veregan|Jsandye|Female|Khaki|1/27/2007", null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>())).Verifiable();
            
            var mockFileSystemWrapper = new Mock<IFileSystem>();
            mockFileSystemWrapper.Setup(s => s.FileExists(It.IsAny<String>())).Returns(true); 
           
            var parserService = new ParserService(mockStreamReader.Object, mockFileSystemWrapper.Object, _formatGetters, _delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);

            var person = persons.FirstOrDefault();

            //assert
            string expected = "Veregan";
            Assert.AreEqual(expected, person.LastName);
        }
                
        [TestMethod]
        public void ParserServiceShouldMatchPersonListCountToLineNumberCount()
        {
            //arrange
            //the "pipe" string in the file name tells the parser to use a pipe delimiter
            string fileName = @"C:\gtr\gtr-pipe.txt";

            string line = "Veregan|Jsandye|Female|Khaki|1/27/2007";
            string line2 = "Ruperto|Billie|Female|Teal|7/24/1962";

            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                 .Returns(new Queue<string>(new[] { line, line2, null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>())).Verifiable();
                       
            var mockFileSystemWrapper = new Mock<IFileSystem>();
            mockFileSystemWrapper.Setup(s => s.FileExists(It.IsAny<String>())).Returns(true);  
           
            var parserService = new ParserService(mockStreamReader.Object, mockFileSystemWrapper.Object, _formatGetters, _delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);

            //assert
            int expected = 2;
            Assert.AreEqual(expected, persons.Count());
            mockStreamReader.VerifyAll();
        }

        [TestMethod]
        public void ParserServiceShouldGetPersonFromSpaceDelimitedLine()
        {
            //arrange
            //the "space" string in the file name tells the parser to use a space delimiter
            string fileName = @"C:\gtr\gtr-space.txt";

            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                 .Returns(new Queue<string>(new[] { "Rout Theodora Female Teal 2/3/1976", null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>())).Verifiable();
           
            var mockFileSystemWrapper = new Mock<IFileSystem>();
            mockFileSystemWrapper.Setup(s => s.FileExists(It.IsAny<String>())).Returns(true);   
           
            var parserService = new ParserService(mockStreamReader.Object, mockFileSystemWrapper.Object, _formatGetters, _delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);

            var person = persons.FirstOrDefault();

            //assert
            string expected = "Rout";
            Assert.AreEqual(expected, person.LastName);
            mockStreamReader.VerifyAll();
        }
       
        [ExpectedException(typeof(Exception))]
        [TestMethod]        
        public void FileFormatGettersShouldBePresentForParserService()
        {
            //arrange
            //the "space" string in the file name tells the parser to use a space delimiter
            string fileName = @"C:\gtr\gtr-space.txt";

            List<IFileFormatGetter> formatGetters = null;

            Dictionary<FormatEnum, char> delimiters = GetDelimiters();
            
            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                 .Returns(new Queue<string>(new[] { "Rout Theodora Female Teal 2/3/1976", null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>()));
            
            var parserService = new ParserService(mockStreamReader.Object, _fileSystemWrapper, formatGetters, delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);       
        }
       
        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void DelimitersShouldBePresentForParserService()
        {
            //arrange
            //the "space" string in the file name tells the parser to use a space delimiter
            string fileName = @"C:\gtr\gtr-space.txt";
           
            List<IFileFormatGetter> formatGetters = GetFormatGetters();

            Dictionary<FormatEnum, char> delimiters = null;
            
            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                 .Returns(new Queue<string>(new[] { "Rout Theodora Female Teal 2/3/1976", null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>()));
            
            var parserService = new ParserService(mockStreamReader.Object, _fileSystemWrapper, formatGetters, delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ParserServiceShouldThrowExceptionIfParseFails()
        {
            //arrange            
            string fileName = @"C:\gtr\gtr-deliberately-wrong-format-specified-pipe.txt"; 

            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                 .Returns(new Queue<string>(new[] { "Rout Theodora Female Teal 2/3/1976", null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>()));
            
            var parserService = new ParserService(mockStreamReader.Object, _fileSystemWrapper, _formatGetters, _delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);            
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ParserServiceShouldThrowExceptionIfValidFileFormatIsNotSpecified()
        {
            //arrange            
            string fileName = @"C:\gtr\gtr-no-valid-format-specified.txt"; 

            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                 .Returns(new Queue<string>(new[] { "Rout Theodora Female Teal 2/3/1976", null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>()));

            var parserService = new ParserService(mockStreamReader.Object, _fileSystemWrapper, _formatGetters, _delimiters);

            //act
            IList<Person> persons = parserService.GetPersons(fileName);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ParserServiceShouldThrowExceptionIfSpecifiedFileIsNotFound()
        {
            //arrange
            //the "space" string in the file name tells the parser to use a space delimiter
            string fileName = @"C:\gtr\gtr-space.txt";
            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false).Verifiable();

            var mockStreamReader = new Mock<IStreamReader>();
            mockStreamReader.Setup(s => s.ReadLine())
                 .Returns(new Queue<string>(new[] { "Rout Theodora Female Teal 2/3/1976", null }).Dequeue);

            mockStreamReader.Setup(s => s.CreateReader(It.IsAny<String>()));
           
            var parserService = new ParserService(mockStreamReader.Object, mockFileSystem.Object, _formatGetters, _delimiters);

            //act and assert
            IList<Person> persons = parserService.GetPersons(fileName);
            mockFileSystem.VerifyAll();
        }
    }
}
