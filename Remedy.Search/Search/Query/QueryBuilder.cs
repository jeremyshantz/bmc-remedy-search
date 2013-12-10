namespace Remedy.Search.Query
{    
    using Remedy.Search.Query.Clauses;   
    using Remedy.Search.Query.Providers;
    using System;
    using System.Collections.Generic;

    public partial class QueryBuilder : IQueryBuilder, IStringClauses, IPersonClauses, IDateClauses, IPeopleFields, IDateFields, Interval, IStringFields, IStatusClauses
    {
        private List<IValueClause> valueparams = new List<IValueClause>();
        private List<IStatusGroupClause> statusgroupparams = new List<IStatusGroupClause>();
        private List<IGroupClause> groupparams = new List<IGroupClause>();
        private List<IPairsGroupClause> pairsgroupparams = new List<IPairsGroupClause>();

        private double? pendingfieldid = null;
        private SearchField? pendingsearchfield = null;
        private int pendinginterval = 0;

        public bool StatusClauseAdded { get; set; }

        public IStoredClauseSet StoredClauses
        {
            get
            {
                return new StoredClauseSet
                {
                    GroupParams = this.groupparams,
                    PairsGroupParams = this.pairsgroupparams,
                    StatusGroupParams = this.statusgroupparams,
                    ValueParams = this.valueparams
                };
            }
        }
        
        private static DateTime Today()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        }

        private static DateTime Tomorrow()
        {
            return Today().AddDays(1);
        }

        private static DateTime FirstOfThisMonth()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, 1, 0, 0, 0);
        }

        private static DateTime FirstOfLastMonth()
        {
            var now = FirstOfThisMonth().AddMonths(-1);
            return new DateTime(now.Year, now.Month, 1, 0, 0, 0);
        }

        private static DateTime FirstOfThisYear()
        {
            return new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
        }

        private static DateTime FirstOfLastYear()
        {
            return FirstOfThisYear().AddYears(-1);
        }
        
        private double PendingFieldID
        {
            get
            {
                double value = pendingfieldid.Value;
                pendingfieldid = null; // one time use
                return value;
            }
        }

        private SearchField PendingSearchField
        {
            get
            {
                SearchField value = pendingsearchfield.Value;
                pendingsearchfield = null; // one time use
                return value;
            }
        }

        private bool SearchFieldIsPending
        {
            get
            {
                return this.pendingsearchfield.HasValue;
            }
        }

        public override string ToString()
        {
            return this.ToString(new ReadAbleProvider());
        }

        public string ToString(IQualProvider provider)
        {
            if (provider == null)
            {
                return string.Empty;
            }

            return provider.BuildQual(this);
        }

        private void SetPending(double fieldid)
        {
            this.pendingfieldid = fieldid;
        }

        private void SetPending(SearchField searchfield)
        {
            this.pendingsearchfield = searchfield;
        }

        private void AddParam(SearchField field, Operator @operator, string[] searchterms)
        {
            AddParam(field, @operator, searchterms, ClauseOperator.OR, ClauseOperator.AND);
        }

        private void AddParam(SearchField field, Operator @operator, string[] searchterms, ClauseOperator interclauseoperator, ClauseOperator clauseoperator)
        {
            if (field == SearchField.Status)
            {
                this.StatusClauseAdded = true;
            }

            this.groupparams.Add(new GroupClause
            {
                SearchField = field,
                ClauseOperator = clauseoperator,
                InterClauseOperator = interclauseoperator,
                Operator = @operator,
                SearchTerms = searchterms
            });
        }

        private void AddParam(SearchField field, Operator @operator, string searchterm)
        {
            if (field == SearchField.Status)
            {
                this.StatusClauseAdded = true;
            }

            this.valueparams.Add(new ValueClause
            {
                ClauseOperator = ClauseOperator.AND,
                SearchField = field,
                Operator = @operator,
                Value = searchterm
            });
        }

        private void AddParam(Operator @operator, string searchterm)
        {
            if (SearchFieldIsPending)
            {
                AddParam(PendingSearchField, @operator, searchterm);
                return;
            }

            AddParam(PendingFieldID, @operator, searchterm);
        }

        private void AddParam(Operator @operator, string[] searchterms)
        {
            AddParam(@operator, searchterms, ClauseOperator.AND);
        }

        private void AddParam(Operator @operator, string[] searchterms, ClauseOperator interclauseoperator)
        {
            this.AddParam(@operator, searchterms, interclauseoperator, ClauseOperator.AND);
        }

        private void AddParam(Operator @operator, string[] searchterms, ClauseOperator interclauseoperator, ClauseOperator clauseoperator)
        {
            if (SearchFieldIsPending)
            {
                AddParam(PendingSearchField, @operator, searchterms, interclauseoperator, clauseoperator);
                return;
            }

            AddParam(PendingFieldID, @operator, searchterms, interclauseoperator, clauseoperator);
        }

        private void AddParam(double fieldid, Operator @operator, string[] searchterms, ClauseOperator clauseoperator)
        {
            this.AddParam(@operator, searchterms, ClauseOperator.OR, clauseoperator);
        }

        private void AddParam(double fieldid, Operator @operator, string[] searchterms, ClauseOperator interclauseoperator, ClauseOperator clauseoperator)
        {
            if (searchterms == null || searchterms.Length == 0)
            {
                return;
            }

            this.groupparams.Add(new GroupFieldIDClause
            {
                ClauseOperator = clauseoperator,
                InterClauseOperator = interclauseoperator,
                Fieldid = fieldid,
                Operator = @operator,
                SearchTerms = searchterms
            });
        }

        private void AddParam(double fieldid, Operator @operator, string searchterm)
        {
            this.valueparams.Add(new FieldIDClause
            {
                ClauseOperator = ClauseOperator.AND,
                Fieldid = fieldid,
                Operator = @operator,
                Value = searchterm
            });
        }

        private void AddPersonParam(string userid)
        {
            if (SearchFieldIsPending)
            {
                var pendingsearchfield = PendingSearchField;

                if (pendingsearchfield == SearchField.RequesterFirstName || pendingsearchfield == SearchField.RequesterLastName)
                {
                    throw new NotSupportedException("Requester can only be searched by first name last name.");
                }
                else if (pendingsearchfield == SearchField.SubmittedBy)
                {
                    this.AddSubmitter(userid);
                }
                else if (pendingsearchfield == SearchField.LastModifiedBy)
                {
                    this.AddModifier(userid);
                }
                else if (pendingsearchfield == SearchField.AssigneePersonFullName || pendingsearchfield == SearchField.AssigneePersonUserID)
                {
                    this.AddAssignee(userid);
                }
            }
        }

        private void AddPersonParam(IFirstAndLastName person)
        {
            if (SearchFieldIsPending)
            {
                var pendingsearchfield = PendingSearchField;

                if (pendingsearchfield == SearchField.RequesterFirstName || pendingsearchfield == SearchField.RequesterLastName)
                {
                    this.AddRequester(person);
                }
                else if (pendingsearchfield == SearchField.SubmittedBy)
                {
                    throw new NotSupportedException("Submitter can only be searched by userid.");
                }
                else if (pendingsearchfield == SearchField.LastModifiedBy)
                {
                    throw new NotSupportedException("Modifier can only be searched by userid.");
                }
                else if (pendingsearchfield == SearchField.AssigneePersonFullName || pendingsearchfield == SearchField.AssigneePersonUserID)
                {
                    this.AddAssignee(person);
                }
            }
        }

        private void AddPersonParam(string[] userids)
        {
            if (SearchFieldIsPending)
            {
                var pendingsearchfield = PendingSearchField;

                if (pendingsearchfield == SearchField.RequesterFirstName || pendingsearchfield == SearchField.RequesterLastName)
                {
                    throw new NotSupportedException("Requester can only be searched by first name last name.");
                }
                else if (pendingsearchfield == SearchField.SubmittedBy)
                {
                    this.AddSubmitter(userids);
                }
                else if (pendingsearchfield == SearchField.LastModifiedBy)
                {
                    this.AddModifier(userids);
                }
                else if (pendingsearchfield == SearchField.AssigneePersonFullName || pendingsearchfield == SearchField.AssigneePersonUserID)
                {
                    this.AddAssignee(userids);
                }
            }
        }
        
        private void AddRequester(IFirstAndLastName person)
        {
            this.AddRequester(new IFirstAndLastName[] { person });
        }

        private void AddRequester(IFirstAndLastName[] people)
        {   
            var pairsparam = new PairsGroupClause
            {
                Pairs = new List<ValuePair>(),
                ClauseOperator = ClauseOperator.AND,
                InterClauseOperator = ClauseOperator.OR
            };

            foreach (var person in people)
            {
                pairsparam.Pairs.Add(
                    new ValuePair(
                        new ValueClause
                        {
                            ClauseOperator = ClauseOperator.AND,
                            SearchField = SearchField.RequesterFirstName,
                            Operator = Operator.Equals,
                            Value = person.FirstName
                        },
                        ClauseOperator.AND,
                        new ValueClause
                        {
                            ClauseOperator = ClauseOperator.AND,
                            SearchField = SearchField.RequesterLastName,
                            Operator = Operator.Equals,
                            Value = person.LastName
                        }));
            }

            this.pairsgroupparams.Add(pairsparam);
        }

        private void AddAssignee(string userid)
        {
            this.AddAssignee(new string[] { userid });
        }

        private void AddAssignee(string[] userids)
        {
            this.AddParam(SearchField.AssigneePersonUserID, Operator.Equals, userids);
        }

        private void AddAssignee(IFirstAndLastName person)
        {
            this.AddParam(SearchField.AssigneePersonFullName, Operator.Equals, string.Format("{0} {1}", person.FirstName, person.LastName));
        }

        private void AddSubmitter(string userid)
        {
            this.AddParam(SearchField.SubmittedBy, Operator.Equals, userid);
        }

        private void AddSubmitter(string[] userids)
        {
            this.AddParam(SearchField.SubmittedBy, Operator.Equals, userids, ClauseOperator.OR, ClauseOperator.AND);
        }

        private void AddModifier(string userid)
        {
            this.AddParam(SearchField.LastModifiedBy, Operator.Equals, userid);
        }

        private void AddModifier(string[] userids)
        {
            this.AddParam(SearchField.LastModifiedBy, Operator.Equals, userids, ClauseOperator.OR, ClauseOperator.AND);
        }

        private void AddToday(SearchField searchfield)
        {
            this.SetPending(searchfield);
            this.AddToday();
        }

        private void AddToday()
        {
            this.AddBetweenDates(Today(), Tomorrow());
        }

        private void AddTomorrow()
        {
            this.AddBetweenDates(Tomorrow(), Tomorrow().AddDays(1));
        }

        private void AddYesterday(SearchField searchfield)
        {
            this.SetPending(searchfield);
            this.AddYesterday();
        }

        private void AddYesterday()
        {
            this.AddBetweenDates(Today().AddDays(-1), Today());
        }

        private void AddInThePastWeek(SearchField searchfield)
        {
            this.SetPending(searchfield);
            this.AddInThePastWeek();
        }

        private void AddInThePastWeek()
        {
            this.AddBetweenDates(Today().AddDays(-7), Tomorrow());
        }

        private void AddMonthToDate(SearchField searchfield)
        {
            this.SetPending(searchfield);
            this.AddMonthToDate();
        }

        private void AddMonthToDate()
        {
            this.AddBetweenDates(FirstOfThisMonth(), Tomorrow());
        }

        private void AddLastMonth(SearchField searchfield)
        {
            this.SetPending(searchfield);
            this.AddLastMonth();
        }

        private void AddLastMonth()
        {
            this.AddBetweenDates(FirstOfLastMonth(), FirstOfThisMonth());
        }

        private void AddYearToDate(SearchField searchfield)
        {
            this.SetPending(searchfield);
            this.AddYearToDate();
        }

        private void AddYearToDate()
        {
            this.AddBetweenDates(FirstOfThisYear(), Tomorrow());
        }

        private void AddLastYear(SearchField searchfield)
        {
            this.SetPending(searchfield);
            this.AddLastYear();
        }

        private void AddLastYear()
        {
            this.AddBetweenDates(FirstOfLastYear(), FirstOfThisYear());
        }

        private void AddOnDate(DateTime date)
        {
            this.AddBetweenDates(date, date.AddDays(1));
        }

        private void AddOnDate(SearchField searchfield, DateTime date)
        {
            this.SetPending(searchfield);
            this.AddOnDate(date);
        }

        private void AddBeforeDate(DateTime date)
        {
            if (SearchFieldIsPending)
            {
                var pendingsearchfield = PendingSearchField;
                this.AddParam(pendingsearchfield, Operator.LesserThan, date.ToRemedyQualStringDate());
                return;
            }

            var pendingfieldid = PendingFieldID;
            this.AddParam(pendingfieldid, Operator.LesserThan, date.ToRemedyQualStringDate());
        }

        private void AddBeforeDate(SearchField searchfield, DateTime date)
        {
            this.SetPending(searchfield);
            this.AddBeforeDate(date);
        }

        private void AddAfterDate(DateTime date)
        {
            if (SearchFieldIsPending)
            {
                var pendingsearchfield = PendingSearchField;
                this.AddParam(pendingsearchfield, Operator.GreaterThanOrEqualTo, date.ToRemedyQualStringDate());
                return;
            }

            var pendingfieldid = PendingFieldID;
            this.AddParam(pendingfieldid, Operator.GreaterThanOrEqualTo, date.ToRemedyQualStringDate());
        }

        private void AddAfterDate(SearchField searchfield, DateTime date)
        {
            this.SetPending(searchfield);
            this.AddAfterDate(date);
        }

        private void AddBetweenDates(DateTime after, DateTime before)
        {
            if (after.CompareTo(before) > 0)
            {
                var tmp = before;
                before = after;
                after = tmp;
            }

            if (SearchFieldIsPending)
            {
                var pendingsearchfield = PendingSearchField;
                this.AddParam(pendingsearchfield, Operator.GreaterThanOrEqualTo, after.ToRemedyQualStringDate());
                this.AddParam(pendingsearchfield, Operator.LesserThan, before.ToRemedyQualStringDate());
                return;
            }

            var pendingfieldid = PendingFieldID;
            this.AddParam(pendingfieldid, Operator.GreaterThanOrEqualTo, after.ToRemedyQualStringDate());
            this.AddParam(pendingfieldid, Operator.LesserThan, before.ToRemedyQualStringDate());
        }

        private void AddBetweenDates(SearchField searchfield, DateTime after, DateTime before)
        {
            this.SetPending(searchfield);
            this.AddBetweenDates(after, before);
        }
    }

    public partial class QueryBuilder
    {
        /// <summary>
        /// Exposes all People fields.
        /// </summary>
        /// <returns></returns>
        public IPeopleFields People()
        {
            return this;
        }

        #region People

        public IPersonClauses Assignee()
        {
            this.SetPending(SearchField.AssigneePersonFullName);
            return this;
        }

        public IPersonClauses Modifier()
        {
            this.SetPending(SearchField.LastModifiedBy);
            return this;
        }

        public IPersonClauses Requester()
        {
            this.SetPending(SearchField.RequesterFirstName);
            return this;
        }

        public IPersonClauses Submitter()
        {
            this.SetPending(SearchField.SubmittedBy);
            return this;
        }

        #endregion

        /// <summary>
        /// Exposes all Date fields.
        /// </summary>
        /// <returns></returns>
        public IDateFields Dates()
        {
            return this;
        }

        #region  Dates
        
        public IDateClauses Modified()
        {
            this.SetPending(SearchField.LastModifiedDate);
            return this;
        }
        
        public IDateClauses Submitted()
        {
            this.SetPending(SearchField.SubmittedDate);
            return this;
        }
        
        #endregion

        public IStringFields Strings()
        {
            return this;
        }

        #region Strings

        public IStringClauses Description()
        {
            this.SetPending(SearchField.Description);
            return this;
        }

        /// <summary>
        /// Effective for Tasks only.
        /// </summary>
        /// <returns></returns>
        public IStringClauses ParentID()
        {
            this.SetPending(SearchField.ParentID);
            return this;
        }

        public IStringClauses Status()
        {
            this.SetPending(SearchField.Status);
            return this;
        }

        public IStatusClauses StatusIs()
        {
            return this;
        }

        public IStringClauses Summary()
        {
            this.SetPending(SearchField.Summary);
            return this;
        }

        public IStringClauses TicketNumber()
        {
            this.SetPending(SearchField.TicketNumber);
            return this;
        }
        
        public IStringClauses AssigneePersonFullName()
        {
            this.SetPending(SearchField.AssigneePersonFullName);
            return this;
        }

        public IStringClauses AssigneePersonUserID()
        {
            this.SetPending(SearchField.AssigneePersonUserID);
            return this;
        }

        public IStringClauses AssigneeTeam()
        {
            this.SetPending(SearchField.AssigneeTeam);
            return this;
        }

        #endregion
        
        public IStringClauses FieldString(double fieldid)
        {
            this.SetPending(fieldid);
            return this;
        }

        public IDateClauses FieldDate(double fieldid)
        {
            this.SetPending(fieldid);
            return this;
        }

        #region StatusClauses

        /// <summary>
        /// Causes Status to be ignored in searches unless specified via another method call. By default, searches are limited to open tickets only.
        /// </summary>
        /// <returns></returns>
        QueryBuilder IStatusClauses.Any()
        {
            this.StatusClauseAdded = true;
            return this;
        }

        /// <summary>
        /// Returns only tickets where the Status is a closed status.
        /// </summary>
        /// <returns></returns>
        QueryBuilder IStatusClauses.Closed()
        {
            this.StatusClauseAdded = true;
            this.statusgroupparams.Add(new StatusGroupClause { Status = StatusType.Closed });
            return this;
        }

        /// <summary>
        /// Returns only tickets where the Status is an open status.
        /// </summary>
        /// <returns></returns>
        QueryBuilder IStatusClauses.Open()
        {
            this.StatusClauseAdded = true;
            this.statusgroupparams.Add(new StatusGroupClause { Status = StatusType.Open });
            return this;
        }

        #endregion

        #region StringClauses members

        QueryBuilder IStringClauses.Equals(string searchterm)
        {
            this.AddParam(Operator.Equals, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.Equals(params string[] searchterms)
        {
            this.AddParam(Operator.Equals, searchterms, ClauseOperator.OR);
            return this;
        }

        QueryBuilder IStringClauses.DoesNotEqual(string searchterm)
        {
            this.AddParam(Operator.DoesNotEqual, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.DoesNotEqual(params string[] searchterms)
        {
            this.AddParam(Operator.DoesNotEqual, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.EndsWith(string searchterm)
        {
            this.AddParam(Operator.EndsWith, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.EndsWith(params string[] searchterms)
        {
            this.AddParam(Operator.EndsWith, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.DoesNotEndWith(string searchterm)
        {
            this.AddParam(Operator.DoesNotEndWith, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.DoesNotEndWith(params string[] searchterms)
        {
            this.AddParam(Operator.DoesNotEndWith, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.StartsWith(string searchterm)
        {
            this.AddParam(Operator.StartsWith, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.StartsWith(params string[] searchterms)
        {
            this.AddParam(Operator.StartsWith, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.DoesNotStartWith(string searchterm)
        {
            this.AddParam(Operator.DoesNotStartWith, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.DoesNotStartWith(params string[] searchterms)
        {
            this.AddParam(Operator.DoesNotStartWith, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.Contains(string searchterm)
        {
            this.AddParam(Operator.Contains, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.ContainsAllOf(params string[] searchterms)
        {
            this.AddParam(Operator.Contains, searchterms, ClauseOperator.AND);
            return this;
        }

        QueryBuilder IStringClauses.ContainsAnyOf(params string[] searchterms)
        {
            this.AddParam(Operator.Contains, searchterms, ClauseOperator.OR);
            return this;
        }

        QueryBuilder IStringClauses.DoesNotContain(string searchterm)
        {
            this.AddParam(Operator.DoesNotContain, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.DoesNotContain(params string[] searchterms)
        {
            this.AddParam(Operator.DoesNotContain, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.GreaterThan(string searchterm)
        {
            this.AddParam(Operator.GreaterThan, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.GreaterThan(params string[] searchterms)
        {
            this.AddParam(Operator.GreaterThan, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.GreaterThanOrEqualTo(string searchterm)
        {
            this.AddParam(Operator.GreaterThanOrEqualTo, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.GreaterThanOrEqualTo(params string[] searchterms)
        {
            this.AddParam(Operator.GreaterThanOrEqualTo, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.LesserThan(string searchterm)
        {
            this.AddParam(Operator.LesserThan, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.LesserThan(params string[] searchterms)
        {
            this.AddParam(Operator.LesserThan, searchterms);
            return this;
        }

        QueryBuilder IStringClauses.LesserThanOrEqualTo(string searchterm)
        {
            this.AddParam(Operator.LesserThanOrEqualTo, searchterm);
            return this;
        }

        QueryBuilder IStringClauses.LesserThanOrEqualTo(params string[] searchterms)
        {
            this.AddParam(Operator.LesserThanOrEqualTo, searchterms);
            return this;
        }

        #endregion

        #region PersonClauses members

        QueryBuilder IPersonClauses.Is(string remedyloginid)
        {
            this.AddPersonParam(remedyloginid);
            return this;
        }

        QueryBuilder IPersonClauses.Is(string firstname, string lastname)
        {
            return (this as IPersonClauses).Is(new FirstAndLastName(firstname, lastname));
        }

        QueryBuilder IPersonClauses.Is(IFirstAndLastName person)
        {
            this.AddPersonParam(person);
            return this;
        }

        QueryBuilder IPersonClauses.IsAnyOf(params string[] remedyloginids)
        {
            this.AddPersonParam(remedyloginids);
            return this;
        }


        QueryBuilder IPersonClauses.IsSet()
        {
            this.AddParam(Operator.DoesNotEqual, string.Empty);
            return this;
        }

        QueryBuilder IPersonClauses.IsNotSet()
        {
            this.AddParam(Operator.Equals, string.Empty);
            return this;
        }

        #endregion

        #region DateClauses members
        
        QueryBuilder IDateClauses.Today()
        {
            this.AddToday();
            return this;
        }

        QueryBuilder IDateClauses.Tomorrow()
        {
            this.AddTomorrow();
            return this;
        }

        QueryBuilder IDateClauses.Yesterday()
        {
            this.AddYesterday();
            return this;
        }

        QueryBuilder IDateClauses.InThePastWeek()
        {
            this.AddInThePastWeek();
            return this;
        }

        QueryBuilder IDateClauses.ThisMonth()
        {
            this.AddMonthToDate();
            return this;
        }

        QueryBuilder IDateClauses.LastMonth()
        {
            this.AddLastMonth();
            return this;
        }

        QueryBuilder IDateClauses.ThisYear()
        {
            this.AddYearToDate();
            return this;
        }

        QueryBuilder IDateClauses.LastYear()
        {
            this.AddLastYear();
            return this;
        }

        QueryBuilder IDateClauses.On(DateTime date)
        {
            this.AddOnDate(date);
            return this;
        }

        QueryBuilder IDateClauses.Before(DateTime date)
        {
            this.AddBeforeDate(date);
            return this;
        }

        QueryBuilder IDateClauses.After(DateTime date)
        {
            this.AddAfterDate(date);
            return this;
        }

        QueryBuilder IDateClauses.Between(DateTime after, DateTime before)
        {
            this.AddBetweenDates(after, before);
            return this;    
        }

        Interval IDateClauses.InThePast(int number)
        {
            this.pendinginterval = -number;
            return this;
        }

        Interval IDateClauses.InTheNext(int number)
        {
            this.pendinginterval = number;
            return this;
        }

        #endregion

        #region Interval members

        QueryBuilder Interval.Seconds()
        {
            var now = DateTime.Now;
            this.AddBetweenDates(now, now.AddSeconds(this.pendinginterval));
            return this;
        }

        QueryBuilder Interval.Minutes()
        {
            var now = DateTime.Now;
            this.AddBetweenDates(now, now.AddMinutes(this.pendinginterval));
            return this;
        }

        QueryBuilder Interval.Hours()
        {
            var now = DateTime.Now;
            this.AddBetweenDates(now, now.AddHours(this.pendinginterval));
            return this;
        }

        QueryBuilder Interval.Days()
        {
            this.AddBetweenDates(Today(), Today().AddDays(this.pendinginterval));
            return this;
        }

        QueryBuilder Interval.Weeks()
        {
            this.AddBetweenDates(Today(), Today().AddDays(7 * this.pendinginterval));
            return this;
        }

        QueryBuilder Interval.Months()
        {
            this.AddBetweenDates(Today(), Today().AddMonths(this.pendinginterval));
            return this;
        }

        #endregion
         
    }
}
