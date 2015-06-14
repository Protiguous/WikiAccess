using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiAccess
{
    /// <summary>
    /// General interface to Wikidata
    /// </summary>
    public class WikidataIO : WikimediaApi
    {
        protected override string APIurl { get { return @"http://www.Wikidata.org/w/api.php?"; } }
        protected override string Parameters
        {
            get
            {
                string Param = "action=" + Action;
                if (Format != "") Param += "&format=" + Format;
                if (Sites != "") Param += "&sites=" + Sites;
                if (Ids != 0) Param += "&ids=Q" + Ids.ToString();
                if (Props != "") Param += "&props=" + Props;
                if (Languages != "") Param += "&languages=" + Languages;

                return Param;
            }
        }
        private WikidataIOErrorLog WikidataErrors { get; set; }
        private ErrorLog ExternalErrors { get; set; }

        public string Action { get; set; }
        public string Format { get; set; }
        public string Sites { get; set; }
        public int Ids { get; set; }
        public string Props { get; set; }
        public string Languages { get; set; }
        public string[] ClaimsRequired { get; set; }

        public WikidataIO()
            : base()
        {
            WikidataErrors = new WikidataIOErrorLog();
        }

        public WikidataFields GetData()
        {
            if (GrabPage())
            {
                WikidataExtract Item = new WikidataExtract(Content, ClaimsRequired);
                ExternalErrors = Item.WikidataExtractErrors;
                if (Item.Success)
                    return Item.Fields;
                else
                    return null;
            }
            else
            {
                WikidataErrors.UnableToRetrieveData();
                return null;
            }
        }

        public List<ErrorLog> GetErrors()
        {
            List<ErrorLog> Logs = new List<ErrorLog>();
            Logs.Add(APIErrors);
            Logs.Add(WikidataErrors);
            Logs.Add(ExternalErrors);
            return Logs;
        }
    }
}
