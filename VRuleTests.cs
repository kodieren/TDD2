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

        [Fact]
        public void ValidCaseCanBeVerified()
        {
            var rule = VRule<int>.Define((x) => x == 0);
            var result = rule.Validate(0);
            Assert.True(result);
        }
        
        [Fact]
        public void InvalidCaseCanBeVerified()
        {
            var rule = VRule<int>.Define((x) => x == 0);
            var result = rule.Validate(1);
            Assert.False(result);
        }
    }

    public struct VRule<T>
    {
        public static VRule<T> Define(Func<T, bool> rule)
        {
            return new VRule<T>();
        }

        public bool Validate(T value)
        {
            return true;
        }
    }
}