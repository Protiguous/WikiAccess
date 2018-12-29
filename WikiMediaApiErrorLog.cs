namespace WikiAccess {

	using System;
	using System.Collections.Generic;

	public class WikiMediaApiErrorLog : IErrorLog {

		public String Module => "A";

		public List<ErrorMessage> Errors { get; set; }

		public WikiMediaApiErrorLog() {
			this.Errors = new List<ErrorMessage>();
#if DEBUG
			this.Errors.Add( new ErrorMessage( this.Module, 0, "WikimediaAPI module" ) );
#endif
		}

		/// <summary>
		///     Web server is not contactable. Either no Internet or an invalid URL
		/// </summary>
		public void CannotAccessWiki( String url, String systemMessage ) =>
			this.Errors.Add( new ErrorMessage( this.Module, 1, "Unable to contact Wiki URL " + url, systemMessage ) );

		/// <summary>
		///     No file was grabbed from the internet page. Unknown reason.
		/// </summary>
		public void NoFileDownloaded() => this.Errors.Add( new ErrorMessage( this.Module, 2, "No file downloaded" ) );

		public void UnableToRetrieveDownload( String systemMessage ) => this.Errors.Add( new ErrorMessage( this.Module, 3, "Unable to retrieve Download", systemMessage ) );
	}
}