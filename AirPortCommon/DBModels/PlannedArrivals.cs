using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortCommon.DBModels
{
    public class PlannedArrivals : PlannedCourse
    {

        public PlannedArrivals()
        {

        }

        public PlannedArrivals(int airplaneId) : base(airplaneId)
        {

        }
    }
}
