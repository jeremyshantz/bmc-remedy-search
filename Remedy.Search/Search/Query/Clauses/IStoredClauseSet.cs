namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;

    public interface IStoredClauseSet
    {
        int ClauseCount { get; }
        List<IGroupClause> GroupParams { get; set; }
        List<IPairsGroupClause> PairsGroupParams { get; set; }
        List<IStatusGroupClause> StatusGroupParams { get; set; }
        List<IValueClause> ValueParams { get; set; }
    }
}
