using Remedy.Search.Query.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface IQueryBuilder
    {
        IStoredClauseSet StoredClauses { get; }

        bool StatusClauseAdded { get;  }
    }
}
