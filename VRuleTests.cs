using System;
using Xunit;

namespace Tdd2Validator
{
    public class VRuleTests
    {
        [Fact]
        public void ValidationRuleCanBeSpecified()
        {
            var rule = VRule<int>.Define((x) => x == 0);
        }

        [Fact]
        public void ValidationRuleCanBeExecuted()
        {
            var rule = VRule<int>.Define((x) => x == 0);
            var result = rule.Validate(1);
        }
    }

    public struct VRule<T>
    {
        public static VRule<T> Define(Func<T, bool> rule)
        {
            return new VRule<T>();
        }

        internal bool Validate(T value)
        {
            return true;
        }
    }
}