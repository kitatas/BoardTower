using System.Linq;
using BoardTower.Common.Application;
using BoardTower.Common.Utility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Utility
{
    [TestFixture]
    public sealed class CollectionHelperTests
    {
        [Test]
        public void CopyShuffle_WithNullArray_ThrowsQuitExceptionVO()
        {
            int[] array = null;

            Assert.That(() => array.CopyShuffle(), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void CopyShuffle_WithEmptyArray_ReturnsEmptyArray()
        {
            var array = new int[0];

            var result = array.CopyShuffle();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        public void CopyShuffle_WithSingleElement_ReturnsSingleElementArray()
        {
            var array = new[] { 42 };

            var result = array.CopyShuffle();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(42));
        }

        [Test]
        public void CopyShuffle_ReturnsDifferentArrayReference()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            var result = array.CopyShuffle();

            Assert.That(result, Is.Not.SameAs(array));
        }

        [Test]
        public void CopyShuffle_DoesNotModifyOriginalArray()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var originalCopy = (int[])array.Clone();

            array.CopyShuffle();

            Assert.That(array, Is.EqualTo(originalCopy));
        }

        [Test]
        public void CopyShuffle_PreservesAllElements()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            var result = array.CopyShuffle();

            Assert.That(result.OrderBy(x => x), Is.EqualTo(array.OrderBy(x => x)));
        }

        [Test]
        public void CopyShuffle_PreservesLength()
        {
            var array = new[] { 10, 20, 30, 40, 50 };

            var result = array.CopyShuffle();

            Assert.That(result.Length, Is.EqualTo(array.Length));
        }

        [Test]
        public void CopyShuffle_WithStringArray_PreservesAllElements()
        {
            var array = new[] { "alpha", "beta", "gamma", "delta" };

            var result = array.CopyShuffle();

            Assert.That(result.OrderBy(x => x), Is.EqualTo(array.OrderBy(x => x)));
        }
    }
}
