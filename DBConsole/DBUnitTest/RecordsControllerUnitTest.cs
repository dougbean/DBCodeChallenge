using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using DBLibrary.Model;
using DBWebAPINet8.Model;
using DBWebAPINet8.Controllers;

namespace DBUnitTest
{
    [TestClass]
    public class RecordsControllerUnitTest
    {
        private RecordsController _controller;
        
        [TestInitialize]
        public void Initialize()
        {
            _controller = new RecordsController();
            _controller._parserServiceWrapper.PersonCache = GetUnsortedPersonList();
        }

        private static List<Person> GetUnsortedPersonList()
        {
            var person1 = new Person() { LastName = "Garrique", FirstName = "Charlie", Gender = "Male", FavoriteColor = "Yellow", DateOfBirth = DateTime.Parse("4/10/1960") };
            var person2 = new Person() { LastName = "Connichie", FirstName = "Myrlene", Gender = "Female", FavoriteColor = "Turquoise", DateOfBirth = DateTime.Parse("11/20/1980") };
            var person3 = new Person() { LastName = "Passe", FirstName = "Gisele", Gender = "Female", FavoriteColor = "Goldenrod", DateOfBirth = DateTime.Parse("12/27/1971") };
            var person4 = new Person() { LastName = "Bearns", FirstName = "Skye", Gender = "Male", FavoriteColor = "Violet", DateOfBirth = DateTime.Parse("3/12/1975") };
            var person5 = new Person() { LastName = "Carder", FirstName = "Sancho", Gender = "Male", FavoriteColor = "Goldenrod", DateOfBirth = DateTime.Parse("3/23/1981") };
            var person6 = new Person() { LastName = "Whiteside", FirstName = "Zachary", Gender = "Male", FavoriteColor = "Teal", DateOfBirth = DateTime.Parse("5/25/1977") };
            var person7 = new Person() { LastName = "Nortcliffe", FirstName = "Boyd", Gender = "Male", FavoriteColor = "Violet", DateOfBirth = DateTime.Parse("3/22/2006") };
            var person8 = new Person() { LastName = "Tatters", FirstName = "Althea", Gender = "Female", FavoriteColor = "Indigo", DateOfBirth = DateTime.Parse("5/16/2017") };
            var person9 = new Person() { LastName = "Gibbe", FirstName = "Candace", Gender = "Female", FavoriteColor = "Crimson", DateOfBirth = DateTime.Parse("3/28/2010") };
            var person10 = new Person() { LastName = "Beese", FirstName = "Bram", Gender = "Male", FavoriteColor = "Red", DateOfBirth = DateTime.Parse("2/10/1982") };

            var persons = new List<Person>() {person1,person2,person3,person4,person5,person6,
                                             person7,person8,person9,person10 };
            return persons;
        }

        [TestMethod]
        public void RecordsControllerGetShouldReturnSortedByGenderList()
        {
            //arrange           
            string sortby = "gender";
            IList<Person> unsortedList = _controller._parserServiceWrapper.PersonCache;

            //act  
            IList<Person> sortedList = _controller.Get(sortby);  
            
            var first = sortedList.FirstOrDefault();
            var last = sortedList.LastOrDefault();

            //assert
            Assert.AreEqual("Connichie", first.LastName);
            Assert.AreEqual("Whiteside", last.LastName);
        }

        [TestMethod]
        public void RecordsControllerGetShouldReturnSortedByBirthdateList()
        {
            //arrange            
            string sortby = "birthdate";
            IList<Person> unsortedList = _controller._parserServiceWrapper.PersonCache;

            //act 
            IList<Person> sortedList = _controller.Get(sortby);  
            
            var first = sortedList.FirstOrDefault();
            var last = sortedList.LastOrDefault();

            //assert
            Assert.AreEqual("Garrique", first.LastName);
            Assert.AreEqual("Tatters", last.LastName);
        }

        [TestMethod]
        public void RecordsControllerGetShouldReturnSortedByNameList()
        {
            //arrange            
            string sortby = "name";
            IList<Person> unsortedList = _controller._parserServiceWrapper.PersonCache;

            //act 
            IList<Person> sortedList = _controller.Get(sortby);    
            
            var first = sortedList.FirstOrDefault();
            var last = sortedList.LastOrDefault();

            //assert
            Assert.AreEqual("Whiteside", first.LastName);
            Assert.AreEqual("Bearns", last.LastName);
        }

        [TestMethod]
        public void RecordsControllerPostShouldStoreRecordInCache()
        {
            //arrange
            var controller = new RecordsController();
            var record = new Record() { Delimiter = "pipe", Line = "Braams|Karrah|Female|Goldenrod|10/8/1968" };           
            controller._parserServiceWrapper.PersonCache = new List<Person>();

            //act             
            _controller.Post(record);

            IList<Person> unsortedList = controller._parserServiceWrapper.PersonCache;
            var first = unsortedList.FirstOrDefault();

            //assert
            Assert.AreEqual("Braams", first.LastName);
            Assert.AreEqual("Karrah", first.FirstName);
            Assert.AreEqual("Female", first.Gender);
            Assert.AreEqual("Goldenrod", first.FavoriteColor);
            Assert.AreEqual("10/8/1968", first.DateOfBirth.ToString("d"));
        }
    }
}
