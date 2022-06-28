using System;
using System.Collections.Generic;
using System.Text;
using static AirPortCommon.Enums.Enums;

namespace AirPortCommon.Models
{
    public class Course
    {
        public int Id { get; set; }
        public virtual List<Station> Stations { get; set; }
        public FlightType Type{ get; set; }

        public Course(FlightType type, List<Station> stations)
        {
            Type = type;
            Stations = stations;
        }

        public Course()
        {

        }
    }
}
