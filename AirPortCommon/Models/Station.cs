using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AirPortCommon.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public int WaitTime { get; set; }

        [JsonIgnore]
        public virtual List<Course> Courses { get; set; }

        public Station(string name, int time, List<Course> courses, bool isAvailable = true)
        {
            Name = name;
            WaitTime = time;
            IsAvailable = isAvailable;
            Courses = courses;
        }

        public Station()
        {

        }
    }
}