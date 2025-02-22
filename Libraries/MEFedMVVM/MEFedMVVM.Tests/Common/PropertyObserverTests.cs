﻿using System.ComponentModel;
using System.Text;
using MEFedMVVM.Common;
using NUnit.Framework;

namespace MEFedMVVMTests.Common
{
    [TestFixture]
    public class PropertyObserverTests
    {
        [TestCase]
        public void Test_DoOnce()
        {
            //ARRANGE
            var notifyPropertyChangedMock = new NotifyPropertyChangedMock();

            const string calledOnce = "1";
            var called = new StringBuilder();

            //ACT
            notifyPropertyChangedMock.OnChanged(x => x.TestProperty).DoOnce(x => called.Append(calledOnce) );
            notifyPropertyChangedMock.TestProperty = "asdasd";
            notifyPropertyChangedMock.TestProperty = "asdasd";

            //ASSERT
            Assert.That(called.ToString(), Is.EqualTo(calledOnce), "Property changed was not called with DoOnce");
        }

        [TestCase]
        public void Test_Do()
        {
            //ARRANGE
            var notifyPropertyChangedMock = new NotifyPropertyChangedMock();

            const string calledOnce = "1";
            var called = new StringBuilder();

            //ACT
            notifyPropertyChangedMock.OnChanged(x => x.TestProperty).Do(x => called.Append(calledOnce));
            notifyPropertyChangedMock.TestProperty = "asdasd";
            notifyPropertyChangedMock.TestProperty = "asdase";

            //ASSERT
            Assert.That(called.ToString(), Is.EqualTo(calledOnce + calledOnce), "Property changed was not called with DoOnce");
        }

        [TestCase]
        public void Test_Do_With_Unsubscribe()
        {
            //ARRANGE
            var notifyPropertyChangedMock = new NotifyPropertyChangedMock();

            const string calledOnce = "1";
            var called = new StringBuilder();

            //ACT
            using(notifyPropertyChangedMock.OnChanged(x => x.TestProperty).Do(x => called.Append(calledOnce)))
                notifyPropertyChangedMock.TestProperty = "asdasd";

            notifyPropertyChangedMock.TestProperty = "asdasd";

            //ASSERT
            Assert.That(called.ToString(), Is.EqualTo(calledOnce), "Property changed was not called with DoOnce");
        }
    }

    internal class NotifyPropertyChangedMock : INotifyPropertyChanged
    {
        private string _testProperty;
        public string TestProperty
        {
            get
            {
                return _testProperty;
            }
            set
            {
                _testProperty = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TestProperty"));
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
