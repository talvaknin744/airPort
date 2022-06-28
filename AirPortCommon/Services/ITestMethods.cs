using AirPortCommon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortCommon.Services
{
    public interface ITestMethods
    {
        Course FindCourse(Airplane airplane, List<Course> courses);
        Station FindStationInCourse(Airplane airplane, List<Course> courses);
    }
}
