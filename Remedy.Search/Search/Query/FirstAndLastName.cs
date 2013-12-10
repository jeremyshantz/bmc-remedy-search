using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public class FirstAndLastName: IFirstAndLastName
    {
        public FirstAndLastName(string first, string last)
        {
            this.FirstName = first;
            this.LastName = last;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
