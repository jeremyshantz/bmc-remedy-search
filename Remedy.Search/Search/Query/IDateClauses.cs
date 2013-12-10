using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public interface IDateClauses
    {
        QueryBuilder Today();

        QueryBuilder Tomorrow();

        QueryBuilder Yesterday();

        QueryBuilder InThePastWeek();

        QueryBuilder ThisMonth();

        QueryBuilder LastMonth();

        QueryBuilder ThisYear();

        QueryBuilder LastYear();

        QueryBuilder On(DateTime date);

        QueryBuilder Before(DateTime date);

        QueryBuilder After(DateTime date);

        QueryBuilder Between(DateTime after, DateTime before);

        Interval InThePast(int number);

        Interval InTheNext(int number);
    }
}
