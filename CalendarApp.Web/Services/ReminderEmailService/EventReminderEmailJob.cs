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
    public class EventReminderEmailJob : IJob
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IEmailSender sender;

        private readonly ILogger _logger;

        private readonly EventManagement eventManagment;


        public EventReminderEmailJob() { }
        public EventReminderEmailJob(UserManager<ApplicationUser> userManager, IEmailSender sender, ILogger<IEmailSender> logger, EventManagement eventManagment)
        {
            this.userManager = userManager;
            this.sender = sender;
            this._logger = logger;
            this.eventManagment = eventManagment;
        }

        public async void Execute()//go zemam momentalniot datum i gi zemam od baza site eventi koi 
                                   //imaat datum za Reminder deneshen den ili se sluchuvaat denes i gi izminuvam i prakjam soodveten mail
        {
            DateTime current = DateTime.Now;
            List<Event> emailNotifications = eventManagment.GetEventsFromDate(current, "notification"); //list of all events that occur today and email notification needs to be sent
            List<Event> emailReminders = eventManagment.GetEventsFromDate(current, "reminder"); //list of all events that need email reminder to be sent

            if (emailNotifications.Count != 0) //sending emails to users when event occurs
            {
                foreach (Event e in emailNotifications)
                {
                    var user = await userManager.FindByIdAsync(e.UserId);
                    string toEmail = user.Email;
                    string subject = "Notification email";
                    string message = String.Format("This is a notification for your upcoming event {0} on {1},check your calendar for more details!", e.Name, e.StartTime);

                    var response = sender.SendEmailAsync(toEmail, subject, message);
                    _logger.LogInformation(response.IsCompletedSuccessfully ? "Email queued successfully!" : "Something went wrong!");
                }
            }

            if (emailReminders.Count != 0) //sending reminder emails
            {
                foreach (Event e in emailReminders)
                {
                    //reminder email za samiot korisnik
                    var user = await userManager.FindByIdAsync(e.UserId);
                    string toEmail = user.Email;
                    string subject = "Reminder Email";
                    string message = String.Format("This is a reminder for your upcoming event {0} on {1},check your calendar for more details!", e.Name, e.StartTime);

                    var response = sender.SendEmailAsync(toEmail, subject, message);
                    _logger.LogInformation(response.IsCompletedSuccessfully ? "Email queued successfully!" : "Something went wrong!");

                    //reminder emails za ostanatite lugje sto saka da im prati reminder
                    foreach (String email in e.Emails)
                    {
                        toEmail = email;
                        subject = "Reminder Email";
                        message = String.Format("This is a reminder for my upcoming event {0} on {1}!", e.Name, e.StartTime);

                        response = sender.SendEmailAsync(toEmail, subject, message);
                        _logger.LogInformation(response.IsCompletedSuccessfully ? "Email queued successfully!" : "Something went wrong!");

                    }
                    e.eventReminderDate = CalculateEventReminderDate(e); //tuka apdejtnuvam nov datum za sleden reminder
                }
            }
        }

        public static DateTime CalculateEventReminderDate(Event e)
        {
            DateTime reminderDate = DateTime.Now;
            if (e.reminderFrequency.Equals(ReminderFrequency.Daily)) return reminderDate.AddDays(e.NTimesFrequency * 1);
            else if (e.reminderFrequency.Equals(ReminderFrequency.Weekly)) return reminderDate.AddDays(e.NTimesFrequency * 7);
            else if (e.reminderFrequency.Equals(ReminderFrequency.Monthly)) return reminderDate.AddMonths(e.NTimesFrequency * 1);
            else return reminderDate.AddYears(e.NTimesFrequency * 1);
        }
    }
}
