namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IValueClause
    {
        ClauseOperator ClauseOperator { get; set; }

        Operator Operator { get; set; }

        string Value { get; set; }

        string Field();
    }
}
