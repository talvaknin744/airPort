using AirPortCommon.DBModels;
using AirPortCommon.Models;
using AirPortDal.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirPortServer.Hubs
{
    public class BlToUiHub: Hub
    {
        private readonly IDataService _dataService;
        private List<Course> courses;
        private List<Airplane> allAirplanes;
        public BlToUiHub(IDataService dataService)
        {
            _dataService = dataService;
            courses = _dataService.GetCourses();
        }

        public async void SendStations(List<Station> stations)
        {
            await Clients.Others.SendAsync("sendStations", stations);
        }
        public async void SendAirplanes(List<Airplane> airplanes)
        {
            await Clients.Others.SendAsync("sendAirplanes", airplanes);
            lock (_dataService)
            {
                allAirplanes = _dataService.GetAirplanes();
            }
            await Clients.Others.SendAsync("sendAllAirplanes", allAirplanes);
        }
        public async void SendHistory(List<HistoryInfo> history)
        {
            await Clients.Others.SendAsync("sendHistory", history);
        }
        public async void SendCourses()
        {
            await Clients.Caller.SendAsync("sendCourses", courses);
        }
    }
}
