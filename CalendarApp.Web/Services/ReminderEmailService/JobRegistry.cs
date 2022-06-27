using System;
using System.Collections.Generic;
using FluentScheduler;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CALENDAR.Entity;
using CALENDAR.BusinessLogic;

namespace CalendarApp.Web.Services.ReminderEmailService
{
    public class JobRegistry : Registry
    {
        public JobRegistry()
        {
            //IJob job = new ProbenTask();
           // JobManager.AddJob(job, s => s.ToRunEvery(5).Seconds());

            IJob job = new EventReminderEmailJob();
            JobManager.AddJob(job, s => s.ToRunEvery(1).Days());
        }
    }
}
