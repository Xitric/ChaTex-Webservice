using System;
using Business;
using Business.Models;
using DAL.Models;
using DAL.Mapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    class GroupRepository : IGroupRepository
    {
        public void AddMemberToGroup(GroupUserModel groupUserModel)
        {
            using (var context = new ChatexdbContext())
            {
                var entity = GroupUserMapper.MapGroupUserModelToEntity(groupUserModel);

                context.GroupUser.Add(entity);
                context.SaveChanges();
            }
        }

        public void AddMembersToGroup(IEnumerable<GroupUserModel> groupUserModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = groupUserModels.Select(x => GroupUserMapper.MapGroupUserModelToEntity(x));

                context.GroupUser.AddRange(entities);
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

        public bool DeleteGroup(int groupId)
        {
            using (var context = new ChatexdbContext())
            {
                var entity = context.Group.FirstOrDefault(x => x.GroupId == groupId);

                entity.IsDeleted = true;
                context.SaveChanges();
            }

            return true;
        }

        public void RemoveUsersFromGroup(IEnumerable<GroupUserModel> groupUserModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = groupUserModels.Select(x => GroupUserMapper.MapGroupUserModelToEntity(x));

                context.GroupUser.RemoveRange(entities);
                context.SaveChanges();
            }
        }

        public void AddRolesToGroup(IEnumerable<GroupRoleModel> groupRoleModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = groupRoleModels.Select(x => GroupRoleMapper.MapGroupRoleModelToEntity(x));

                context.GroupRole.AddRange(entities);
                context.SaveChanges();
            }
        }

        public void RemoveRolesFromGroup(IEnumerable<GroupRoleModel> groupRoleModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = groupRoleModels.Select(x => GroupRoleMapper.MapGroupRoleModelToEntity(x));

                context.GroupRole.RemoveRange(entities);
                context.SaveChanges();
            }
        }

        public bool SetUserAdministratorOnGroup(GroupUserModel groupUserModel)
        {
            using (var context = new ChatexdbContext())
            {
                var groupUser = GroupUserMapper.MapGroupUserModelToEntity(groupUserModel);
                var existing = context.GroupUser.FirstOrDefault(gu => gu.GroupId == groupUser.GroupId && gu.UserId == groupUser.UserId);

                if (existing == null)
                {
                    return false;
                }

                existing.IsAdministrator = groupUser.IsAdministrator;

                context.SaveChanges();
            }

            return true;
        }
       
        public GroupUserModel GetGroupUser (int groupId, int loggedInUser)
        {
            using (var context = new ChatexdbContext())
            {
                return GroupUserMapper.MapGroupUserEntityToModel(
                                       context.GroupUser.FirstOrDefault(x => x.GroupId == groupId && x.UserId == loggedInUser));
            }
        }

        public IEnumerable<UserModel> GetAllGroupUsers(int groupId)
        {
            using (var context = new ChatexdbContext())
            {
                IQueryable<User> groupUsers = context.GroupUser
                    .Where(gu => gu.GroupId == groupId)
                    .Select(gu => gu.User);

                IQueryable<User> userMatchingGroupRole = context.UserRole
                    .Where(ur => context.GroupRole
                        .Where(gr => gr.GroupId == groupId)
                        .Select(gr => gr.Role.RoleId)
                        .Contains(ur.RoleId))
                    .Select(ur => ur.User);

                return groupUsers.Union(userMatchingGroupRole)
                    .Where(u => u.IsDeleted == false)
                    .Select(u => UserMapper.MapUserEntityToModel(u))
                    .ToList();
            }
        }

        public IEnumerable<UserModel> GetAllGroupAdmins(int groupId) {
            using (var context = new ChatexdbContext())
            {
                List<UserModel> userMatchingAdmin = context.GroupUser
                    .Where(i => i.GroupId == groupId && i.IsAdministrator == true)
                    .Select(x => UserMapper.MapUserEntityToModel(x.User))
                    .ToList();

                return userMatchingAdmin;
            }
        }

        public IEnumerable<GroupModel> GetGroupsForUser(int userId)
        {
            using (var context = new ChatexdbContext())
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
                return unionGroups.Select(g => GroupMapper.MapGroupEntityToModel(g))
                    .ToList();
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

        public void UpdateGroup(int groupId, string groupName, int callerId)
        {
            throw new NotImplementedException();
        }
    }
}
