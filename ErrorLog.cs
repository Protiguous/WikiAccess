namespace WikiAccess {

	using System;
	using System.Collections.Generic;

	public interface IErrorLog {

		List<ErrorMessage> Errors { get; set; }

		String Module { get; }
	}
}

/*
 * A = WikimediaAPI
 * B = WikidataBiography
 * C = Category (cf Wikipedia)
 * D = WikidataIO
 * E = WikidataExtract
 * G = WikipediaBiography
 * T = Template
 * W = WikipediaIO
 * Y = Category (cf Wikidata)
 */