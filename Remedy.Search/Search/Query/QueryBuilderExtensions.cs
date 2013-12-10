using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query.Extensions
{
    /// <summary>
    /// Convenience methods for the QueryBuilder
    /// </summary>
    public static class QueryBuilderExtensions
    {
        public static QueryBuilder SummaryContains(this QueryBuilder builder, string searchterm)
        {
            return builder.Summary().Contains(searchterm);
        }

        public static QueryBuilder SummaryContains(this QueryBuilder builder, params string[] searchterms)
        {
            return builder.Summary().ContainsAllOf(searchterms);
        }

        public static QueryBuilder SummaryDoesNotContain(this QueryBuilder builder, string searchterm)
        {
            return builder.Summary().DoesNotContain(searchterm);
        }

        public static QueryBuilder SummaryEquals(this QueryBuilder builder, string searchterm)
        {
            return builder.Summary().Equals(searchterm);
        }

        public static QueryBuilder SummaryEquals(this QueryBuilder builder, params string[] searchterms)
        {
            return builder.Summary().Equals(searchterms);
        }

        public static QueryBuilder SummaryStartsWith(this QueryBuilder builder, string searchterm)
        {
            return builder.Summary().StartsWith(searchterm);
        }

        public static QueryBuilder SummaryStartsWith(this QueryBuilder builder, params string[] searchterms)
        {
            return builder.Summary().StartsWith(searchterms);
        }

        public static QueryBuilder SummaryEndsWith(this QueryBuilder builder, string searchterm)
        {
            return builder.Summary().EndsWith(searchterm);
        }

        public static QueryBuilder SummaryEndsWith(this QueryBuilder builder, params string[] searchterms)
        {
            return builder.Summary().EndsWith(searchterms);
        }

        public static QueryBuilder SubmittedToday(this QueryBuilder builder)
        {
            return builder.Submitted().Today();
        }

        public static QueryBuilder SubmittedYesterday(this QueryBuilder builder)
        {
            return builder.Submitted().Yesterday();
        }

        public static QueryBuilder SubmittedInThePastWeek(this QueryBuilder builder)
        {
            return builder.Submitted().InThePastWeek();
        }

        public static QueryBuilder SubmittedMonthToDate(this QueryBuilder builder)
        {
            return builder.Submitted().ThisMonth();
        }

        public static QueryBuilder SubmittedLastMonth(this QueryBuilder builder)
        {
            return builder.Submitted().LastMonth();
        }

        public static QueryBuilder SubmittedYearToDate(this QueryBuilder builder)
        {
            return builder.Submitted().ThisYear();
        }

        public static QueryBuilder SubmittedLastYear(this QueryBuilder builder)
        {
            return builder.Submitted().LastYear();
        }

        public static  QueryBuilder ModifiedToday(this QueryBuilder builder)
        {
            return builder.Modified().Today();
        }

        public static QueryBuilder ModifiedYesterday(this QueryBuilder builder)
        {
            return builder.Modified().Yesterday();
        }

        public static QueryBuilder ModifiedInThePastWeek(this QueryBuilder builder)
        {
            return builder.Modified().InThePastWeek();
        }

        public static QueryBuilder ModifiedMonthToDate(this QueryBuilder builder)
        {
            return builder.Modified().ThisMonth();
        }

        public static QueryBuilder ModifiedLastMonth(this QueryBuilder builder)
        {
            return builder.Modified().LastMonth();
        }

        public static QueryBuilder ModifiedYearToDate(this QueryBuilder builder)
        {
            return builder.Modified().ThisYear();
        }

        public static QueryBuilder ModifiedLastYear(this QueryBuilder builder)
        {
            return builder.Modified().LastYear();
        }

        /// <summary>
        /// Returns only tickets where the Status is equal to the provided status.
        /// </summary>
        /// <returns></returns>
        public static QueryBuilder StatusIs(this QueryBuilder builder, string status)
        {
            return builder.Status().Equals(status);
        }

        /// <summary>
        /// Returns only tickets where the Status is not equal to the provided status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static QueryBuilder StatusIsNot(this QueryBuilder builder, string status)
        {
            return builder.Status().DoesNotEqual(status);
        }

        /// <summary>
        /// Returns only tickets where the Status is equal to one of the provided statuses.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static QueryBuilder StatusIn(this QueryBuilder builder, params string[] statuses)
        {
            return builder.Status().DoesNotEqual(statuses);
        }

        /// <summary>
        /// Limits to results to tickets assigned to the provided Remedy group.
        /// </summary>
        /// <param name="groupname">The name of the Remedy group to which the ticket is assigned.</param>
        /// <returns></returns>
        public static QueryBuilder AssignedTo(this QueryBuilder builder, string groupname)
        {
            return builder.AssigneeTeam().Equals(groupname);
        }

        /// <summary>
        /// Limits to results to tickets assigned to the provided person
        /// </summary>
        /// <param name="firstname">The first name of the assignee.</param>
        /// <param name="lastname">The last name of the assignee.</param>
        /// <returns></returns>
        public static QueryBuilder AssignedTo(this QueryBuilder builder, string firstname, string lastname)
        {
            return builder.AssigneePersonFullName().Equals(string.Format("{0} {1}", firstname, lastname));
        }

        public static QueryBuilder RequestedBy(this QueryBuilder builder, string firstname, string lastname)
        {
            return builder.Requester().Is(firstname, lastname);
        }

        public static QueryBuilder SubmittedBy(this QueryBuilder builder, string remedyloginid)
        {
            return builder.Submitter().Is(remedyloginid);
        }

        public static QueryBuilder ModifiedBy(this QueryBuilder builder, string remedyloginid)
        {
            return builder.Modifier().Is(remedyloginid);
        }

        /// <summary>
        /// Returns only tickets that are assigned to a person.
        /// </summary>
        /// <returns></returns>
        public static QueryBuilder Assigned(this QueryBuilder builder)
        {
            return builder.People().Assignee().IsSet();
        }

        /// <summary>
        /// Returns only tickets that are not assigned to a person.
        /// </summary>
        /// <returns></returns>
        public static QueryBuilder Unassigned(this QueryBuilder builder)
        {
            return builder.People().Assignee().IsNotSet();
        }

        public static QueryBuilder AnyStatus(this QueryBuilder builder)
        {
            return builder.StatusIs().Any();
        }

        public static QueryBuilder Open(this QueryBuilder builder)
        {
            return builder.StatusIs().Open();
        }

        public static QueryBuilder Closed(this QueryBuilder builder)
        {
            return builder.StatusIs().Closed();
        }

        public static QueryBuilder ChildrenOf(this QueryBuilder builder, string parentticket)
        {
            return builder.ChildrenOf(new string[] { parentticket });
        }

        public static QueryBuilder ChildrenOf(this QueryBuilder builder, params string[] parenttickets)
        {
            return builder.AnyStatus()
                .Strings().ParentID().Equals(parenttickets);
        }

        public static QueryBuilder TicketNumberIs(this QueryBuilder builder, string ticket)
        {
            return builder.TicketNumberIn(new string[] { ticket });
        }

        public static QueryBuilder TicketNumberIn(this QueryBuilder builder, params string[] tickets)
        {
            return builder.AnyStatus() // because we are looking for particular tickets
                .Strings().TicketNumber().Equals(tickets);            
        }
    }
}
