using System;
using System.Collections.Generic;

namespace XMLanalyze.XML_Manager
{
    public class Person
    {
        public string FullName { get; set; }
        public string Room { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public Dictionary<string, string> Attributes { get; set; }

        public string Faculty => Attributes.ContainsKey("Faculty") ? Attributes["Faculty"] : "N/A";
        public string Course => Attributes.ContainsKey("Course") ? Attributes["Course"] : "N/A";

        public Person()
        {
            FullName = string.Empty;
            Room = string.Empty;
            CheckInDate = null;
            CheckOutDate = null;
            Attributes = new Dictionary<string, string>();
        }
    }

}
