using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiAccess
{
    public interface ErrorLog
    {
        string Module { get; }
        List<ErrorMessage> Errors { get; set; }
    }
}
