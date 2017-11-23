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
                throw new ArgumentException("The user was not authorized to add users to the group", "callerId");
            }
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

        public bool DeleteGroup(int groupId, int callerId)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);
            if (loggedInUser.IsAdministrator)
            {
             return groupRepository.DeleteGroup(groupId);
            }
            return false;
        }

        public void UpdateGroup(int groupId, string groupName, int callerId)
        {
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);
            if(loggedInUser.IsAdministrator)
            {
                groupRepository.UpdateGroup(groupId, groupName, callerId);
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
                throw new ArgumentException("The user was not authorized to remove users to the group", "callerId");
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
                throw new ArgumentException("The user was not authorized to add roles to the group", "callerId");
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
                throw new ArgumentException("The user was not authorized to remove roles from the group", "callerId");
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
                throw new ArgumentException("The user was not authorized to change administrator", "callerId");
            }
        }

        public IEnumerable<UserModel> GetAllGroupUsers(int groupId, int callerId) { 
        
            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);

            if (loggedInUser != null)
            {
               return groupRepository.GetAllGroupUsers(groupId);
            }
            else
            {
                throw new ArgumentException("Failed to get all group users", "callerId");
            }
        }

        public IEnumerable<UserModel> GetAllGroupAdmins(int groupId, int callerId) {

            var loggedInUser = groupRepository.GetGroupUser(groupId, callerId);

            if (loggedInUser != null)
            {
                return groupRepository.GetAllGroupAdmins(groupId);
            }
            else
            {
                throw new ArgumentException("Failed to get all group admins", "callerId");
            }
        }

        public IEnumerable<GroupModel> GetGroupsForUser(int userId)
        {
            return groupRepository.GetGroupsForUser(userId);
        }

        public void UpdateGroup(int groupId, string groupName)
        {
            throw new NotImplementedException();
        }
    }
}
