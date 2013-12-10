namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ValuePair
    {
        internal ValuePair(IValueClause first, ClauseOperator clauseoperator, IValueClause second)
        {
            this.First = first;
            this.Second = second;
            this.ClauseOperator = clauseoperator;
        }

        public ClauseOperator ClauseOperator { get; set; }

        public IValueClause First { get; set; }

        public IValueClause Second { get; set; }
    };
}
