using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public enum Operator
    {
        Equals,
        DoesNotEqual,
        Contains,
        DoesNotContain,
        StartsWith,
        DoesNotStartWith,
        EndsWith,
        DoesNotEndWith,
        GreaterThan,
        GreaterThanOrEqualTo,
        LesserThan,
        LesserThanOrEqualTo
    }
}
