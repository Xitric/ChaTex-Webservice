﻿using System.Collections.Generic;
using Business.Models;
using System.Linq;
using System;

namespace Business.Groups
{
    class GroupManager : IGroupManager
    {
        private readonly IGroupRepository groupRepository;
        public GroupManager(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        public void AddUsersToGroup(int groupId, List<int> userIds, int loggedInUserId)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, loggedInUserId);
            //Iterates through all user ids, creates a GroupUserModel and sends that to our group repository
            if (loggedInUser.IsAdministrator)
            {
                groupRepository.AddMembersToGroup(groupUserModel: userIds.Select(userId => new GroupUserModel()
                {
                    Group = new GroupModel()
                    {
                        Id = groupId
                    },
                    User = new UserModel()
                    {
                        Id = userId
                    }
                }));
            }
            else
            {
                throw new Exception("The user was not authorized to add users to the group");
            }
        }

        public void CreateGroup(int userId, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false)
        {
            var user = new UserModel()
            {
                Id = userId
            };

            GroupModel group = new GroupModel()
            {
                Creator = user,
                Name = groupName,
                AllowEmployeeSticky = allowEmployeeSticky,
                AllowEmployeeAcknowledgeable = allowEmployeeAcknowledgeable,
                AllowEmployeeBookmark = allowEmployeeBookmark,
            };

            group.Id = groupRepository.CreateGroup(group);

            //Add this user to GroupUser
            var groupUserModel = new GroupUserModel
            {
                Group = group,
                User = user,
                IsAdministrator = true
            };

            groupRepository.AddMemberToGroup(groupUserModel);

        }

        public bool DeleteGroup(int groupId)
        {
            return groupRepository.DeleteGroup(groupId); ;
        }

        public void RemoveUsersFromGroups(int groupId, List<int> userIds)
        {
            //Iterates through all user ids, creates a GroupUserModel and sends that to our group repository
            groupRepository.RemoveUsersFromGroups(groupUserModel: userIds.Select(userId => new GroupUserModel()
            {
                Group = new GroupModel()
                {
                    Id = groupId
                },
                User = new UserModel()
                {
                    Id = userId
                }
            }));
        }

        public void AddRolesToGroup(int groupId, int callerId, IEnumerable<int> roleIds)
        {
            groupRepository.AddRolesToGroup(groupRoleModel: roleIds.Select(roleId => new GroupRoleModel()
            {
                Group = new GroupModel()
                {
                    Id = groupId
                },
                Role = new RoleModel()
                {
                    Id = roleId,
                }
            }));
        }

        public void RemoveRolesFromGroup(int groupId, int callerId, IEnumerable<int> roleIds)
        {

        }

        public void MarkUserAsAdministrator(int userId, int callerId, bool isAdministrator)
        {

        }
    }
}
