using CsvHelper;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace LocationNotesAppUser;

public partial class MainNotes : ContentPage
{
    ObservableCollection<Note> allNotes = [];

    Map mainMap { get; set; }

	public MainNotes()
	{
		InitializeComponent();

		notesList.ItemsSource = allNotes;

        getMap();
    }

    private async void getMap()
    {
        var loc = await Geolocation.GetLocationAsync();

        MapSpan span = new MapSpan(loc, loc.Latitude + 15, loc.Longitude + 15); // sets the maps location to the user, with a small area of sight

        mainMap = new Map(span);

        mainMap.IsVisible = true;

        mainMap.VerticalOptions = LayoutOptions.Fill; // makes sure the map fills the area it has

        mainMap.IsShowingUser = true;
    }

    private void newNoteBtn_Clicked(object sender, EventArgs e)
    {
		allNotes.Add(new());

        allNotes.Last().setLoc();

		// pull up the notepage where they can add things to the new note
		Navigation.PushAsync(new NotePage(allNotes.Last(), allNotes, mainMap));
    }

    private void notesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((Note)notesList.SelectedItem != null)
        {
            Navigation.PushAsync(new NotePage((Note) notesList.SelectedItem, allNotes, mainMap));
        }
    }

    private void getData()
    {
        // put csv code here

        using (var reader = new StreamReader(""))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Note>();
        }
    }
}