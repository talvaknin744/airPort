using AirPortCommon.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static AirPortCommon.Enums.Enums;

namespace AirPortSimulator
{
   
    public class AirPortSimulator
    {
        #region Fields
        private List<Course> Courses;

        const int COLOR_NUMBER = 5;
        const int COMPANY_NUMBER = 5;
        const int INTERVAL = 6000;

        private Random rnd = new Random();
        private Timer timer;
        private HubConnection connection;
        private string url = "http://localhost:60805/BlToSim";
        #endregion

        public async Task InitHubConnection()
        {
            connection = new HubConnectionBuilder().
                           WithUrl(url)
                          .Build();

            connection.StartAsync().Wait();

            Courses = await connection.InvokeAsync<List<Course>>("SendCourses");
        }

        #region Timer
        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await connection.InvokeAsync("NewAirplane", CreateAirplane());
        }
        public void InitTimer()
        {
            timer = new Timer(INTERVAL);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        #endregion

        private Airplane CreateAirplane()
        {
            int courseNumber = rnd.Next(0, Courses.Count);
            int colorIndex = rnd.Next(0, COLOR_NUMBER);
            int companyIndex = rnd.Next(0, COMPANY_NUMBER);
            Color color = (Color)Enum.Parse(typeof(Color), colorIndex.ToString());
            Company company = (Company)Enum.Parse(typeof(Company), companyIndex.ToString());
            var airplane = new Airplane(color, company, Courses[courseNumber].Id);
            return airplane;
        }
    }
}
