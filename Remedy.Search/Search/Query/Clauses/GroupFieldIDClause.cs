namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public class GroupFieldIDClause : IGroupClause
    {
        public double Fieldid { get; set; }

        public ClauseOperator ClauseOperator { get; set; }

        public ClauseOperator InterClauseOperator { get; set; }

        public Operator Operator { get; set; }

        public string[] SearchTerms { get; set; }

        public List<IValueClause> Values
        {
            get
            {
                var list = new List<IValueClause>();

                foreach (var term in this.SearchTerms)
                {
                    list.Add(new FieldIDClause { Operator = this.Operator, Fieldid = this.Fieldid, Value = term, ClauseOperator = this.ClauseOperator });
                }

                return list;
            }
        }

        public override string ToString()
        {
            return string.Format(" {0} ('{1}' {2} \"{3}\")",
                Enum.GetName(typeof(ClauseOperator), this.ClauseOperator),
                this.Fieldid.ToString(),
                Enum.GetName(typeof(Operator), this.Operator),
                string.Join(string.Format(" {0} ", Enum.GetName(typeof(ClauseOperator), this.InterClauseOperator)), this.SearchTerms)
                );
        }
    }
}
