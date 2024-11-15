using System;

namespace XMLanalyze.XML_Manager
{
    public class Filters
    {
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string Course { get; set; }
        public string Room { get; set; }
        public DateOnly? CheckInDate { get; set; } // Дата заселення
        public DateOnly? CheckOutDate { get; set; } // Дата виселення

        public Filters()
        {
            Name = string.Empty;
            Faculty = string.Empty;
            Course = string.Empty;
            Room = string.Empty;
            CheckInDate = null;
            CheckOutDate = null;
        }

        public bool ValidatePerson(Person person)
        {
            bool nameMatches = string.IsNullOrEmpty(Name) ||
                               ($"{person.Name.FirstName} {person.Name.LastName}".ToLower().Contains(Name.ToLower()));
            bool facultyMatches = string.IsNullOrEmpty(Faculty) ||
                                  person.Faculty.ToLower().Contains(Faculty.ToLower());
            bool courseMatches = string.IsNullOrEmpty(Course) ||
                                 person.Course.ToLower().Contains(Course.ToLower());
            bool roomMatches = string.IsNullOrEmpty(Room) ||
                               person.Room.ToLower().Contains(Room.ToLower());

            bool checkInMatches = !CheckInDate.HasValue || person.CheckInDate == CheckInDate;
            bool checkOutMatches = !CheckOutDate.HasValue || person.CheckOutDate == CheckOutDate;

            return nameMatches && facultyMatches && courseMatches && roomMatches && checkInMatches && checkOutMatches;
        }
    }
}
