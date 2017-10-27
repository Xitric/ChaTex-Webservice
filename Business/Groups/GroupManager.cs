using System.Collections.Generic;
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

        public void AddUsersToGroup(int groupId, List<int> userIds, int callerId)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);
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

        public void DeleteGroup(int groupId, int callerId)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);
            if (loggedInUser.IsAdministrator)
            {
                if (!groupRepository.DeleteGroup(groupId))
                {
                    throw new Exception("Group could not be deleted, something went wrong in the database");
                }
            }
            else
            {
                throw new Exception("Group could not be delete, user is not administrator");
            }

        }

        public void RemoveUsersFromGroup(int groupId, List<int> userIds, int callerId)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);
            //Iterates through all user ids, creates a GroupUserModel and sends that to our group repository
            if (loggedInUser.IsAdministrator)
            {
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
            else
            {
                throw new Exception("The user was not authorized to remove users to the group");
            }
        }

        public void AddRolesToGroup(int groupId, int callerId, IEnumerable<int> roleIds)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);

            if (loggedInUser.IsAdministrator)
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
            else
            {
                throw new Exception("The user was not authorized to add roles to the group");
            }

        }

        public void RemoveRolesFromGroup(int groupId, int callerId, IEnumerable<int> roleIds)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);

            if (loggedInUser.IsAdministrator)
            {
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
            else
            {
                throw new Exception("The user was not authorized to remove roles from the group");
            }
        }

        public void SetUserAdministratorOnGroup(int groupId, int userId, int callerId, bool isAdministrator)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);

            if (loggedInUser.IsAdministrator)
            {
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
            else
            {
                throw new Exception("The user was not authorized to change administrator");
            }
        }

    }
}
