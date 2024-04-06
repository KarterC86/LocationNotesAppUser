using System.Collections.ObjectModel;
using System.Diagnostics;

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
}