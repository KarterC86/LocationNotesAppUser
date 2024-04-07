using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Xml.Linq;
using Map = Microsoft.Maui.Controls.Maps.Map;
using CsvHelper.Configuration;
using Pin = Microsoft.Maui.Controls.Maps.Pin;
using Microsoft.Maui.Controls.Maps;

namespace LocationNotesAppUser;

public partial class MainNotes : ContentPage
{
    public ObservableCollection<Note> allNotes = [];

    public Map mainMap;

	public MainNotes()
	{
		InitializeComponent();
    }

    private void newNoteBtn_Clicked(object sender, EventArgs e)
    {
		allNotes.Add(new());

		// pull up the notepage where they can add things to the new note
		Navigation.PushAsync(new NotePage(allNotes.Last(), allNotes, mainMap, this));
    }

    protected override async void OnAppearing() // when it first loads
    {
        var loc = await Geolocation.GetLocationAsync();

        var span = new MapSpan(loc, loc.Latitude + 1, loc.Longitude + 1);

        mainMap = new Map(span);

        mainMap.IsVisible = true;

        mainMap.IsShowingUser = true;

        mainMap.VerticalOptions = LayoutOptions.Fill;

        appearFunc();

        base.OnAppearing();
    }

    public void appearFunc() 
    {
        // create the map and set it to a variable
        if (mainGrid.Children.Contains(mainMap))
        {
            mainGrid.Children.Remove(mainMap);
        }

        resetPins();

        mainGrid.AddWithSpan(mainMap, 1, 0, 1, 2);
    }

    public void resetPins()
    {
        ObservableCollection<Pin> pins = new();

        foreach (Note note in allNotes)
        {
            var pin = new Pin();

            // sets the pins label and location
            pin.Label = note.name;
            pin.Location = note.loc;

            // whenever the user clicks the pin, it takes them to its edit page
            pin.MarkerClicked += (object? sender, PinClickedEventArgs e) =>
            {
                Navigation.PushAsync(new NotePage(allNotes.Last(), allNotes, mainMap, this));
            };

            pins.Add(pin); // adds the pin to the map
        }

        mainMap.ItemsSource = pins;
    }
    private void getData()
    {
        using (var reader = new StreamReader(""))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Note>();
        }

    }

    private void SaveNotes()
    {
        using var writer = new StreamWriter("notes.csv");
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteHeader<notemap>();
        csv.NextRecord();
        foreach (var Note in allNotes)
        {
             csv.WriteRecord(Note);
             csv.NextRecord();
        }

        csv.Flush();
    }
}