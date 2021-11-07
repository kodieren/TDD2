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
    }

    public struct VRule<T>
    {
        public static VRule<T> Define(Func<T, bool> rule)
        {
            return new VRule<T>();
        }
    }
}