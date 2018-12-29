namespace WikiAccess {

	using System;
	using System.IO;
	using System.Net;
	using System.Threading;

	/// <summary>
	///     Abstract class that does the actual call to the Wiki websites.
	///     Must set Target Framework to .NET framework 4 (not client profile)
	///     if you are using this in your own project, please change BOTNAME and CONTACT below
	/// </summary>
	public abstract class WikimediaApi {

		private Int32 _second;

		protected WikiMediaApiErrorLog ApiErrors { get; set; }

		protected abstract String ApIurl { get; }

		protected String Content { get; private set; }

		protected abstract String Parameters { get; }

		private const String Botname = "Perigliobot";

		private const String Contact = "Wikidata@lynxmail.co.uk";

		public WikimediaApi() => this.ApiErrors = new WikiMediaApiErrorLog();

		/// <summary>
		///     Download page from Wiki web site into temp file
		/// </summary>
		/// <returns>Temp file name</returns>
		private String DownloadPage() {
			var tempfile = Path.GetTempFileName();

			var wikiClient = new WebClient();
			wikiClient.Headers.Add( "user-agent", Botname + " Contact: " + Contact + ")" );
			var fullUrl = this.ApIurl + this.Parameters;

			try {
				wikiClient.DownloadFile( fullUrl, tempfile );
			}
			catch ( WebException e ) {
				tempfile = null;
				this.ApiErrors.CannotAccessWiki( fullUrl, e.Message );
			}

			return tempfile;
		}

		/// <summary>
		///     Read page from download, store in Content property
		/// </summary>
		/// <param name="tempfile"></param>
		private Boolean LoadPage( String tempfile ) {
			if ( tempfile == null ) {
				return false;
			}

			try {
				this.Content = File.ReadAllText( tempfile );
				File.Delete( tempfile );
			}
			catch ( Exception e ) {
				this.ApiErrors.UnableToRetrieveDownload( e.Message );

				return false;
			}

			return true;
		}

		/// <summary>
		///     Make sure we wait a second between calls.
		///     This simple method only throttles fast running scripts allowing slower ones to run at full speed.
		/// </summary>
		private void ThrottleWikiAccess() {
			if ( DateTime.Now.Second == this._second ) {
				Thread.Sleep( 1000 );
			}

			this._second = DateTime.Now.Second;
		}

		/// <summary>
		///     Method used to grab page from Wiki website, and store into Content property.
		/// </summary>
		/// <returns></returns>
		protected Boolean GrabPage() {
			this.ThrottleWikiAccess();

			return this.LoadPage( this.DownloadPage() );
		}
	}
}