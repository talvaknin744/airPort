using AirPortCommon.Models;
using AirPortCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortBl
{
    public class TestLogic : ITestMethods
    {
        public Course FindCourse(Airplane airplane, List<Course> courses)
        {
            Course course;
            lock (courses)
            {
                course = courses.Find(t => t.Id == airplane.CourseId);
            }
            return course;
        }

        public Station FindStationInCourse(Airplane airplane, List<Course> courses)
        {
            Station station;
            lock (courses)
            {
                station = FindCourse(airplane, courses).Stations.Find(s => s.Id == airplane.ActiveStationId);
            }
            return station;
        }
    }
}
