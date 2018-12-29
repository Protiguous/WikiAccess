namespace WikiAccess {

	using System;
	using System.Collections.Generic;

	/// <summary>
	///     Container to hold a single item grabbed from Wikidata
	/// </summary>
	public class WikidataFields {

		public List<KeyValuePair<Int32, WikidataClaim>> Claims { get; set; }

		public Dictionary<String, String> Description { get; set; }

		public Int32 ID { get; set; }

		public Dictionary<String, String> Labels { get; set; }

		public Dictionary<String, String> WikipediaLinks { get; set; }

		// Cannot use Dictionary as can have multiple claims per item

		public WikidataFields() {
			this.WikipediaLinks = new Dictionary<String, String>();
			this.Labels = new Dictionary<String, String>();
			this.Description = new Dictionary<String, String>();
			this.Claims = new List<KeyValuePair<Int32, WikidataClaim>>();
		}
	}
}