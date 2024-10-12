using NUnit.Framework;
using System.Collections.Generic;

namespace GongSolutions.Wpf.DragDrop.UnitTests
{
    [TestFixture]
    public class DefaultDropHandlerTests
    {
        [TestCase]
        public void TestCompatibleTypes_Of_Same_Type()
        {
            Assert.That(DefaultDropHandler.TestCompatibleTypes(
                new List<string>(), 
                new[] { "Foo", "Bar" }));
        }

        [TestCase]
        public void TestCompatibleTypes_Common_Interface()
        {
            Assert.That(DefaultDropHandler.TestCompatibleTypes(
                new List<IInterface>(),
                new[] { new BaseClass(), new DerivedClassA() }));
        }

        [TestCase]
        public void TestCompatibleTypes_Collection_TooDerived()
        {
            Assert.That(!DefaultDropHandler.TestCompatibleTypes(
                new List<DerivedClassA>(),
                new[] { new BaseClass(), new DerivedClassA() }));
        }

        interface IInterface { }
        class BaseClass : IInterface { }
        class DerivedClassA : BaseClass { }
        class DerivedClassB : BaseClass { }
    }
}
