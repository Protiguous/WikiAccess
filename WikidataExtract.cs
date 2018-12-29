// https://github.com/JamesNK/Newtonsoft.Json

namespace WikiAccess {

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using Newtonsoft.Json.Linq;

	/// <summary>
	///     Class to extract data from downloaded Wikidata
	///     This could have been part of WikidataIO.cs, but split out as it is large and cumbersome
	/// </summary>
	internal class WikidataExtract {

		private readonly WikidataCache _cache = new WikidataCache();

		private String Content { get; }

		public String[] ClaimsRequired { get; set; }

		public WikidataFields Fields { get; set; }

		public Boolean Success { get; set; }

		public WikidataExtractErrorLog WikidataExtractErrors { get; set; }

		public WikidataExtract( String content, String[] claimsrequired ) {
			this.WikidataExtractErrors = new WikidataExtractErrorLog();
			this.ClaimsRequired = claimsrequired;
			this.Fields = new WikidataFields();
			this.Content = content;
			this.Success = this.ExtractJSON();
		}

		/// <summary>
		///     Note: This method requires Newtonsoft Json to be installed
		/// </summary>
		/// <returns></returns>
		private Boolean ExtractJSON() {

			//Interpret the JSON - Basically read in a level at a time.
			var dataFromWiki = JObject.Parse( this.Content );
			var entities = ( JObject ) dataFromWiki[ "entities" ];

			var entity = entities.Properties().First(); // Name is variable, so grab data by using first method
			var entityKey = entity.Name;

			var entityData = ( JObject ) entity.Value;

			if ( entityKey == "-1" ) {
				this.WikidataExtractErrors.NotWikidata();

				return false;
			}

			var qcode = ( String ) entityData[ "id" ];
			this.Fields.ID = Convert.ToInt32( qcode.Substring( 1 ) );
			var entityType = ( String ) entityData[ "type" ];

			if ( entityType == null ) {
				this.WikidataExtractErrors.QcodeNotExist( entityKey );

				return false;
			}

			var descriptions = ( JObject ) entityData[ "descriptions" ];
			var labels = ( JObject ) entityData[ "labels" ];
			var wikipediaLinks = ( JObject ) entityData[ "sitelinks" ];

			if ( labels != null ) {
				foreach ( var label in labels.Properties() ) {
					var labelData = ( JObject ) label.Value;
					this.Fields.Labels.Add( ( String ) labelData[ "language" ], ( String ) labelData[ "value" ] );
				}
			}

			if ( descriptions != null ) {
				foreach ( var description in descriptions.Properties() ) {
					var descriptionData = ( JObject ) description.Value;
					var l = ( String ) descriptionData[ "language" ];
					this.Fields.Description.Add( ( String ) descriptionData[ "language" ], ( String ) descriptionData[ "value" ] );
				}
			}

			if ( wikipediaLinks != null ) {
				foreach ( var wikipediaLink in wikipediaLinks.Properties() ) {
					var wikipediaLinkData = ( JObject ) wikipediaLink.Value;
					this.Fields.WikipediaLinks.Add( ( String ) wikipediaLinkData[ "site" ], ( String ) wikipediaLinkData[ "title" ] );
				}
			}

			var claims = ( JObject ) entityData[ "claims" ];

			if ( claims != null ) {

				//Now we get to loop through each claim property for that article
				foreach ( var claim in claims.Properties() ) {
					var claimKey = claim.Name;

					if ( Array.IndexOf( this.ClaimsRequired, claimKey ) == -1 ) {
						continue;
					}

					var claimData = ( JArray ) claim.Value;

					//claimData is an array - another loop
					for ( var thisClaim = 0; thisClaim < claimData.Count; thisClaim++ ) {

						var thisClaimData = new WikidataClaim();

						var mainSnak = ( JObject ) claimData[ thisClaim ][ "mainsnak" ];
						var snakType = ( String ) mainSnak[ "snaktype" ];
						var snakDataType = ( String ) mainSnak[ "datatype" ];
						var snakDataValue = ( JObject ) mainSnak[ "datavalue" ];

						if ( snakType == "novalue" || snakType == "somevalue" ) {
							thisClaimData.ValueAsString = snakType;
						}
						else {
							if ( snakDataType == "string" || snakDataType == "commonsMedia" || snakDataType == "url" ) {
								thisClaimData.ValueAsString = ( String ) snakDataValue[ "value" ];
							}
							else if ( snakDataType == "wikibase-item" ) {
								var objectValue = ( JObject ) snakDataValue[ "value" ];
								thisClaimData.Qcode = ( Int32 ) objectValue[ "numeric-id" ];
								thisClaimData.ValueAsString = this._cache.RetrieveLabel( thisClaimData.Qcode );
							}
							else if ( snakDataType == "time" ) {
								var objectValue = ( JObject ) snakDataValue[ "value" ];

								var valueTime = ( String ) objectValue[ "time" ];

								var valueTimePrecision = ( String ) objectValue[ "precision" ];
								var valueTimeCalendarModel = ( String ) objectValue[ "calendarmodel" ];

								var julian = false;
								var gregorian = valueTimeCalendarModel != "http://www.Wikidata.org/entity/Q1985727";

								if ( valueTimeCalendarModel == "http://www.Wikidata.org/entity/Q1985786" ) {
									julian = true;
								}

								if ( valueTimePrecision == "11" || valueTimePrecision == "10" || valueTimePrecision == "9" || valueTimePrecision == "8" ||
								     valueTimePrecision == "7" || valueTimePrecision == "6" ) {
									var dateStart = valueTime.IndexOf( "-", 2 ) - 4;

									var thisDateString = valueTime.Substring( dateStart, 10 );
									thisDateString = thisDateString.Replace( "-00", "-01" ); // Occasionally get 1901-00-00 ?

									var validDate = true;
									DateTime thisDate;

									try {
										thisDate = DateTime.Parse( thisDateString, null, DateTimeStyles.RoundtripKind );
									}
									catch {
										thisDate = DateTime.MinValue;
										validDate = false;
									}

									if ( julian && valueTimePrecision == "11" ) {

										// All dates will be Gregorian
										// Julian flag tells us to display Julian date.
										// JulianCalendar JulCal = new JulianCalendar();
										// DateTime dta = JulCal.ToDateTime(thisDate.Year, thisDate.Month, thisDate.Day, 0, 0, 0, 0);
										// thisDate = dta;
									}

									var precision = DatePrecision.Null;

									if ( validDate == false ) {
										precision = DatePrecision.Invalid;
									}
									else if ( valueTime.Substring( 0, 1 ) == "+" ) {
										switch ( valueTimePrecision ) {
											case "11":
												precision = DatePrecision.Day;

												break;

											case "10":
												precision = DatePrecision.Month;

												break;

											case "9":
												precision = DatePrecision.Year;

												break;

											case "8":
												precision = DatePrecision.Decade;

												break;

											case "7":
												precision = DatePrecision.Century;

												break;

											case "6":
												precision = DatePrecision.Millenium;

												break;
										}
									}
									else {
										precision = DatePrecision.Bce;
									}

									thisClaimData.ValueAsDateTime.ThisDate = thisDate;
									thisClaimData.ValueAsDateTime.ThisPrecision = precision;
								}
							}
							else if ( snakDataType == "monolingualtext" ) {
								var objectValue = ( JObject ) snakDataValue[ "value" ];
								var valueText = ( String ) objectValue[ "text" ];
								var valueLanguage = ( String ) objectValue[ "language" ];

								// TODO Multi language handling
								thisClaimData.ValueAsString = valueText + "(" + valueLanguage + ")";
							}
							else if ( snakDataType == "quantity" ) {
								var objectValue = ( JObject ) snakDataValue[ "value" ];
								var valueAmount = ( String ) objectValue[ "amount" ];
								var valueUnit = ( String ) objectValue[ "unit" ];
								var valueUpper = ( String ) objectValue[ "upperBound" ];
								var valueLower = ( String ) objectValue[ "lowerBound" ];

								thisClaimData.ValueAsString = "(" + valueLower + " to " + valueUpper + ") Unit " + valueUnit;
							}
						}

						this.Fields.Claims.Add( new KeyValuePair<Int32, WikidataClaim>( Convert.ToInt32( claimKey.Substring( 1 ) ), thisClaimData ) );
					}
				}
			}

			return true;
		}
	}
}