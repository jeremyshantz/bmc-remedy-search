using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface IPersonClauses
    {
        QueryBuilder Is(string remedyloginid);

        QueryBuilder Is(string firstname, string lastname);

        QueryBuilder Is(IFirstAndLastName person);

        QueryBuilder IsAnyOf(params string[] remedyloginids);

        QueryBuilder IsSet();

        QueryBuilder IsNotSet();
    }
}
