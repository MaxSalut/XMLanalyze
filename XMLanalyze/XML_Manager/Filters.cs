using System;

namespace XMLanalyze.XML_Manager
{
    public class Filters
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public string NodeName { get; set; }
        public string NodeValue { get; set; }

        public Filters()
        {
            AttributeName = string.Empty;
            AttributeValue = string.Empty;
            NodeName = string.Empty;
            NodeValue = string.Empty;
        }

        public bool ValidatePerson(Person person)
        {
            bool attributeMatches = string.IsNullOrEmpty(AttributeName) ||
                                    (person.Attributes.ContainsKey(AttributeName) &&
                                     person.Attributes[AttributeName].Contains(AttributeValue, StringComparison.OrdinalIgnoreCase));

            bool nodeMatches = string.IsNullOrEmpty(NodeName) ||
                               (person.GetType().GetProperty(NodeName)?.GetValue(person)?.ToString()
                                   ?.Contains(NodeValue, StringComparison.OrdinalIgnoreCase) == true);

            return attributeMatches && nodeMatches;
        }
    }
}
