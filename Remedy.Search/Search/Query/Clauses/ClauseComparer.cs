namespace Remedy.Search.Query.Clauses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Compares Clauses allowing for the removal of duplicates
    /// </summary>
    internal class ClauseComparer : IEqualityComparer<Clause>
    {
        public bool Equals(Clause x, Clause y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Clause obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
