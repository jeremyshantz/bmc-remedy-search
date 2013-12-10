using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface IStringFields
    {
        IStringClauses Summary();

        IStringClauses Description();

        IStringClauses TicketNumber();

        IStringClauses Status();

        IStringClauses ParentID();

        IStringClauses AssigneePersonFullName();

        IStringClauses AssigneePersonUserID();

        IStringClauses AssigneeTeam();
    }
}
