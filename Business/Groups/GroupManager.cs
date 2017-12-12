using System.Collections.Generic;
using Business.Models;
using System.Linq;
using System.Threading;

namespace Business.Groups
{
    class GroupManager : AuthenticatedManager, IGroupManager
    {
        private readonly IGroupRepository groupRepository;
        private readonly ReaderWriterLock groupLock;

        public GroupManager(IGroupRepository groupRepository) : base(groupRepository)
        {
            this.groupRepository = groupRepository;

            groupLock = new ReaderWriterLock();
        }

        public int? CreateGroup(int userId, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false)
        {
            //No need to use locks here, as creating a group has no effect on, and is not affecte by other methods
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

            //Lock here to ensure that we don't accidentally delete a group that is being modified elsewhere
            try
            {
                groupLock.AcquireWriterLock(Timeout.Infinite);
                groupRepository.DeleteGroup(groupId);
            }
            finally
            {
                groupLock.ReleaseLock();
            }
        }

        public void UpdateGroup(int groupId, string groupName, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            //Lock to prevent concurrent modification and deletion of groups
            try
            {
                groupLock.AcquireWriterLock(Timeout.Infinite);
                groupRepository.UpdateGroup(groupId, groupName);
            }
            finally
            {
                groupLock.ReleaseLock();
            }
        }

        public void AddUsersToGroup(int groupId, IEnumerable<int> userIds, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            //Lock modification of groups to ensure that the group id not deleted while we add members
            try
            {
                groupLock.AcquireWriterLock(Timeout.Infinite);
                addUsersToGroupInternal(groupId, userIds);
            }
            finally
            {
                groupLock.ReleaseLock();
            }
        }

        private void addUsersToGroupInternal(int groupId, IEnumerable<int> userIds)
        {
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

        public void RemoveUsersFromGroup(int groupId, IEnumerable<int> userIds, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            //Lock modification of groups to ensure that the group is not deleted while we remove members
            try
            {
                groupLock.AcquireWriterLock(Timeout.Infinite);
                removeUsersFromGroupInternal(groupId, userIds);
            }
            finally
            {
                groupLock.ReleaseLock();
            }
        }

        private void removeUsersFromGroupInternal(int groupId, IEnumerable<int> userIds)
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

        public void AddRolesToGroup(int groupId, IEnumerable<int> roleIds, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            //Lock modification of groups to ensure that the group is not deleted while we add roles
            try
            {
                groupLock.AcquireWriterLock(Timeout.Infinite);
                addRolesToGroupInternal(groupId, roleIds);
            }
            finally
            {
                groupLock.ReleaseLock();
            }
        }

        private void addRolesToGroupInternal(int groupId, IEnumerable<int> roleIds)
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

        public void RemoveRolesFromGroup(int groupId, IEnumerable<int> roleIds, int callerId)
        {
            throwIfNotAdministrator(groupId, callerId);

            //Lock modification of groups to ensure that the group is not deleted while we add members
            try
            {
                groupLock.AcquireWriterLock(Timeout.Infinite);
                removeRolesFromGroupInternal(groupId, roleIds);
            }
            finally
            {
                groupLock.ReleaseLock();
            }
        }

        private void removeRolesFromGroupInternal(int groupId, IEnumerable<int> roleIds)
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

        public void SetUserAdministratorOnGroup(int groupId, int userId, int callerId, bool isAdministrator)
        {
            throwIfNotAdministrator(groupId, callerId);

            try
            {
                groupLock.AcquireWriterLock(Timeout.Infinite);
                setUserAdministratorOnGroupInternal(groupId, userId, isAdministrator);
            }
            finally
            {
                groupLock.ReleaseLock();
            }
        }

        private void setUserAdministratorOnGroupInternal(int groupId, int userId, bool isAdministrator)
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