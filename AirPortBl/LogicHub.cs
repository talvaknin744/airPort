using AirPortCommon.DBModels;
using AirPortCommon.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirPortBl
{
    class LogicHub
    {
        #region Fields
        private HubConnection simConnection;
        private HubConnection uiConnection;
        private List<Airplane> airplanes;
        private List<Station> stations;
        private List<Course> courses;
        private List<HistoryInfo> history;
        string blUrl = "http://localhost:60805/BlToSim";
        string uiUrl = "http://localhost:60805/BlToUI";
        #endregion

        #region UI Methods
        public void InitUiHubConnection()
        {
            uiConnection = new HubConnectionBuilder()
                           .WithUrl(uiUrl)
                           .Build();

            uiConnection.StartAsync().Wait();

            AirplanesToUi(airplanes);
            StationsToUi(stations);
            HistoryToUi(history);
        }
        public async void StationsToUi(List<Station> stations)
        {
            await uiConnection.InvokeAsync("SendStations", stations);
        }
        public async void AirplanesToUi(List<Airplane> airplanes)
        {
            await uiConnection.InvokeAsync("SendAirplanes", airplanes);
        }
        public async void HistoryToUi(List<HistoryInfo> history)
        {
            await uiConnection.InvokeAsync("SendHistory", history);
        }
        #endregion

        #region Simulator Methods
        public async Task InitSimHubConnection()
        {
            simConnection = new HubConnectionBuilder()
                           .WithUrl(blUrl)
                           .Build();
            simConnection.On("CreatedAirplane", (Airplane airplane) =>
            {
                lock (airplanes)
                {
                    airplanes.Add(airplane);
                }
            });

            simConnection.StartAsync().Wait();

            airplanes = await simConnection.InvokeAsync<List<Airplane>>("SendAirplanes");
            stations = await simConnection.InvokeAsync<List<Station>>("SendStations");
            courses = await simConnection.InvokeAsync<List<Course>>("SendCourses");
            history = await simConnection.InvokeAsync<List<HistoryInfo>>("SendHistory");
        }
        public async void UpdateAirplane(Airplane airplane)
        {
            await simConnection.InvokeAsync("UpdateAirplaneInDB", airplane);
        }
        public async void AddToHistory(HistoryInfo info)
        {
            history.Add(info);
            HistoryToUi(history);
            await simConnection.InvokeAsync("AddToHistoryInfo", info);
        }
        public async void UpdateRealTime(Station station)
        {
            await simConnection.InvokeAsync("UpdateStationStatus", station);
        }
        #endregion

        #region Logic Methods
        public List<Airplane> SendAirplanesToLogic()
        {
            return airplanes;
        }
        public List<Station> SendStationsToLogic()
        {
            return stations;
        }
        public List<Course> SendCoursesToLogic()
        {
            return courses;
        }
        #endregion
    }
}
