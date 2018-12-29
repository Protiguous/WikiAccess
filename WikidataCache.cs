namespace WikiAccess {

	using System;
	using System.Collections.Generic;
	using System.IO;

	/// <summary>
	///     Class to create a cache of property labels, cutting down on Wikidata traffic.
	/// </summary>
	internal class WikidataCache {

		private readonly Dictionary<Int32, String> _cache = new Dictionary<Int32, String>();

		private readonly String _labelcache = Path.GetTempPath() + "WikidataLabelCache";

		/// <summary>
		///     Constructor. Reads in existing cache from LABELCACHE
		///     TODO Error trap for dodgy cache.
		/// </summary>
		public WikidataCache() {
			if ( !File.Exists( this._labelcache ) ) {
				File.Create( this._labelcache ).Close();
			}

			if ( this._cache.Count == 0 ) {
				using ( var sr = new StreamReader( this._labelcache ) ) {
					String propertyAsString;

					while ( ( propertyAsString = sr.ReadLine() ) != null ) {
						var property = Convert.ToInt32( propertyAsString );
						var description = sr.ReadLine();
						this._cache.Add( property, description );
					}
				}
			}
		}

		/// <summary>
		///     If its a new property, look up label on Wikidata and add to cache.
		/// </summary>
		/// <param name="qcode"></param>
		/// <returns></returns>
		private String LookupLabel( Int32 qcode ) {
			var io = new WikidataIO {
				Action = "wbgetentities", Format = "json", Ids = qcode, Props = "labels", Languages = "en|en-gb|ro"
			};

			var fields = io.GetData();

			if ( !fields.Labels.TryGetValue( "en-gb", out var name ) ) {
				if ( !fields.Labels.TryGetValue( "en", out name ) ) {
					fields.Labels.TryGetValue( "en", out name );
				}
			}

			using ( var sw = File.AppendText( this._labelcache ) ) {
				sw.WriteLine( qcode );
				sw.WriteLine( name );
			}

			this._cache.Add( qcode, name );

			return name;
		}

		public String RetrieveLabel( Int32 qcode ) {
			if ( this._cache.TryGetValue( qcode, out var description ) ) {
				return description;
			}

			return this.LookupLabel( qcode );
		}
	}
}