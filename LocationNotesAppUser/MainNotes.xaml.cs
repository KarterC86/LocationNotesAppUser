using CsvHelper;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace LocationNotesAppUser;

public partial class MainNotes : ContentPage
{
    ObservableCollection<Note> allNotes = [];

	public MainNotes()
	{
		InitializeComponent();

		notesList.ItemsSource = allNotes;
	}

    private void newNoteBtn_Clicked(object sender, EventArgs e)
    {
		allNotes.Add(new());

		// pull up the notepage where they can add things to the new note
		Navigation.PushAsync(new NotePage(allNotes.Last(), allNotes));
    }

    private void notesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((Note)notesList.SelectedItem != null)
        {
            Navigation.PushAsync(new NotePage((Note) notesList.SelectedItem, allNotes));
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