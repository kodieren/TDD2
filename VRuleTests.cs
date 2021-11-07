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
        private Func<T, bool> _rule;

        private VRule(Func<T, bool> rule)
        {
            _rule = rule;
        }

        public static VRule<T> Define(Func<T, bool> rule)
        {
            return new VRule<T>(rule);
        }

        public bool Validate(T value)
        {
            return _rule.Invoke(value);
        }
    }
}