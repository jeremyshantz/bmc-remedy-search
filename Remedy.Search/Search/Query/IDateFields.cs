using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface IDateFields
    {
        IDateClauses Modified();

        IDateClauses Submitted();
    }
}
