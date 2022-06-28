using AirPortCommon.DBModels;
using AirPortCommon.Models;
using AirPortDal.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AirPortCommon.Enums.Enums;

namespace AirPortServer.Hubs
{
    public class BlToSimHub : Hub
    {
        private readonly IDataService _dataService;
        public BlToSimHub(IDataService dataService)
        {
            _dataService = dataService;
        }

        public List<Airplane> SendAirplanes()
        {
            List<int> planeIds = new List<int>();
            List<PlannedArrivals> arrivals;
            List<PlannedDepartures> departures;
            List<Airplane> planes = new List<Airplane>();
            lock (_dataService)
            {
                arrivals = _dataService.GetPlannedArrivals();
                departures = _dataService.GetPlannedDepartures();

                foreach (var item in arrivals)
                {
                    var airplane = _dataService.GetSpecificAirplane(item.AirplaneId);
                    if (!airplane.IsDone)
                        planeIds.Add(item.AirplaneId);
                }

                foreach (var item in departures)
                {
                    var airplane = _dataService.GetSpecificAirplane(item.AirplaneId);
                    if (!airplane.IsDone)
                        planeIds.Add(item.AirplaneId);
                }

                foreach (var id in planeIds)
                {
                    planes.Add(_dataService.GetSpecificAirplane(id));
                }
            }
            return planes;
        }
        public List<Station> SendStations()
        {
            List<Station> stations;
            lock (_dataService)
            {
                stations = _dataService.GetStations();
            }
            return stations;
        }
        public List<Course> SendCourses()
        {
            List<Course> courses;
            lock (_dataService)
            {
                courses = _dataService.GetCourses();
            }
            return courses;
        }
        public List<HistoryInfo> SendHistory()
        {
            List<HistoryInfo> history;
            lock (_dataService)
            {
                history = _dataService.GetHistory();
            }
            return history;
        }
        public async void NewAirplane(Airplane airplane)
        {
            Airplane newAirplane;
            Course course;
            lock (_dataService)
            {
                newAirplane = _dataService.AddAirplane(airplane);
                course = _dataService.GetSpecificCourse(airplane.CourseId);
            }
            if (course.Type == FlightType.Arrival)
            {
                PlannedArrivals arrival = new PlannedArrivals(newAirplane.Id);
                lock (_dataService)
                {
                    _dataService.AddArrival(arrival);
                }
            }
            else
            {
                PlannedDepartures departure = new PlannedDepartures(newAirplane.Id);
                lock (_dataService)
                {
                    _dataService.AddDeparture(departure);
                }
            }
            await Clients.Others.SendAsync("CreatedAirplane", newAirplane);
        }
        public void UpdateAirplaneInDB(Airplane airplane)
        {
            lock (_dataService)
            {
                _dataService.UpdateAirplane(airplane);
            }
        }
        public void AddToHistoryInfo(HistoryInfo history)
        {
            lock (_dataService)
            {
                _dataService.AddHistoryInfo(history);
            }
        }
        public void UpdateStationStatus(Station station)
        {
            lock (_dataService)
            {
                _dataService.UpdateStation(station);
            }
        }
    }
}
