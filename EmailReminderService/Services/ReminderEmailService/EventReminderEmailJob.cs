using System;
using System.Collections.Generic;
using FluentScheduler;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CALENDAR.Entity;
using CALENDAR.BusinessLogic.EventManagement;

namespace EmailReminderService
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

        /// <summary>
        /// This method gets all events from db that have eventReminderDate value equal to current DateTime, 
        /// and sends reminder and notification emails accordingly
        /// </summary>
        public async void Execute()
        {
            DateTime current = DateTime.Now;
            List<Event> emailNotifications = eventManagment.GetEventsFromDate(current, "notification"); //list of all events that occur today and email notification needs to be sent
            List<Event> emailReminders = eventManagment.GetEventsFromDate(current, "reminder"); //list of all events that need email reminder to be sent

            //send email notification to user when the event occurs
            if (emailNotifications.Count != 0) 
            {
                SendEmailNotification(emailNotifications);
            }

            if (emailReminders.Count != 0) //send reminder emails to user and their other chosen reminder receivers
            {
                SendEmailReminderToUser(emailReminders);
            }
        }

        /// <summary>
        /// This method sends event email reminder to user
        /// </summary>
        /// <param name="emailReminders"></param>
        private async void SendEmailReminderToUser(List<Event> emailReminders) 
        {
            foreach (Event e in emailReminders)
            {
                //reminder email for user
                var user = await userManager.FindByIdAsync(e.UserId);
                string toEmail = user.Email;
                string subject = "Reminder Email";
                string message = String.Format("This is a reminder for your upcoming event {0} on {1},check your calendar for more details!", e.Name, e.StartTime);

                var response = sender.SendEmailAsync(toEmail, subject, message);
                _logger.LogInformation(response.IsCompletedSuccessfully ? "Email queued successfully!" : "Something went wrong!");

                //reminder emails for other chosen reminder receivers
                foreach (String email in e.Emails)
                {
                    SendEmailReminderToOtherUsers(email, e);
                }
                e.eventReminderDate = CalculateEventReminderDate(e); //updating next eventReminder date
            }
        }

        /// <summary>
        /// This method sends event reminder emails to other people the user has previously specified 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="e"></param>
        private async void SendEmailReminderToOtherUsers(String email, Event e)
        {
            string toEmail = email;
            string subject = "Reminder Email";
            string message = String.Format("This is a reminder for my upcoming event {0} on {1}!", e.Name, e.StartTime);

            var response = sender.SendEmailAsync(toEmail, subject, message);
            _logger.LogInformation(response.IsCompletedSuccessfully ? "Email queued successfully!" : "Something went wrong!");
        }

        /// <summary>
        /// This method sends email notification to user when event occurs
        /// </summary>
        /// <param name="emailNotifications"></param>
        private async void SendEmailNotification(List<Event> emailNotifications)
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

        
        /// <summary>
        /// This method calculates the next reminder date based on the event reminder frequency
        /// </summary>
        /// <param name="reminderFrequency"></param>
        /// <param name="NTimesFrequency"></param>
        /// <returns></returns>
        public static DateTime CalculateEventReminderDate(Event e)
        {
            DateTime reminderDate = DateTime.Now;
            ReminderFrequency reminderFrequency = e.reminderFrequency;
            int nTimesFrequency = e.NTimesFrequency;
            switch (reminderFrequency)
            {
                case ReminderFrequency.Daily:
                    reminderDate = reminderDate.AddDays(nTimesFrequency);
                    break;
                case ReminderFrequency.Weekly:
                    reminderDate = reminderDate.AddDays(nTimesFrequency * 7);
                    break;
                case ReminderFrequency.Monthly:
                    reminderDate = reminderDate.AddMonths(nTimesFrequency);
                    break;
                case ReminderFrequency.Yearly:
                    reminderDate = reminderDate.AddYears(nTimesFrequency);
                    break;
            }

            return reminderDate;
        }
    }
}
