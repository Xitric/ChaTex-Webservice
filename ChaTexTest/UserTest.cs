using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using Business.Authentication;
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

        [TestMethod]
        public void TestLogin()
        {
            var userRepository = new UserRepository();
            var authenticator = new Authenticator(userRepository);
            var userManager = new UserManager(userRepository, authenticator);
            var actual = userManager.Login("anthy17@student.sdu.dk");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(actual), "Login didn't work.");
        }

        [TestMethod]
        public void TestCreateMessage()
        {
            var messageRepository = new MessageRepository();
            var groupRepository = new GroupRepository();
            var channelRepository = new ChannelRepository();
            var messageManager = new MessageManager(messageRepository, groupRepository, channelRepository);

            var callerId = 10;
            var channelid = 3;
            var content = "This is a unit test message.";

            var messageId = messageManager.CreateMessage(callerId, channelid, content);
            var message = messageManager.GetMessage(callerId, messageId);

            Assert.IsTrue(message != null);
            Assert.IsTrue(message.Id == messageId);
            Assert.IsTrue(message.Content == content);
            Assert.IsTrue(message.ChannelId == channelid);
        }

        [TestMethod]
        public void TestCreateGroup()
        {
            var groupRepository = new GroupRepository();
            var groupManager = new GroupManager(groupRepository);

            var callerId = 10;
            var groupId = groupManager.CreateGroup(callerId,"UnitTest");
            var groups = groupManager.GetGroupsForUser(callerId);
            var groupInGroup = groups.FirstOrDefault(x => x.Id == groupId);

            Assert.IsTrue(groupId != null);
            Assert.IsTrue(groupInGroup != null);
        }
    }
}
