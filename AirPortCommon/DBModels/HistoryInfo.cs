using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortCommon.DBModels
{
   public class HistoryInfo
    {
        public int Id { get; set; }
        public int  AirplaneId { get; set; }
        public int StationId { get; set; }
        public string Title { get; set; }
        public DateTime ArrivedOrLeft { get; set; }

        public HistoryInfo(int airplaneId, string title, DateTime arrivedOrLeft, int stationId)
        {
            AirplaneId = airplaneId;
            StationId = stationId;
            Title = title;
            ArrivedOrLeft = arrivedOrLeft;
        }

        public HistoryInfo()
        {

        }
    }
}
