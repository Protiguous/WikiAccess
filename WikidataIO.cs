namespace WikiAccess {

	using System;
	using System.Collections.Generic;

	/// <summary>
	///     General interface to Wikidata
	/// </summary>
	public class WikidataIO : WikimediaApi {

		private IErrorLog ExternalErrors { get; set; }

		private WikidataIOErrorLog WikidataErrors { get; }

		protected override String ApIurl => @"http://www.Wikidata.org/w/api.php?";

		protected override String Parameters {
			get {
				var param = "action=" + this.Action;

				if ( this.Format != "" ) {
					param += "&format=" + this.Format;
				}

				if ( this.Sites != "" ) {
					param += "&sites=" + this.Sites;
				}

				if ( this.Ids != 0 ) {
					param += "&ids=Q" + this.Ids;
				}

				if ( this.Props != "" ) {
					param += "&props=" + this.Props;
				}

				if ( this.Languages != "" ) {
					param += "&languages=" + this.Languages;
				}

				return param;
			}
		}

		public String Action { get; set; }

		public String[] ClaimsRequired { get; set; }

		public String Format { get; set; }

		public Int32 Ids { get; set; }

		public String Languages { get; set; }

		public String Props { get; set; }

		public String Sites { get; set; }

		public WikidataIO() => this.WikidataErrors = new WikidataIOErrorLog();

		public WikidataFields GetData() {
			if ( this.GrabPage() ) {
				var item = new WikidataExtract( this.Content, this.ClaimsRequired );
				this.ExternalErrors = item.WikidataExtractErrors;

				if ( item.Success ) {
					return item.Fields;
				}

				return null;
			}

			this.WikidataErrors.UnableToRetrieveData();

			return null;
		}

		public List<IErrorLog> GetErrors() {
			var logs = new List<IErrorLog> {
				this.ApiErrors, this.WikidataErrors, this.ExternalErrors
			};

			return logs;
		}
	}
}