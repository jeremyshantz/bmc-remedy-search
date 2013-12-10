namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IGroupClause
    {
        /// <summary>
        /// Joins this clause to the query
        /// </summary>
        ClauseOperator ClauseOperator { get; }

        /// <summary>
        /// Joins the sub-clauses of this clause
        /// </summary>
        ClauseOperator InterClauseOperator { get; }

        List<IValueClause> Values { get; }
    }
}
