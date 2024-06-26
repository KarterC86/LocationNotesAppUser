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

    MainNotes mainP;

    public NotePage(Note note, ObservableCollection<Note> allNotes, Map mainMap, MainNotes main)
	{
        InitializeComponent();

        this.mainP = main;

        currentNote = note;

        noteGrid.BindingContext = currentNote;

        this.notes = allNotes;

        // Add null check before assigning
        if (mainMap != null)
        {
            this.noteMap = mainMap;
            // Ensure event subscription here if necessary
            noteMap.MapClicked += NoteMap_MapClicked;
        }
        else
        {
            // Handle the case where mainMap is null
            // Log an error, throw an exception, or handle it gracefully as per your requirement.
            // For now, we just log an error.
            Debug.WriteLine("mainMap is null in NotePage constructor.");
        }
    }

    private void nameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
		currentNote.Name = nameEntry.Text; // sets the name of the currentNote to the name entry

        addPin(noteMap); 
    }

    private void backBtn_Clicked(object sender, EventArgs e)
    {
        noteMap.MapClicked -= NoteMap_MapClicked;

		Navigation.PopAsync(); // goes to the previous screen

        mainP.appearFunc();
    }

    private void descEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        currentNote.Desc = descEntry.Text;
    }

    private void deleteBtn_Clicked(object sender, EventArgs e)
    {

        noteMap.MapClicked -= NoteMap_MapClicked;
        
        notes.Remove(currentNote);

        Navigation.PopAsync();

        mainP.appearFunc();
    }

    private void noteGrid_Loaded(object sender, EventArgs e)
    {
        noteMap.MapClicked += NoteMap_MapClicked;
        
        addPin(noteMap);

        noteGrid.Add(noteMap, 0, 4);
    }

    private void NoteMap_MapClicked(object? sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
        currentNote.loc = e.Location;

        addPin(noteMap);
    }

    private async void addPin(Map map)
    {
        if (noteMap != null && map != null)
        {
            map.Pins.Clear();

            var pin = new Pin();

            if (currentNote.loc == null)
            {
                await currentNote.setLoc();
            }

            pin.Location = currentNote.loc;

            pin.Label = currentNote.name;

            map.Pins.Add(pin);
        }
        else
        {
            Debug.WriteLine("Map was null");
        }
    }
}