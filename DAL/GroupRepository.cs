using System;
using Business;
using Business.Models;
using DAL.Models;
using DAL.Mapper;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    class GroupRepository : IGroupRepository
    {
        public GroupModel GetGroup(int groupId)
        {
            using (var context = new ChatexdbContext())
            {
                return GroupMapper.MapGroupEntityToModel(context.Group.Find(groupId));
            }
        }

        public void AddMemberToGroup(GroupUserModel groupUserModel)
        {
            using (var context = new ChatexdbContext())
            {
                var entity = GroupUserMapper.MapGroupUserModelToEntity(groupUserModel);

                if (context.GroupUser.Find(entity.GroupId, entity.UserId) == null)
                {
                    context.GroupUser.Add(entity);
                    context.SaveChanges();
                }
            }
        }

        public void AddMembersToGroup(IEnumerable<GroupUserModel> groupUserModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = groupUserModels.Select(x => GroupUserMapper.MapGroupUserModelToEntity(x));
                var entitiesToAdd = entities.Where(e => context.GroupUser.Find(e.GroupId, e.UserId) == null);

                context.GroupUser.AddRange(entitiesToAdd);
                context.SaveChanges();
            }
        }

        public int CreateGroup(GroupModel group)
        {
            var entity = GroupMapper.MapGroupModelToEntity(group);

            using (var context = new ChatexdbContext())
            {
                context.Group.Add(entity);
                context.SaveChanges();
            }

            return entity.GroupId;
        }

        public void DeleteGroup(int groupId)
        {
            using (var context = new ChatexdbContext())
            {
                var entity = context.Group.FirstOrDefault(x => x.GroupId == groupId);

                entity.IsDeleted = true;
                context.SaveChanges();
            }
        }

        public void RemoveUsersFromGroup(IEnumerable<GroupUserModel> groupUserModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = groupUserModels.Select(x => GroupUserMapper.MapGroupUserModelToEntity(x));
                var entitiesToRemove = context.GroupUser.Where(etr => entities.Where(e => e.UserId == etr.UserId && e.GroupId == etr.GroupId).Any());

                context.GroupUser.RemoveRange(entitiesToRemove);
                context.SaveChanges();
            }
        }

        public void AddRolesToGroup(IEnumerable<GroupRoleModel> groupRoleModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = groupRoleModels.Select(x => GroupRoleMapper.MapGroupRoleModelToEntity(x));
                var entitiesToAdd = entities.Where(e => context.GroupRole.Find(e.GroupId, e.RoleId) == null);

                context.GroupRole.AddRange(entitiesToAdd);
                context.SaveChanges();
            }
        }

        public void RemoveRolesFromGroup(IEnumerable<GroupRoleModel> groupRoleModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = groupRoleModels.Select(x => GroupRoleMapper.MapGroupRoleModelToEntity(x));
                var entitiesToRemove = context.GroupRole.Where(etr => entities.Where(e => e.RoleId == etr.RoleId && e.GroupId == etr.GroupId).Any());

                context.GroupRole.RemoveRange(entitiesToRemove);
                context.SaveChanges();
            }
        }

        public void SetUserAdministratorOnGroup(GroupUserModel groupUserModel)
        {
            using (var context = new ChatexdbContext())
            {
                var groupUser = GroupUserMapper.MapGroupUserModelToEntity(groupUserModel);
                var existing = context.GroupUser.FirstOrDefault(gu => gu.GroupId == groupUser.GroupId && gu.UserId == groupUser.UserId);

                if (existing == null)
                {
                    context.GroupUser.Add(groupUser);
                }
                else
                {
                    existing.IsAdministrator = groupUser.IsAdministrator;
                }

                context.SaveChanges();
            }
        }

        public IEnumerable<UserModel> GetAllGroupUsers(int groupId)
        {
            using (var context = new ChatexdbContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.Snapshot))
                {
                    Group group = context.Group.Find(groupId);
                    if (group != null && group.IsDeleted == true)
                    {
                        return new List<UserModel>();
                    }

                    IQueryable<User> groupUsers = context.GroupUser
                        .Where(gu => gu.GroupId == groupId)
                        .Select(gu => gu.User);

                    IQueryable<User> userMatchingGroupRole = context.UserRole
                        .Where(ur => context.GroupRole
                            .Where(gr => gr.GroupId == groupId)
                            .Select(gr => gr.Role.RoleId)
                            .Contains(ur.RoleId))
                        .Select(ur => ur.User);

                    List<UserModel> users = groupUsers
                        .Union(userMatchingGroupRole)
                        .Where(u => u.IsDeleted == false)
                        .Select(u => UserMapper.MapUserEntityToModel(u))
                        .ToList();

                    transaction.Commit();
                    return users;
                }
            }
        }

        public IEnumerable<UserModel> GetAllGroupAdmins(int groupId)
        {
            using (var context = new ChatexdbContext())
            {
                return context.GroupUser
                    .Where(i => i.GroupId == groupId && i.IsAdministrator == true)
                    .Select(x => UserMapper.MapUserEntityToModel(x.User))
                    .ToList();
            }
        }

        public IEnumerable<GroupModel> GetGroupsForUser(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.Snapshot))
                {
                    IQueryable<Group> groupsUser = context.GroupUser
                        .Where(gu => gu.UserId == userId)
                        .Select(gu => gu.Group);

                    IQueryable<Group> groupsRole = context.GroupRole
                        .Where(gr => context.UserRole
                            .Where(ur => ur.UserId == userId)
                            .Select(ur => ur.Role.RoleId)
                            .Contains(gr.RoleId))
                        .Select(gr => gr.Group);

                    IQueryable<Group> unionGroups = groupsUser.Union(groupsRole)
                        .Where(g => g.IsDeleted == false);

                    //Load and track only those channels that are not deleted
                    foreach (Group group in unionGroups)
                    {
                        context.Entry(group)
                        .Collection(g => g.Channel)
                        .Query()
                        .Where(c => c.IsDeleted == false)
                        .ToList();
                    }

                    //Convert to group models
                    List<GroupModel> groups = unionGroups
                        .Select(g => GroupMapper.MapGroupEntityToModel(g))
                        .ToList();

                    transaction.Commit();
                    return groups;
                }
            }
        }

        public void UpdateGroup(int groupId, string groupName)
        {
            using (var context = new ChatexdbContext())
            {
                var entity = context.Group.FirstOrDefault(x => x.GroupId == groupId);
                entity.Name = groupName;

                context.SaveChanges();
            }
        }

        public GroupMembershipDetails GetGroupMembershipDetailsForUser(int groupId, int userId)
        {
            bool isUserMember = GetAllGroupUsers(groupId).Where(u => u.Id == userId).Any();
            bool isUserAdmin = GetAllGroupAdmins(groupId).Where(u => u.Id == userId).Any();

            return new GroupMembershipDetails()
            {
                GroupId = groupId,
                UserId = userId,
                IsMember = isUserMember,
                IsAdministrator = isUserAdmin
            };
        }

        public IEnumerable<UserModel> GetAllDirectGroupUsers(int groupId)
        {
            using (var context = new ChatexdbContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.Snapshot))
                {
                    Group group = context.Group.Find(groupId);
                    if (group != null && group.IsDeleted == true)
                    {
                        return new List<UserModel>();
                    }

                    List<UserModel> users = context.GroupUser
                        .Where(gu => gu.GroupId == groupId)
                        .Select(gu => UserMapper.MapUserEntityToModel(gu.User))
                        .ToList();

                    transaction.Commit();
                    return users;
                }
            }
        }

        public IEnumerable<RoleModel> GetAllGroupRoles(int groupId)
        {
            using (var context = new ChatexdbContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.Snapshot))
                {
                    Group group = context.Group.Find(groupId);
                    if (group != null && group.IsDeleted == true)
                    {
                        return new List<RoleModel>();
                    }

                    List<RoleModel> roles = context.GroupRole
                        .Where(gr => gr.GroupId == groupId)
                        .Select(gr => RoleMapper.MapRoleEntityToModel(gr.Role))
                        .ToList();

                    transaction.Commit();
                    return roles;
                }
            }
        }
    }
}
