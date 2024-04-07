using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Devices.Sensors;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.StaticMaps.Request;
using GoogleApi.Entities.Maps.StaticMaps.Response;
using GoogleApi;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using Microsoft.Extensions.Options;
using Plugin.LocalNotification;

namespace LocationNotesAppUser;

public partial class NotePage : ContentPage
{
	Note currentNote { get; set; }
    ObservableCollection<Note> notes { get; set; }

    public NotePage(Note note, ObservableCollection<Note> notes)
	{
		InitializeComponent();

		currentNote = note;

		noteGrid.BindingContext = currentNote;

        this.notes = notes;
	}

    private void nameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
		currentNote.Name = nameEntry.Text; // sets the name of the currentNote to the name entry
    }

    private void backBtn_Clicked(object sender, EventArgs e)
    {
		Navigation.PopAsync(); // goes to the previous screen
    }

    private void descEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        currentNote.Desc = descEntry.Text;
    }

    private void deleteBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
        
        notes.Remove(currentNote);
    }

    private async void noteGrid_Loaded(object sender, EventArgs e)
    {
        // get current geolocation
        var loc = await Geolocation.GetLocationAsync();

        if (loc != null)
        {
            MapSpan span = new MapSpan(loc, 1, 1); // sets the maps location to the user, with a small area of sight

            Map noteMap = new Map(span);

            noteMap.IsVisible = true;

            noteMap.X

            currentNote.loc = loc;

            noteMap.VerticalOptions = LayoutOptions.Fill; // makes sure the map fills the area it has

            noteMap.IsShowingUser = true;

            noteGrid.Add(noteMap, 0, 4);
        }
    }

    private void testBtn_Clicked(object sender, EventArgs e)
    {
        // put csv code here

        var request = new NotificationRequest
        {
            NotificationId = 1000,
            Title = currentNote.name,
            Subtitle = currentNote.desc,
            Description = "Something",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(5),
                NotifyRepeatInterval = TimeSpan.FromDays(1)
            }
        };
        LocalNotificationCenter.Current.Show(request);
    }
}