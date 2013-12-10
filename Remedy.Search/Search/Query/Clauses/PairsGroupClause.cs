namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PairsGroupClause : IPairsGroupClause
    {
        internal PairsGroupClause()
        {
            this.Pairs = new List<ValuePair>();
        }

        public ClauseOperator ClauseOperator { get; set; }

        public ClauseOperator InterClauseOperator { get; set; }

        public List<ValuePair> Pairs { get; set; }
    }
}
