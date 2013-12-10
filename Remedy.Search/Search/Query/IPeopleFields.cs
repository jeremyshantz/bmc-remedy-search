using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface IPeopleFields
    {
        IPersonClauses Assignee();

        IPersonClauses Modifier();

        IPersonClauses Requester();

        IPersonClauses Submitter();
    }
}
