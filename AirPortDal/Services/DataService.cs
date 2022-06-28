using AirPortCommon.DBModels;
using AirPortCommon.Models;
using AirPortDal.Data;
using System.Collections.Generic;
using System.Linq;
using static AirPortCommon.Enums.Enums;

namespace AirPortDal.Services
{
    public class DataService : IDataService
    {
        AirPortContext _context;
        public DataService(AirPortContext context)
        {
            _context = context;
        }

        #region HistoryInfo
        public List<HistoryInfo> GetHistory()
        {
            return _context.Histroy.ToList();
        }
        public void AddHistoryInfo(HistoryInfo info)
        {
            _context.Histroy.Add(info);
            _context.SaveChanges();
        }
        #endregion
        #region PlannedArrivals
        public List<PlannedArrivals> GetPlannedArrivals()
        {
            return _context.PlannedArrivals.ToList();
        }
        public void AddArrival(PlannedArrivals arrival)
        {
            var arrivalToAdd = new PlannedArrivals(arrival.AirplaneId);
            _context.PlannedArrivals.Add(arrivalToAdd);
            lock (_context)
            {
                _context.SaveChanges();
            }
        }
        #endregion
        #region PlannedDepartures
        public List<PlannedDepartures> GetPlannedDepartures()
        {
            return _context.PlannedDepartures.ToList();
        }
        public void AddDeparture(PlannedDepartures departure)
        {
            var departureToAdd = new PlannedDepartures(departure.AirplaneId);
            _context.PlannedDepartures.Add(departureToAdd);
            lock (_context)
            {
                _context.SaveChanges();
            }
        }
        #endregion
        #region Airplane
        public Airplane GetSpecificAirplane(int airplaneId)
        {
            return _context.Airplanes.FirstOrDefault(a => a.Id == airplaneId);
        }
        public Airplane AddAirplane(Airplane airplane)
        {
            var Course = _context.Courses.FirstOrDefault(t => t.Id == airplane.CourseId);
            var airplaneToAdd = new Airplane(airplane.Color, airplane.Company, Course.Id);
            lock (_context)
            {
                _context.Airplanes.Add(airplaneToAdd);
                _context.SaveChanges();
            }
            return _context.Airplanes.OrderBy(a => a.Id).Last();
        }
        public List<Airplane> GetAirplanes()
        {
            return _context.Airplanes.ToList();
        }
        public void UpdateAirplane(Airplane airplane)
        {
            var tempPlane = _context.Airplanes.FirstOrDefault(a => a.Id == airplane.Id);
            tempPlane.IsDone = airplane.IsDone;
            tempPlane.ActiveStationId = airplane.ActiveStationId;
            tempPlane.Waited = airplane.Waited;
            lock (_context)
            {
                _context.SaveChanges();
            }
        }
        #endregion
        #region Station
        public void UpdateStation(Station station)
        {
            var refStation = _context.Stations.FirstOrDefault(s => s.Id == station.Id);
            refStation.IsAvailable = station.IsAvailable;
            lock (_context)
            {
                _context.SaveChanges();
            }
        }
        public List<Station> GetStations()
        {
            return _context.Stations.ToList();
        }
        #endregion
        #region Course
        public Course GetSpecificCourse(int courseId)
        {
            return _context.Courses.FirstOrDefault(c => c.Id == courseId);
        }
        public List<Course> GetCourses()
        {
            lock (_context)
            {
                if (!(_context.Courses.Count() > 0))
                    CreateCourses();
            }
            return _context.Courses.ToList();
        }
        private void CreateCourses()
        {
            var Courses = new List<Course>(){
                new Course(FlightType.Arrival, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 3),
                    _context.Stations.FirstOrDefault(s => s.Id == 5),
                    _context.Stations.FirstOrDefault(s => s.Id == 1)
                }),
                new Course(FlightType.Arrival, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 7),
                    _context.Stations.FirstOrDefault(s => s.Id == 2),
                    _context.Stations.FirstOrDefault(s => s.Id == 4)
                }),
                new Course(FlightType.Arrival, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 1),
                    _context.Stations.FirstOrDefault(s => s.Id == 6),
                    _context.Stations.FirstOrDefault(s => s.Id == 3)
                }),
                new Course(FlightType.Arrival, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 4),
                    _context.Stations.FirstOrDefault(s => s.Id == 7),
                    _context.Stations.FirstOrDefault(s => s.Id == 2)
                }),
                new Course(FlightType.Arrival, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 2),
                    _context.Stations.FirstOrDefault(s => s.Id == 4),
                    _context.Stations.FirstOrDefault(s => s.Id == 1)
                }),
                new Course(FlightType.Departure, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 2),
                    _context.Stations.FirstOrDefault(s => s.Id == 1),
                    _context.Stations.FirstOrDefault(s => s.Id == 5)
                }),
                new Course(FlightType.Departure, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 3),
                    _context.Stations.FirstOrDefault(s => s.Id == 6),
                    _context.Stations.FirstOrDefault(s => s.Id == 7)
                }),
                new Course(FlightType.Departure, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 2),
                    _context.Stations.FirstOrDefault(s => s.Id == 4),
                    _context.Stations.FirstOrDefault(s => s.Id == 6)
                }),
                new Course(FlightType.Departure, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 7),
                    _context.Stations.FirstOrDefault(s => s.Id == 6),
                    _context.Stations.FirstOrDefault(s => s.Id == 5)
                }),
                new Course(FlightType.Departure, new List<Station>()
                {
                    _context.Stations.FirstOrDefault(s => s.Id == 1),
                    _context.Stations.FirstOrDefault(s => s.Id == 2),
                    _context.Stations.FirstOrDefault(s => s.Id == 3)
                })};
            foreach (var item in Courses)
            {
                _context.Courses.Add(item);
            }
            _context.SaveChanges();

        }
        #endregion
    }
}
