using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortCommon.DBModels
{
    abstract public class PlannedCourse
    {
        public int Id { get; set; }
        public int AirplaneId { get; set; }

        public PlannedCourse()
        {

        }

        public PlannedCourse(int airplaneId)
        {
            AirplaneId = airplaneId;
        }
    }
}
