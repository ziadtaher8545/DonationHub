using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.ValueObjects
{
    public class ValueObject
    {
        public override bool Equals(object? obj)
        {
            if (obj is ValueObject other)
            {
                return GetType() == other.GetType();
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
        public static bool operator ==(ValueObject left, ValueObject right)
        {

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }

    }
}
