
namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Container for passing the various types of stored clauses from RemedySearcher to IQualProviders
    /// </summary>
    public class StoredClauseSet : IStoredClauseSet
    {
        internal StoredClauseSet()
        {
        }

        public List<IValueClause> ValueParams { get; set; }

        public List<IStatusGroupClause> StatusGroupParams { get; set; }

        public List<IGroupClause> GroupParams { get; set; }

        public List<IPairsGroupClause> PairsGroupParams { get; set; }

        public int ClauseCount
        {
            get
            {
                int count = 0;

                if (this.ValueParams != null)
                {
                    count += this.ValueParams.Count;
                }

                if (this.StatusGroupParams != null)
                {
                    count += this.StatusGroupParams.Count;
                }

                if (this.GroupParams != null)
                {
                    count += this.GroupParams.Count;
                }

                if (this.PairsGroupParams != null)
                {
                    count += this.PairsGroupParams.Count;
                }

                return count;
            }
        }
    }
}
