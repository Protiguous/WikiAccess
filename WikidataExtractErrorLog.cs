namespace WikiAccess {

	using System;
	using System.Collections.Generic;

	public class WikidataExtractErrorLog : IErrorLog {

		public String Module => "E";

		public List<ErrorMessage> Errors { get; set; }

		public WikidataExtractErrorLog() {
			this.Errors = new List<ErrorMessage>();
#if DEBUG
			this.Errors.Add( new ErrorMessage( this.Module, 0, "WikidataExtract module" ) );
#endif
		}

		public void NotWikidata() => this.Errors.Add( new ErrorMessage( this.Module, 1, "Download not in expected format" ) );

		public void QcodeNotExist( String qcode ) => this.Errors.Add( new ErrorMessage( this.Module, 2, qcode + " not found on Wikidata" ) );
	}
}