using AirPortCommon.DBModels;
using AirPortCommon.Models;
using AirPortCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace AirPortBl
{
   public class Logic
    {
        #region Fields
        LogicHub connection;
        List<Airplane> airplanes;
        List<Airplane> refplanes;
        List<Station> stations;
        List<Course> courses;
        Timer timer;
        ITestMethods tm;
        const int INTERVAL = 3000;
        #endregion

        #region Constructor
        public Logic(ITestMethods testMethods)
        {
            connection = new LogicHub();
            tm = testMethods;
        }
        #endregion

        #region Initialization
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (refplanes)
            {
                foreach (var airplane in refplanes)
                {
                    if (!airplane.IsDone)
                        Task.Run(() => MoveToNextStation(airplane));
                    else
                    {
                        lock (airplanes)
                        {
                            airplanes.Remove(airplane);
                        }
                    }
                }
                InitRefList();
            }
        }
        private void InitRefList()
        {
            refplanes = new List<Airplane>();
            lock (airplanes)
            {
                foreach (var item in airplanes)
                {
                    refplanes.Add(item);
                }
            }
        }
        public void InitTimer()
        {
            timer = new Timer(INTERVAL);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        public void CreateConnection()
        {
            connection.InitSimHubConnection().GetAwaiter().GetResult();
            connection.InitUiHubConnection();
            stations = connection.SendStationsToLogic();
            courses = connection.SendCoursesToLogic();
            airplanes = connection.SendAirplanesToLogic();
            InitRefList();
        }
        #endregion

        #region Movement
        public void MoveToNextStation(Airplane airplane)
        {
            if (!IsFirstStation(airplane))
            {
                WaitingTime(airplane);
                if (IsLastStation(airplane, courses))
                {
                    FinishedTrip(airplane);
                }

                else if (IsNextStationAvailable(airplane, stations, courses))
                {
                    MoveToNextStationIfAvailable(airplane);
                }
            }
            else if (IsFirstStationAvailable(airplane, stations, courses))
            {
                MoveToFirstStation(airplane);
            }
        }
        #endregion

        #region Testable
        public bool IsNextStationAvailable(Airplane airplane, List<Station> stations, List<Course> courses)
        {
            bool result;
            Course airplaneTrip = tm.FindCourse(airplane, courses);
            Station airplaneStation = tm.FindStationInCourse(airplane, courses);

            int nextStationIndex = airplaneTrip.Stations.IndexOf(airplaneStation) + 1;
            int nextStationId = airplaneTrip.Stations[nextStationIndex].Id;
            lock (stations)
            {
                result = stations[nextStationId - 1].IsAvailable;
            }
            return result;
        }

        public bool IsFirstStationAvailable(Airplane airplane, List<Station> stations, List<Course> courses)
        {
            Course airplaneCourse = tm.FindCourse(airplane, courses);
            int stationIndex = airplaneCourse.Stations[0].Id;
            bool result;
            lock (stations)
            {
                result = stations[stationIndex - 1].IsAvailable;
            }
            return result;
        }
        public bool IsFirstStation(Airplane airplane)
        {
            return airplane.ActiveStationId == 0;
        }
        public bool IsLastStation(Airplane airplane, List<Course> courses)
        {
            Course airplaneCourse = tm.FindCourse(airplane, courses);
            var lastStationInCourse = airplaneCourse.Stations[airplaneCourse.Stations.Count - 1];
            return airplane.ActiveStationId == lastStationInCourse.Id;
        }
        #endregion

        #region Sub-Testable
        private void CreateHistoryInfo(Airplane airplane, string title)
        {
            var info = new HistoryInfo(airplane.Id, title, DateTime.Now, airplane.ActiveStationId);
            connection.AddToHistory(info);
        }
        private void UpdateStationStatus(Airplane airplane, bool isAvailable)
        {
            Station station;
            int stationId = airplane.ActiveStationId;
            lock (stations)
            {
                station = stations[stationId - 1];
                station.IsAvailable = isAvailable;
            }
            connection.UpdateRealTime(station);
            connection.StationsToUi(stations);
        }
        private void FinishedTrip(Airplane airplane)
        {
            airplane.IsDone = true;

            CreateHistoryInfo(airplane, "Left");
            UpdateStationStatus(airplane, true);

            airplane.ActiveStationId = 0;
            connection.UpdateAirplane(airplane);
            CreateHistoryInfo(airplane, "Finished");
        }
        private void MoveToFirstStation(Airplane airplane)
        {
            Course airplaneTrip = tm.FindCourse(airplane, courses);
            airplane.ActiveStationId = airplaneTrip.Stations[0].Id;
            connection.UpdateAirplane(airplane);
            connection.AirplanesToUi(airplanes);

            CreateHistoryInfo(airplane, "Arrived");
            UpdateStationStatus(airplane, false);
        }
        private void MoveToNextStationIfAvailable(Airplane airplane)
        {
            CreateHistoryInfo(airplane, "Left");
            UpdateStationStatus(airplane, true);

            Course airplaneTrip = tm.FindCourse(airplane, courses);
            Station airplaneStation = tm.FindStationInCourse(airplane, courses);
            var activeStationIndex = airplaneTrip.Stations.IndexOf(airplaneStation);
            airplane.ActiveStationId = airplaneTrip.Stations[activeStationIndex + 1].Id;
            airplane.Waited = 0;
            connection.UpdateAirplane(airplane);
            connection.AirplanesToUi(airplanes);

            CreateHistoryInfo(airplane, "Arrived");
            UpdateStationStatus(airplane, false);
        }
        private void WaitingTime(Airplane airplane)
        {
            if (airplane.Waited != FindStationFromStations(airplane))
            {
                Thread.Sleep(1000);
                airplane.Waited++;
            }
        }
        private int FindStationFromStations(Airplane airplane)
        {
            Station station;
            lock (stations)
            {
                station = stations.Find(s => s.Id == airplane.ActiveStationId);
            }
            return station.WaitTime;
        }
        #endregion
    }
}
