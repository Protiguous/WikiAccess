# WikiAccess
c# classes that download data direct from Wikidata and Wikipedia

nb See my other project WikiBioValidate for further details. I wrote these classes specifically for my biographical validation work. Consequently, the only bits that I know work are the items I specifically required for my own esoteric project. For example. they totally ignore references. There are likely some datatypes that are not handled. The URL parameters ae hardcoded to produce what I liked!

Nevertheless, they work for me, and will provide a foundation for future work. I am hoping someone will come along and help with this project. 

Wiki Interface
==============
For a general interface to Wikidata and Wikipedia, the following files are required. One day I might split these off into a project of their own.

WikimediaApi.cs  This is the class that handles the Internet activity to grab the page. It is a base class for the following IO classes.

WikidataIO.cs  This class retrieves an item from Wikidata. It doesn't do much except store the data within a class
  WikidataFields.cs  Container class to store a Wikidata item
  WikidataClaim.cs Container class to store a "Claim"
  WikidataCache.cs Local cache of property labels
  WikidataExtract.cs Class which interprets the JSON exported by Wikidata (requires Newtonsoft)
 

WikipediaIO.cs This class retrieves a complete article from Wikipedia. This retrieves the text in its raw state, as seen when editing a Wikipedia page. There are methods to allow extraction of templates and categories.


Wikidate.cs - Class to store a date and its precision.
