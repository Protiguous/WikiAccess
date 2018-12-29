namespace WikiAccess {

	using System;

	internal class WikiAccessSample {

		private static void Main( String[] args ) {
			var qcode = 15818798;

			var wio = new WikidataIO {
				Action = "wbgetentities",
				Format = "json",
				Sites = "",
				Ids = qcode,
				Props = "claims|descriptions|labels|sitelinks",
				Languages = "",
				ClaimsRequired = new String[ 5 ] {
					"P31", "P27", "P21", "P569", "P570"
				}
			};

			var fields = wio.GetData();

			Console.WriteLine( "-----Errors-----" );
			var errors = wio.GetErrors();

			foreach ( var thisLog in errors ) {
				if ( thisLog != null ) {
					foreach ( var error in thisLog.Errors ) {
						Console.WriteLine( error.ToString() );
					}
				}
			}

			if ( fields == null ) {
				return;
			}

			if ( !fields.Labels.TryGetValue( "en-gb", out var thisName ) ) {
				fields.Labels.TryGetValue( "en", out thisName );
			}

			if ( !fields.Description.TryGetValue( "en-gb", out var thisDescription ) ) {
				fields.Description.TryGetValue( "en", out thisDescription );
			}

			fields.WikipediaLinks.TryGetValue( "enwiki", out var thisWikipedia );

			Console.WriteLine( thisName );
			Console.WriteLine( thisDescription );

			Console.WriteLine( "====================" );

			var wpio = new WikipediaIO {
				Action = "query",
				Export = "Yes",
				ExportNoWrap = "Yes",
				Format = "xml",
				Redirects = "yes",
				Titles = thisWikipedia
			};

			if ( wpio.GetData() ) {
				var templates = wpio.TemplatesUsed;
				var categories = wpio.CategoriesUsed;

				Console.WriteLine( wpio.PageTitle );
				Console.WriteLine( $"{templates.Count} templates" );
				Console.WriteLine( $"{categories.Count} categories" );
			}

			var errors2 = wpio.GetErrors();

			foreach ( var thisLog in errors2 ) {
				if ( thisLog != null ) {
					foreach ( var error in thisLog.Errors ) {
						Console.WriteLine( error.ToString() );
					}
				}
			}
		}
	}
}