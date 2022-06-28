using AirPortCommon.DBModels;
using AirPortCommon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortDal.Services
{
    public interface IDataService
    {
        #region HistoryInfo
        List<HistoryInfo> GetHistory();
        void AddHistoryInfo(HistoryInfo infoToAdd);
        #endregion
        #region PlannedArrivals
        List<PlannedArrivals> GetPlannedArrivals();
        void AddArrival(PlannedArrivals arrival);
        #endregion
        #region PlannedDepartures
        List<PlannedDepartures> GetPlannedDepartures();
        void AddDeparture(PlannedDepartures departure);
        #endregion
        #region Airplane
        Airplane GetSpecificAirplane(int airplaneId);
        Airplane AddAirplane(Airplane airplane);
        List<Airplane> GetAirplanes();
        void UpdateAirplane(Airplane airplane);
        #endregion
        #region Station
        List<Station> GetStations();
        void UpdateStation(Station station);
        #endregion
        #region Course
        Course GetSpecificCourse(int courseId);
        List<Course> GetCourses();
        #endregion
    }
}
