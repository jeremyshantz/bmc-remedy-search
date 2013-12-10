namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ValueClause : IValueClause
    {
        public ClauseOperator ClauseOperator { get; set; }

        public SearchField SearchField { get; set; }

        public Operator Operator { get; set; }

        public string Value { get; set; }

        public string Field()
        {
            return Enum.GetName(typeof(SearchField), this.SearchField);
        }

        public override string ToString()
        {
            return string.Format(" {0} ('{1}' {2} \"{3}\")",
                Enum.GetName(typeof(ClauseOperator), this.ClauseOperator),
                Enum.GetName(typeof(SearchField), this.SearchField),
                Enum.GetName(typeof(Operator), this.Operator),
                this.Value
                );
        }
    }
}
