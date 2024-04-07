using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LocationNotesAppUser
{
    public class Note : INotifyPropertyChanged
    {
        public String name { get; set; }

        public String desc { get; set; }

        public Location? loc { get; set; }

        public Note()
        {
            this.name = "New Note";
            this.desc = "Add description here!";

            setLoc();
        }

        public async Task<Location> setLoc() // this allows the note to set its default point at the users location
        {
            loc = await Geolocation.GetLocationAsync();

            return loc;
        }

        // things for the property changed things so that the collection view gets set as it changes
        public event PropertyChangedEventHandler? PropertyChanged;

        // somehow tells the collection view that something changed
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // use these variable to get the values
        public String Name { get { return this.name; } set {  this.name = value; NotifyPropertyChanged();  } }
        public String Desc { get { return this.desc; } set {  this.desc = value; NotifyPropertyChanged();  } }

    }
    
    public class notemap : ClassMap<Note> 
    {
        public notemap()
        {

            Map(n => n.name).Name("Name");
            Map(n => n.desc).Name("discription");
            Map(n => n.loc.Latitude).Name("lat");
            Map(n => n.loc.Longitude).Name("long");
            
        }
    }
}
