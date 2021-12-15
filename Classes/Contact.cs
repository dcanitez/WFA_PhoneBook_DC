using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFA_PhoneBook_DC.Classes
{
    public class Contact
    {        
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public Category Category { get; set; }
        public bool IsFemale { get; set; }
        public bool IsFavorite { get; set; }

        public override string ToString()
        {
            return $"{this.Name} {this.Surname} - {this.PhoneNumber}";
        }
    }

    
}
