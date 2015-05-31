using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiAccess
{
    public class WikipediaIOErrorLog : ErrorLog
    {
        public string Module { get { return "W"; } }
        public List<ErrorMessage> Errors { get; set; }

        public WikipediaIOErrorLog()
        {
            Errors = new List<ErrorMessage>();
//            Errors.Add(new ErrorMessage(Module, 0, "WikipediaIO module"));
        }

        public void UnableToRetrieveData()
        {
            Errors.Add(new ErrorMessage(Module, 1, "Unable to retrieve data"));
        }

        public void UnableToParseXML()
        {
            Errors.Add(new ErrorMessage(Module, 2, "Unable to parse XML"));
        }
        
        public void ArticleNotExists()
        {
            Errors.Add(new ErrorMessage(Module, 3, "Wikipedia article does not exist"));
        }

        public void UnbalancedHTMLcomment()
        {
            Errors.Add(new ErrorMessage(Module,4, "Unbalanced HTML comments in article"));
        }

        public void UnbalancedCategoryBrackets()
        {
            Errors.Add(new ErrorMessage(Module,5, "Unbalanced Category brackets"));
        }

        public void UnbalancedTemplateBrackets()
        {
            Errors.Add(new ErrorMessage(Module,6, "Unbalanced Template brackets"));
        }
    }
}
