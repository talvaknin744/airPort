using AirPortCommon.Models;
using AirPortCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortTest
{
    public class Mock : ITestMethods
    {
        public Course FindCourse(Airplane airplane, List<Course> courses)
        {
            return courses[0];
        }

        public Station FindStationInCourse(Airplane airplane, List<Course> courses)
        {
            return FindCourse(airplane, courses).Stations.Find(s => s.Id == airplane.ActiveStationId);
        }
    }
}
