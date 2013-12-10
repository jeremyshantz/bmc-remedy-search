namespace Remedy.Search.Query.Providers
{
    using Remedy.Search.Query;
    using Remedy.Search.Query.Clauses;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ReadAbleProvider : IQualProvider
    {
        public virtual string BuildQual(IQueryBuilder builder)
        {
            if (!builder.StatusClauseAdded)
            {
                // builder
            }

            var set = builder.StoredClauses;

            var clauses = this.ExecuteStoredClauses(set).Distinct(new ClauseComparer()).ToList();

            var qual = new StringBuilder();
            foreach (var clause in clauses)
            {
                if (string.IsNullOrEmpty(qual.ToString()))
                {
                    qual.Append(clause.Value ?? string.Empty);
                }
                else
                {
                    qual.Append(string.Format("{0} {1} ", clause.Operator == ClauseOperator.AND ? " AND" : " OR", clause.Value ?? string.Empty));
                }
            }

            return qual.ToString();
        }

        private List<Clause> ExecuteStoredClauses(IStoredClauseSet set)
        {
            var list = new List<Clause>();

            if (set.ValueParams != null)
            {
                foreach (var @param in set.ValueParams)
                {
                    list.Add(this.Render(@param));
                }
            }

            if (set.StatusGroupParams != null)
            {
                foreach (var @param in set.StatusGroupParams)
                {
                    list.Add(this.Render(@param));
                }
            }

            if (set.GroupParams != null)
            {
                foreach (var @param in set.GroupParams)
                {
                    list.Add(this.Render(@param));
                }
            }

            if (set.PairsGroupParams != null)
            {
                foreach (var @param in set.PairsGroupParams)
                {
                    list.Add(this.Render(@param));
                }
            }

            return list;
        }

        protected virtual string Build(IValueClause @param)
        {
            string fieldpart = string.Format("'{0}'", @param.Field());

            if (fieldpart == "\'\'")
            {
                throw new ArgumentNullException("@param.FieldID");
            }

            string format = string.Empty;

            switch (@param.Operator)
            {
                case Operator.Equals:
                    format = "{1} = \"{0}\"";
                    if (string.IsNullOrEmpty(@param.Value))
                    {
                        format = "({1} = \"{0}\" OR {1} = NULL)";
                    }
                    break;
                case Operator.DoesNotEqual:
                    format = "{1} != \"{0}\"";

                    if (string.IsNullOrEmpty(@param.Value))
                    {
                        format = "({1} != \"{0}\" AND {1} != NULL)";
                    }
                    break;
                case Operator.Contains:
                    format = "{1} LIKE \"%{0}%\"";
                    break;
                case Operator.DoesNotContain:
                    format = "NOT({1} LIKE \"%{0}%\")";
                    break;
                case Operator.StartsWith:
                    format = "{1} LIKE \"{0}%\"";
                    break;
                case Operator.DoesNotStartWith:
                    format = "NOT({1} LIKE \"{0}%\")";
                    break;
                case Operator.EndsWith:
                    format = "{1} LIKE \"%{0}\"";
                    break;
                case Operator.DoesNotEndWith:
                    format = "NOT({1} LIKE \"%{0}\")";
                    break;
                case Operator.GreaterThan:
                    format = "{1} > \"{0}\"";
                    break;
                case Operator.GreaterThanOrEqualTo:
                    format = "{1} >= \"{0}\"";
                    break;
                case Operator.LesserThan:
                    format = "{1} < \"{0}\"";
                    break;
                case Operator.LesserThanOrEqualTo:
                    format = "{1} <= \"{0}\"";
                    break;
            }

            return string.Format(format, @param.Value, fieldpart);
        }

        protected virtual string Build(ValuePair pair)
        {
            return string.Format("{0} {1} {2}", this.Build(pair.First), (pair.ClauseOperator == ClauseOperator.AND ? "AND" : "OR"), this.Build(pair.Second));
        }

        protected virtual Clause Render(IGroupClause @params)
        {
            string values = string.Empty;

            foreach (var value in @params.Values)
            {
                var clause = new Clause()
                {
                    Operator = @params.InterClauseOperator,
                    Value = this.Build(value)
                };

                if (string.IsNullOrEmpty(values))
                {
                    values += clause.Value ?? string.Empty;
                }
                else
                {
                    values += string.Format("{0} {1} ", clause.Operator == ClauseOperator.AND ? " AND" : " OR", clause.Value ?? string.Empty);
                }
            }

            return new Clause
            {
                Operator = @params.ClauseOperator,
                Value = string.Format("({0})", values)
            };
        }

        protected virtual Clause Render(IStatusGroupClause @param)
        {
            return this.Render(@param.AsGroupParameter());
        }

        protected virtual Clause Render(IPairsGroupClause pairsparam)
        {
            var values = string.Empty;
            var @operator = (pairsparam.InterClauseOperator == ClauseOperator.AND ? "AND" : "OR");

            foreach (var item in pairsparam.Pairs)
            {
                values += string.Format("({0}) {1} ", this.Build(item), @operator);
            }

            values = values.Trim();

            if (values.EndsWith(@operator))
            {
                values = values.Substring(0, values.Length - @operator.Length);
            }

            return new Clause
            {
                Operator = pairsparam.ClauseOperator,
                Value = values
            };
        }

        protected virtual Clause Render(IValueClause @param)
        {
            return new Clause
            {
                Operator = @param.ClauseOperator,
                Value = this.Build(@param)
            };
        }
    }
}
