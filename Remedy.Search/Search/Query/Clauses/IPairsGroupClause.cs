namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPairsGroupClause
    {
        ClauseOperator ClauseOperator { get; set; }

        ClauseOperator InterClauseOperator { get; set; }

        List<ValuePair> Pairs { get; set; }
    }
}
