namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FieldIDClause : IValueClause
    {
        public ClauseOperator ClauseOperator { get; set; }

        public double Fieldid { get; set; }

        public Operator Operator { get; set; }

        public string Value { get; set; }

        public string Field()
        {
            return this.Fieldid.ToString();
        }

        public override string ToString()
        {
            return string.Format(" {0} ('{1}' {2} \"{3}\")",
                Enum.GetName(typeof(ClauseOperator), this.ClauseOperator),
                this.Fieldid.ToString(),
                Enum.GetName(typeof(Operator), this.Operator),
                this.Value
                );
        }
    }
}
