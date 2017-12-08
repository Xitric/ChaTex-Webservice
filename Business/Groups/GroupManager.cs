using System.Collections.Generic;
using Business.Models;
using System.Linq;
using System;
using Business.Errors;
using System.Collections;

namespace Business.Groups
{
    class GroupManager : IGroupManager
    {
        private readonly IGroupRepository groupRepository;

        public GroupManager(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        private void throwIfNotAdministrator(int groupId, int callerId)
        {
            GroupMembershipDetails membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(groupId, callerId);

            if (!membershipDetails.IsAdministrator)
            {
                throw new InvalidArgumentException("The user must be an administrator of the group to perform this action", ParamNameType.CallerId);
            }
        }

        private void throwIfNotMember(int groupId, int callerId)
        {
            GroupMembershipDetails membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(groupId, callerId);

            if (!membershipDetails.IsMember)
            {
                throw new InvalidArgumentException("The user is not a member of the specified group", ParamNameType.CallerId);
            }
        }

        public void AddUsersToGroup(int groupId, List<int> userIds, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            //Iterates through all user ids, creates a GroupUserModel and sends that to our group repository
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

        public int? CreateGroup(int userId, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false)
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

            return group.Id;
        }

        public void DeleteGroup(int groupId, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            groupRepository.DeleteGroup(groupId);
        }

        public void UpdateGroup(int groupId, string groupName, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            groupRepository.UpdateGroup(groupId, groupName, callerId);
        }

        public void RemoveUsersFromGroup(int groupId, List<int> userIds, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            //Iterates through all user ids, creates a GroupUserModel and sends that to our group repository
            groupRepository.RemoveUsersFromGroup(groupUserModel: userIds.Select(userId => new GroupUserModel()
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
            throwIfNotAdministrator(groupId, callerId);

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
            throwIfNotAdministrator(groupId, callerId);

            groupRepository.RemoveRolesFromGroup(groupRoleModel: roleIds.Select(roleId => new GroupRoleModel()
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

        public void SetUserAdministratorOnGroup(int groupId, int userId, int callerId, bool isAdministrator)
        {
            throwIfNotAdministrator(groupId, callerId);

            groupRepository.SetUserAdministratorOnGroup(new GroupUserModel()
            {
                Group = new GroupModel()
                {
                    Id = groupId
                },
                User = new UserModel()
                {
                    Id = userId
                },

                IsAdministrator = isAdministrator
            });
        }

        public IEnumerable<UserModel> GetAllGroupUsers(int groupId, int callerId)
        {
            throwIfNotMember(groupId, callerId);

            return groupRepository.GetAllGroupUsers(groupId);
        }

        public IEnumerable<UserModel> GetAllGroupAdmins(int groupId, int callerId)
        {
            throwIfNotMember(groupId, callerId);

            return groupRepository.GetAllGroupAdmins(groupId);
        }

        public IEnumerable<GroupModel> GetGroupsForUser(int userId)
        {
            return groupRepository.GetGroupsForUser(userId);
        }

        public void UpdateGroup(int groupId, string groupName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetAllDirectGroupUsers(int groupId, int callerId)
        {
            return groupRepository.GetAllDirectGroupUsers(groupId);
        }

        public IEnumerable<RoleModel> GetAllGroupRoles(int groupId)
        {
            return groupRepository.GetAllGroupRoles(groupId);
        }
    }
}