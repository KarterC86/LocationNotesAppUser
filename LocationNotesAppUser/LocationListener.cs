using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Plugin.LocalNotification;

namespace LocationNotesAppUser
{
    internal class LocationListener : BackgroundService
    {
        public MainNotes mainPage { get; set; }

        public LocationListener(MainNotes main) 
        {
            this.mainPage = main;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested) // until it is asked to stop
            {
                Thread.Sleep(60000); // sleep for a minute

                var curLoc = await Geolocation.GetLastKnownLocationAsync();

                foreach (Note note in mainPage.allNotes)
                {
                    if (curLoc.CalculateDistance(note.loc, DistanceUnits.Miles) <= note.distance)
                    {
                        if (note.notified == false)
                        {
                            note.notified = true;

                            // send a notification
                            giveNotification(note);
                        }
                    }
                    else
                    {
                        note.notified = false;
                    }
                }
            }
        }

        private void giveNotification(Note note)
        {
            var request = new NotificationRequest
            {
                NotificationId = 4,
                Title = note.name,
                Subtitle = "GeoNotes Notification",
                Description = note.desc,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(3),
                    NotifyRepeatInterval = TimeSpan.FromDays(1),
                }
            };

            LocalNotificationCenter.Current.Show(request);
        }
    }
}
