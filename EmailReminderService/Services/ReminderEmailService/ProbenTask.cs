using System;
using System.Collections.Generic;
using FluentScheduler;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CALENDAR.Entity;
using CALENDAR.BusinessLogic;



namespace EmailReminderService
{
    public class ProbenTask : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Raboti !!!"+ DateTime.Now);
        }
    }
}
