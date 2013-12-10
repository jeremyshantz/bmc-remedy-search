using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface Interval
    {
        QueryBuilder Seconds();

        QueryBuilder Minutes();

        QueryBuilder Hours();

        QueryBuilder Days();

        QueryBuilder Weeks();

        QueryBuilder Months();
    }
}
