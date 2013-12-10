using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface IStatusClauses
    {
        QueryBuilder Any();

        QueryBuilder Closed();

        QueryBuilder Open();
    }
}
