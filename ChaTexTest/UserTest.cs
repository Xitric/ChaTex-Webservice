using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Business;
using Business.Authentication;
using Business.Channels;
using Business.Errors;
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
        private readonly int workingCallerId = 10;
        private readonly int failingCallerId = 0;

        [TestMethod]
        public void TestLogin()
        {
            // arrange
            var userRepository = new UserRepository();
            var authenticator = new Authenticator(userRepository);
            var userManager = new UserManager(userRepository, authenticator);

            // act
            var actual = userManager.Login("anthy17@student.sdu.dk", "abc123");
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
            var messageId = messageManager.CreateMessage(workingCallerId, channelId, content);
            var message = messageManager.GetMessage(workingCallerId, messageId);
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
            var groupId = groupManager.CreateGroup(workingCallerId, "UnitTestGroup");
            var groups = groupManager.GetGroupsForUser(workingCallerId);
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
            var channelId = channelManager.CreateChannel((int) groupId, workingCallerId, "UnitTestChannel");
            var channel = channelRepository.GetChannel(channelId);
            // assert
            Assert.IsTrue(channel != null);
        }

        [TestMethod]
        public void TestForChannelInvalidArgumentExceptions()
        {
            // arrange
            var groupRepository = new GroupRepository();
            var groupId = 0;
            var channelId = 0;

            // arrange
            var messageRepository = new MessageRepository();
            var channelRepository = new ChannelRepository();
            var channelEventManager = new ChannelEventManager(messageRepository, channelRepository);
            var channelManager = new ChannelManager(channelRepository, groupRepository, channelEventManager);
           
            // assert
            Assert.ThrowsException<InvalidArgumentException>(() => channelManager.CreateChannel(groupId,failingCallerId, "UnitTestChannel"));
            Assert.ThrowsException<InvalidArgumentException>(() => channelManager.DeleteChannel(failingCallerId, channelId));
            Assert.ThrowsException<InvalidArgumentException>(() => channelManager.UpdateChannel(failingCallerId, channelId, "UnitTestChannel"));
            Assert.ThrowsException<InvalidArgumentException>(() => channelManager.GetChannelEvents(channelId, failingCallerId, DateTime.Now, CancellationToken.None));
        }

        [TestMethod]
        public void TestForGroupInvalidArgumentExceptions()
        {
            // arrange
            var groupRepository = new GroupRepository();
            var groupManager = new GroupManager(groupRepository);
            int groupId = 0;

            // act & assert
            Assert.ThrowsException<InvalidArgumentException>(() => groupManager.CreateGroup(failingCallerId, "UnitTestGroup"));
            Assert.ThrowsException<InvalidArgumentException>(() => groupManager.GetGroupsForUser(failingCallerId));
            Assert.ThrowsException<InvalidArgumentException>(() => groupManager.AddRolesToGroup(groupId, failingCallerId, new List<int>()));
            Assert.ThrowsException<InvalidArgumentException>(() => groupManager.AddUsersToGroup(groupId, new List<int>(), failingCallerId));
            Assert.ThrowsException<InvalidArgumentException>(() => groupManager.DeleteGroup(groupId, failingCallerId));
        }

        [TestMethod]
        public void TestForMessageInvalidArgumentExceptions()
        {
            // arrange
            var messageRepository = new MessageRepository();
            var groupRepository = new GroupRepository();
            var channelRepository = new ChannelRepository();
            var channelEventManager = new ChannelEventManager(messageRepository, channelRepository);
            var messageManager = new MessageManager(messageRepository, groupRepository, channelRepository, channelEventManager);
            var channelId = 2;
            var messageId = 1;
            var content = "This is a unit test message.";

            // act & assert
            Assert.ThrowsException<InvalidArgumentException>(() => messageManager.CreateMessage(failingCallerId, channelId, content));
            Assert.ThrowsException<InvalidArgumentException>(() => messageManager.GetMessage(failingCallerId, messageId));
            Assert.ThrowsException<InvalidArgumentException>(() => messageManager.DeleteMessage(failingCallerId, messageId));
            Assert.ThrowsException<InvalidArgumentException>(() => messageManager.EditMessage(failingCallerId, messageId, content));
            Assert.ThrowsException<InvalidArgumentException>(() => messageManager.GetMessages(channelId, failingCallerId, DateTime.Now, 10));
        }
    }
}
