using System;
using Xunit;

namespace Tdd2Validator
{
    public class VRuleTests
    {
        [Fact]
        public void ValidationRuleCanBeSpecified()
        {
            var rule = VRule<int>.For((x) => x == 0);
        }

        [Fact]
        public void ValidationRuleCanBeExecuted()
        {
            var rule = VRule<int>.For((x) => x == 0);
            var result = rule.Validate(1);
        }

        [Fact]
        public void ValidCaseCanBeVerified()
        {
            var rule = VRule<int>.For((x) => x == 0);
            var result = rule.Validate(0);
            Assert.True(result);
        }

        [Fact]
        public void InvalidCaseCanBeVerified()
        {
            var rule = VRule<int>.For((x) => x == 0);
            var result = rule.Validate(1);
            Assert.False(result);
        }

        [Fact]
        public void RulesCanBeStacked()
        {
            var rule1 = VRule<int>.For(x => x == 0);
            var rule2 = VRule<int>.For(x => x == 0);

            var combinedRule = rule1 && rule2;
        }
    }

    public struct VRule<T>
    {
        private Func<T, bool> _rule;

        private VRule(Func<T, bool> rule)
        {
            _rule = rule;
        }

        public static VRule<T> For(Func<T, bool> rule)
        {
            return new VRule<T>(rule);
        }

        public bool Validate(T value)
        {
            return _rule.Invoke(value);
        }
    }
}