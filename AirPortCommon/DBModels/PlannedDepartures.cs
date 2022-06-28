using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortCommon.DBModels
{
    public class PlannedDepartures: PlannedCourse
    {
        public PlannedDepartures()
        {

        }

        public PlannedDepartures(int airplaneId) : base(airplaneId)
        {

        }
    }
}
