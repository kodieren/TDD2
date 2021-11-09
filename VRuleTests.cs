using System;
using System.Collections.Generic;
using Xunit;

namespace Tdd2Validator
{
    public class VRuleTests
    {
        private static VRule<int> _isZero => VRule<int>.For((x) => x == 0);
        private static VRule<int> _isOne => VRule<int>.For((x) => x == 1);


        [Theory, MemberData(nameof(TestCases))]
        public void ExecuteTestCases(CompositeSpecification<int> rule, int candidate, bool expectedResult)
        {
            var actualResult = rule.IsSatisfiedBy(candidate);

            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> TestCases()
        {
            return new List<object[]>
            {
                new object[] { _isOne, 1, true },
                new object[] { _isOne, 0, false },
                new object[] { _isOne.And(_isZero), 0, false },
                new object[] { _isOne.And(_isZero), 1, false },
                new object[] { _isZero.And(_isZero), 1, false },
                new object[] { _isOne.And(_isOne), 1, true },
            };
        }
    }

    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
        ISpecification<T> And(ISpecification<T> other);
        //ISpecification<T> AndNot(ISpecification<T> other);
        ISpecification<T> Or(ISpecification<T> other);
        //ISpecification<T> OrNot(ISpecification<T> other);
        //ISpecification<T> Not();
    }


    public class VRule<T> : CompositeSpecification<T>
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

        public override bool IsSatisfiedBy(T candidate)
        {
            return _rule.Invoke(candidate);
        }
    }

    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);

        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);

        public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        private CompositeSpecification<T> _left;
        private ISpecification<T> _right;

        public OrSpecification(CompositeSpecification<T> compositeSpecification, ISpecification<T> other)
        {
            _left = compositeSpecification;
            _right = other;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
        }
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

        public override bool IsSatisfiedBy(T candidate)
        {
            return _thisSpecification.IsSatisfiedBy(candidate) && _other.IsSatisfiedBy(candidate);
        }
    }
}