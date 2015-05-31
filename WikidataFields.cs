using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiAccess
{
    /// <summary>
    /// Container to hold a single item grabbed from Wikidata
    /// </summary>
    public class WikidataFields
    {
        public int ID { get; set; }
        public Dictionary<string, string> WikipediaLinks { get; set; }
        public Dictionary<string, string> Labels { get; set; }
        public Dictionary<string, string> Description { get; set; }
        public List<KeyValuePair<int, WikidataClaim>> Claims { get; set; }  // Cannot use Dictionary as can have multiple claims per item

        public WikidataFields()
        {
            WikipediaLinks = new Dictionary<string, string>();
            Labels = new Dictionary<string, string>();
            Description = new Dictionary<string, string>();
            Claims = new List<KeyValuePair<int, WikidataClaim>>();
        }
    }
}
