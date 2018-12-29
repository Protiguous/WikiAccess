namespace WikiAccess {

	using System;
	using System.Collections.Generic;

	public class WikidataIOErrorLog : IErrorLog {

		public String Module => "D";

		public List<ErrorMessage> Errors { get; set; }

		public WikidataIOErrorLog() {
			this.Errors = new List<ErrorMessage>();
#if DEBUG
			this.Errors.Add( new ErrorMessage( this.Module, 0, "WikidataIO module" ) );
#endif
		}

		public void UnableToRetrieveData() => this.Errors.Add( new ErrorMessage( this.Module, 1, "Unable to retrieve data" ) );
	}
}