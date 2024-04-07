using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Devices.Sensors;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.StaticMaps.Request;
using GoogleApi.Entities.Maps.StaticMaps.Response;
using GoogleApi;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map; // we do these so the program doesnt get it from other places
using Pin = Microsoft.Maui.Controls.Maps.Pin;
using Microsoft.Extensions.Options;
using Plugin.LocalNotification;

namespace LocationNotesAppUser;

public partial class NotePage : ContentPage
{
	Note currentNote { get; set; }
    ObservableCollection<Note> notes { get; set; }

    Map noteMap { get; set; }

    public NotePage(Note note, ObservableCollection<Note> notes, Map noteMap)
	{
		InitializeComponent();

		currentNote = note;

		noteGrid.BindingContext = currentNote;

        this.notes = notes;

        this.noteMap = noteMap;
	}

    private void nameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
		currentNote.Name = nameEntry.Text; // sets the name of the currentNote to the name entry

        addPin(); 
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
        addPin();

        noteGrid.Add(noteMap, 0, 4);
    }

    private void NoteMap_MapClicked(object? sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
        currentNote.loc = e.Location;

        addPin();
    }

    private void addPin()
    {
        if (noteMap != null)
        {
            noteMap.Pins.Clear();

            var pin = new Pin();

            pin.Location = currentNote.loc;

            pin.Label = currentNote.name;

            noteMap.Pins.Add(pin);

        }
    }

    private void testBtn_Clicked(object sender, EventArgs e)
    {
        Trace.WriteLine("");
        var request = new NotificationRequest
        {
           
            Title = currentNote.name,
            Subtitle = "GeoNotes",
            Description = currentNote.desc,
            Schedule = new NotificationRequestSchedule

            {
                NotifyTime = DateTime.Now.AddSeconds(5),
                NotifyRepeatInterval = TimeSpan.FromMinutes(1),
                
            },
            
        };
        LocalNotificationCenter.Current.Show(request);
    }
}