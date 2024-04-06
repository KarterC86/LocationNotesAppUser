using System.Collections.ObjectModel;

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
}