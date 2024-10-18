using System.Collections.Generic;
using MEFedMVVM.Testability.Moq;
using MEFedMVVMDemo.Services.Contracts;
using MEFedMVVM.Services.Contracts;
using MEFedMVVMDemo.ViewModels;
using MEFedMVVMDemo.Services.Models;
using NSubstitute;
using NUnit.Framework;

namespace TestMEFedMVVMDemo
{
    /// <summary>
    /// name is kind of funny isn't it... These are the Tests for the TestViewModel
    /// </summary>
    [TestFixture]
    public class TestViewModelTest
    {
        [TestCase]
        public void Should_Load_Data_On_Loaded_Test()
        {
            //ARRANGE
			var autoStabber = new NSubstituteAutoStabber();

        	var usersService = Substitute.For<IUsersService>();
        	autoStabber.Add(typeof(IUsersService), usersService);

            var firstuser = new User();
            var mockUsersData = new List<User> { firstuser, new User() };

			usersService.GetAllUsers().Returns(mockUsersData);

            //ACT
			var viewModel = autoStabber.Create<UsersViewModel>();
           

            //ASSERT
            Assert.That(viewModel.Users, Has.Count.EqualTo(mockUsersData.Count), "View model did not load the users");
            
            //test that by default the first user in the list was selected
            Assert.That(viewModel.SelectedUser, Is.EqualTo(firstuser), "The first user was not selected by default");

            //test that the correct visual state was invoked
			autoStabber.Get<IVisualStateManager>().Received().GoToState("Welcome");
            
            //make sure that when the selected user was set a message was sent via the mediator
			autoStabber.Get<IMediator>().Received().NotifyColleagues(MEFedMVVMDemo.MediatorMessages.SelectedUser, firstuser);
        }

        [TestCase]
        public void Should_Select_User_Test()
        {
            //ARRANGE
            var mockUser = new User();
			var autoStabber = new NSubstituteAutoStabber();

            //ACT
			var viewModel = autoStabber.Create<UsersViewModel>();
			viewModel.SelectedUser = mockUser;

            //ASSERT
			Assert.That(viewModel.SelectedUser, Is.EqualTo(mockUser), "The selected user was not selected");
			//make sure that when the selected user was set a message was sent via the mediator
			autoStabber.Get<IMediator>().Received().NotifyColleagues(MEFedMVVMDemo.MediatorMessages.SelectedUser, mockUser);
        }
    }
}
