using static AirPortCommon.Enums.Enums;

namespace AirPortCommon.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public Color Color { get; set; }
        public Company Company { get; set; }
        public int CourseId { get; set; }
        public int ActiveStationId { get; set; }
        public int Waited { get; set; }
        public bool IsDone { get; set; }

        public Airplane(Color color, Company company, int courseId)
        {
            Color = color;
            Company = company;
            CourseId = courseId;
        }

        public Airplane()
        {

        }
    }
}
