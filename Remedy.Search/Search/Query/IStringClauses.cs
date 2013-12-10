using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface IStringClauses
    {
        QueryBuilder Equals(string searchterm);

        QueryBuilder Equals(params string[] searchterms);

        QueryBuilder DoesNotEqual(string searchterm);

        QueryBuilder DoesNotEqual(params string[] searchterms);

        QueryBuilder EndsWith(string searchterm);

        QueryBuilder EndsWith(params string[] searchterms);

        QueryBuilder DoesNotEndWith(string searchterm);

        QueryBuilder DoesNotEndWith(params string[] searchterms);

        QueryBuilder StartsWith(string searchterm);

        QueryBuilder StartsWith(params string[] searchterms);

        QueryBuilder DoesNotStartWith(string searchterm);

        QueryBuilder DoesNotStartWith(params string[] searchterms);

        QueryBuilder Contains(string searchterm);

        QueryBuilder ContainsAllOf(params string[] searchterms);

        QueryBuilder ContainsAnyOf(params string[] searchterms);

        QueryBuilder DoesNotContain(string searchterm);

        QueryBuilder DoesNotContain(params string[] searchterms);

        QueryBuilder GreaterThan(string searchterm);

        QueryBuilder GreaterThan(params string[] searchterms);

        QueryBuilder GreaterThanOrEqualTo(string searchterm);

        QueryBuilder GreaterThanOrEqualTo(params string[] searchterms);

        QueryBuilder LesserThan(params string[] searchterms);

        QueryBuilder LesserThan(string searchterm);

        QueryBuilder LesserThanOrEqualTo(string searchterm);

        QueryBuilder LesserThanOrEqualTo(params string[] searchterms);
    }
}
