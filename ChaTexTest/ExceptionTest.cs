using System;
using System.Collections.Generic;
using System.Text;
using Business.Channels;
using Business.Errors;
using Business.Groups;
using Business.Messages;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChaTexTest
{
    [TestClass]
    public class ExceptionTest
    {
        private readonly int failingCallerId = 0;

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
            Assert.ThrowsException<InvalidArgumentException>(() => channelManager.CreateChannel(groupId, failingCallerId, "UnitTestChannel"));
            Assert.ThrowsException<InvalidArgumentException>(() => channelManager.DeleteChannel(failingCallerId, channelId));
            Assert.ThrowsException<InvalidArgumentException>(() => channelManager.UpdateChannel(failingCallerId, channelId, "UnitTestChannel"));
        }

        [TestMethod]
        public void TestForGroupInvalidArgumentExceptions()
        {
            // arrange
            var groupRepository = new GroupRepository();
            var groupManager = new GroupManager(groupRepository);
            int groupId = 0;

            // act & assert
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
