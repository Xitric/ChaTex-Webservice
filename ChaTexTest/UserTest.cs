using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using Business.Authentication;
using Business.Channels;
using Business.Groups;
using Business.Messages;
using Business.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.Users;
using DAL;

namespace ChaTexTest
{
    [TestClass]
    public class UserTest
    {
        private readonly int callerId = 10;

        [TestMethod]
        public void TestLogin()
        {
            // arrange
            var userRepository = new UserRepository();
            var authenticator = new Authenticator(userRepository);
            var userManager = new UserManager(userRepository, authenticator);
            // act
            var actual = userManager.Login("anthy17@student.sdu.dk");
            // assert
            Assert.IsTrue(!string.IsNullOrWhiteSpace(actual), "Login didn't work.");
        }

        [TestMethod]
        public void TestCreateMessage()
        {
            // arrange
            var messageRepository = new MessageRepository();
            var groupRepository = new GroupRepository();
            var channelRepository = new ChannelRepository();
            var channelEventManager = new ChannelEventManager(messageRepository, channelRepository);
            var messageManager = new MessageManager(messageRepository, groupRepository, channelRepository, channelEventManager);
            var channelId = 2;
            var content = "This is a unit test message.";
            // act
            var messageId = messageManager.CreateMessage(callerId, channelId, content);
            var message = messageManager.GetMessage(callerId, messageId);
            // assert
            Assert.IsTrue(message != null);
            Assert.IsTrue(message.Id == messageId);
            Assert.IsTrue(message.Content == content);
            Assert.IsTrue(message.ChannelId == channelId);
        }

        [TestMethod]
        public void TestCreateGroupAndChannel()
        {
            // arrange
            var groupRepository = new GroupRepository();
            var groupManager = new GroupManager(groupRepository);
            // act
            var groupId = groupManager.CreateGroup(callerId, "UnitTestGroup");
            var groups = groupManager.GetGroupsForUser(callerId);
            var groupInGroup = groups.FirstOrDefault(x => x.Id == groupId);
            // assert
            Assert.IsTrue(groupId != null);
            Assert.IsTrue(groupInGroup != null);

            // arrange
            var messageRepository = new MessageRepository();
            var channelRepository = new ChannelRepository();
            var channelEventManager = new ChannelEventManager(messageRepository, channelRepository);
            var channelManager = new ChannelManager(channelRepository, groupRepository, channelEventManager);
            // act
            var channelId = channelManager.CreateChannel((int) groupId, callerId, "UnitTestChannel");
            var channel = channelRepository.GetChannel(channelId);
            // assert
            Assert.IsTrue(channel != null);
        }

        [TestMethod]
        public void TestForUserExceptions()
        {
            var userRepository = new UserRepository();
            var authenticator = new Authenticator(userRepository);
            var userManager = new UserManager(userRepository, authenticator); 
        }

        [TestMethod]
        public void TestForChannelExceptions()
        {
        }

        [TestMethod]
        public void TestForGroupExceptions()
        {
        }

        [TestMethod]
        public void TestForMessageExceptions()
        {
        }
    }
}
