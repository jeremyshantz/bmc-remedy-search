using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query.Clauses
{
    /// <summary>
    /// Holds a single rendered clause and the operator to join it to the rest of the qual. Eg. AND ('1000000182' = "WO0000000000000")
    /// </summary>
    public class Clause: IFormattable
    {
        internal Clause()
        { 
        }

        public string Value { get; set; }

        internal ClauseOperator Operator { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Operator.ToString(), this.Value);
        }

        public bool Equals(Clause obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj.Operator == this.Operator
                && obj.Value == this.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Clause))
            {
                return false;
            }

            return this.Equals(obj as Clause);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                return this.ToString();
            }

            ICustomFormatter fmt = formatProvider.GetFormat(this.GetType()) as ICustomFormatter;

            if (fmt != null)
            {
                return fmt.Format(format, this, formatProvider);
            }

            switch (format)
            {
                case "V":
                    return this.Value;
                case "O":
                    return this.Operator.ToString();
                case "G":
                default:
                    return this.ToString();
            }
        }
    }   
}
