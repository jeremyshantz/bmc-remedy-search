namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StatusGroupClause : IStatusGroupClause
    {
        public StatusType Status { get; set; }

        public GroupClause AsGroupParameter()
        {
            var statuses = GetFormStatuses(this.Status);

            return new GroupClause
            {
                ClauseOperator = ClauseOperator.AND,
                InterClauseOperator = ClauseOperator.OR,
                SearchField = SearchField.Status,
                Operator = Operator.Equals,
                SearchTerms = statuses
            };
        }

        private string[] GetFormStatuses(StatusType status)
        {
            return status == StatusType.Open ? new string[] { "OpenStatus1", "OpenStatus2" } : new string[] { "ClosedStatus1", "ClosedStatus2" };
        }
    }
}
