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
            var result = rule.IsSatisfiedBy(1);
        }

        [Fact]
        public void ValidCaseCanBeVerified()
        {
            var rule = VRule<int>.For((x) => x == 0);
            var result = rule.IsSatisfiedBy(0);
            Assert.True(result);
        }

        [Fact]
        public void InvalidCaseCanBeVerified()
        {
            var rule = VRule<int>.For((x) => x == 0);
            var result = rule.IsSatisfiedBy(1);
            Assert.False(result);
        }

        [Fact]
        public void RulesCanBeStacked()
        {
            var rule1 = VRule<int>.For(x => x == 0);
            var rule2 = VRule<int>.For(x => x == 0);

            var combinedRule = rule1.And(rule2);
        }        
        
        [Fact]
        public void AndRulesCanBeVerifiedPositiveCase()
        {
            var rule1 = VRule<int>.For(x => x == 0);
            var rule2 = VRule<int>.For(x => x != 1);

            var combinedRule = rule1.And(rule2);
            var result = combinedRule.IsSatisfiedBy(0);

            Assert.True(result);
        }

        [Fact]
        public void AndRulesCanBeVerifiedNegativeCase()
        {
            var rule1 = VRule<int>.For(x => x == 0);
            var rule2 = VRule<int>.For(x => x != 0);

            var combinedRule = rule1.And(rule2);
            var result = combinedRule.IsSatisfiedBy(0);

            Assert.False(result);
        }
    }

    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
        ISpecification<T> And(ISpecification<T> other);
        //ISpecification<T> AndNot(ISpecification<T> other);
        //ISpecification<T> Or(ISpecification<T> other);
        //ISpecification<T> OrNot(ISpecification<T> other);
        //ISpecification<T> Not();
    }


    public class VRule<T> : CompositeSpecification<T>, ISpecification<T>
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

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return _rule.Invoke(candidate);
        }
    }

    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);
        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
    }

    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private CompositeSpecification<T> _thisSpecification;
        private ISpecification<T> _other;

        public AndSpecification(CompositeSpecification<T> compositeSpecification, ISpecification<T> other)
        {
            _thisSpecification = compositeSpecification;
            _other = other;
        }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(_thisSpecification, other);
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return _thisSpecification.IsSatisfiedBy(candidate) && _other.IsSatisfiedBy(candidate);
        }
    }
}