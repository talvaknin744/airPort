using AirPortBl;
using AirPortCommon.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace AirPortTest
{
    public class Tests
    {
        #region Fields
        private Logic logic;
        private Course course;
        private List<Course> courses;
        private List<Station> stations;
        private Airplane airplane;
        private Mock testLogic;
        #endregion
        [SetUp]
        public void Setup()
        {
            testLogic = new Mock();
            logic = new Logic(testLogic);
            stations = new List<Station>
            {
                   new Station{ Name = "a", IsAvailable = false, Id = 1 },
                   new Station{ Name = "b", IsAvailable = true , Id = 2 },
                   new Station{ Name = "c", IsAvailable = false, Id = 3 }
            };
            course = new Course()
            {
                Stations = stations
            };
            courses = new List<Course> { course };
            airplane = new Airplane() { CourseId = course.Id };
        }

        #region IsLastStation
        [Test]
        public void IsLastStationTest_StationIsLast_ReturnTrue()
        {
            airplane.ActiveStationId = course.Stations[2].Id;

            var result = logic.IsLastStation(airplane, courses);

            Assert.That(result, Is.True);
        }
        [Test]
        public void IsLastStationTest_StationIsNotLast_ReturnFalse()
        {
            airplane.ActiveStationId = course.Stations[0].Id;

            var result = logic.IsLastStation(airplane, courses);

            Assert.That(result, Is.False);
        }
        #endregion

        #region IsFirstStation
        [Test]
        public void IsFirstStationTest_ActiveStationIsNull_ReturnTrue()
        {
            airplane.ActiveStationId = 0;

            var result = logic.IsFirstStation(airplane);

            Assert.That(result, Is.True);
        }
        [Test]
        public void IsFirstStationTest_ActiveStationIsNotNull_ReturnFalse()
        {
            airplane.ActiveStationId = course.Stations[0].Id;

            var result = logic.IsFirstStation(airplane);

            Assert.That(result, Is.False);
        }
        #endregion

        #region IsNextStationAvailable
        [Test]
        public void IsNextStationAvailableTest_StationIsAvailable_ReturnTrue()
        {
            airplane.ActiveStationId = course.Stations[0].Id;

            var result = logic.IsNextStationAvailable(airplane, stations, courses);

            Assert.That(result, Is.True);
        }
        [Test]
        public void IsNextStationAvailableTest_StationIsNotAvailable_ReturnFalse()
        {
            airplane.ActiveStationId = course.Stations[1].Id;

            var result = logic.IsNextStationAvailable(airplane, stations, courses);

            Assert.That(result, Is.False);
        }
        #endregion

        #region IsFirstStationAvailable
        [Test]
        public void IsFirstStationAvailableTest_StationAvailable_ReturnTrue()
        {
            stations[0].IsAvailable = true;

            var result = logic.IsFirstStationAvailable(airplane, stations, courses);

            Assert.That(result, Is.True);
        }
        [Test]
        public void IsFirstStationAvailableTest_StationNotAvailable_ReturnFalse()
        {
            stations[0].IsAvailable = false;

            var result = logic.IsFirstStationAvailable(airplane, stations, courses);

            Assert.That(result, Is.False);
        }
        #endregion
    }
}