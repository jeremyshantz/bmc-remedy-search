namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IStatusGroupClause
    {
        StatusType Status { get; set; }

        GroupClause AsGroupParameter();
    }
}
