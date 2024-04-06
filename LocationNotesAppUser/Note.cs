using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationNotesAppUser
{
    public class Note
    {
        String name { get; set; }

        public Note(String name)
        {
            this.name = name;
        }
    }
}
