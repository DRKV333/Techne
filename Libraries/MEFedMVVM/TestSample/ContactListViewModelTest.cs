using System;
using System.Collections.Generic;
using MEFedMVVMSample.Services;
using MEFedMVVM.Services.Contracts;
using MEFedMVVMSample.ViewModels;
using MEFedMVVMSample.Models;
using NUnit.Framework;
using NSubstitute;

namespace TestSample
{
    [TestFixture]
    public class ContactListViewModelTest
    {
        [TestCase]
        public void Should_Load_Contacts_InitData()
        {
            //ARRANGE
            var contactsDataService = Substitute.For<IContactsDataService>();
            var stateManager = Substitute.For<IVisualStateManager>();
            var dispatcher = new MockDispatcherService();
            var mockContactData = new List<Contact>
                {
                    new Contact()
                };

            Action<IEnumerable<Contact>> getContactsCallback = null;
            contactsDataService.GetContacts(Arg.Do<Action<IEnumerable<Contact>>>(x => getContactsCallback = x));

            //ACT.. by creating the VM we are also loading the data
            var viewModel = new ContactListViewModel(contactsDataService, stateManager, dispatcher);

            //we have to make our mock service actually pass back the mock data
            getContactsCallback(mockContactData);

            //ASSERT
            Assert.That(viewModel.Contacts, Has.Count.EqualTo(mockContactData.Count), "The contacts were not populated");
            contactsDataService.Received().GetContacts(Arg.Any<Action<IEnumerable<Contact>>>());
            stateManager.Received().GoToState("LoadingContacts");
            stateManager.Received().GoToState("ContactsLoaded");
        }

        [TestCase]
        public void Should_Search_Contacts_Execute()
        {
            //ARRANGE
            var contactsDataService = Substitute.For<IContactsDataService>();
            var stateManager = Substitute.For<IVisualStateManager>();
            var dispatcher = new MockDispatcherService();
            var mockContactData = new List<Contact>
                {
                    new Contact()
                };

            Action<IEnumerable<Contact>> getContactsByNameCallback = null;
            contactsDataService.GetContactsByName(Arg.Any<string>(), Arg.Do<Action<IEnumerable<Contact>>>(x => getContactsByNameCallback = x));

            //ACT.. 
            var viewModel = new ContactListViewModel(contactsDataService, stateManager, dispatcher);
            viewModel.SearchCommand.Execute("some string");

            //we have to make our mock service actually pass back the mock data
            getContactsByNameCallback(mockContactData);

            //ASSERT
            Assert.That(viewModel.Contacts, Has.Count.EqualTo(mockContactData.Count), "The contacts were not populated");
            contactsDataService.Received().GetContacts(Arg.Any<Action<IEnumerable<Contact>>>());
            stateManager.Received().GoToState("LoadingContacts");
            stateManager.Received().GoToState("ContactsLoaded");
        }

        [TestCase]
        public void Should_Search_Contacts_CanExecute()
        {
            //ARRANGE
            var contactsDataService = Substitute.For<IContactsDataService>();
            var stateManager = Substitute.For<IVisualStateManager>();
            var dispatcher = new MockDispatcherService();
           
            //ACT.. 
            var viewModel = new ContactListViewModel(contactsDataService, stateManager, dispatcher);

            //ASSERT
            Assert.That(viewModel.SearchCommand.CanExecute("some string"), Is.True);
            Assert.That(viewModel.SearchCommand.CanExecute(null), Is.False);
            Assert.That(viewModel.SearchCommand.CanExecute(""), Is.False);
        }
    }
}
