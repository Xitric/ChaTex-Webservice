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
            using (var db = new ChatexdbContext())
            {
                var entity = GroupUserMapper.MapGroupUserModelToEntity(groupUserModel);
                db.GroupUser.Add(entity);
                db.SaveChanges();
            }
        }

        public void AddMembersToGroup(IEnumerable<GroupUserModel> groupUserModels)
        {
            using (var db = new ChatexdbContext())
            {
                var entities = groupUserModels.Select(x => GroupUserMapper.MapGroupUserModelToEntity(x));
                db.GroupUser.AddRange(entities);
                db.SaveChanges();
            }
        }

        public int CreateGroup(GroupModel group)
        {
            var entity = GroupMapper.MapGroupModelToEntity(group);
            using (var db = new ChatexdbContext())
            {
                db.Group.Add(entity);
                db.SaveChanges();
            }
            return entity.GroupId;
        }

        public bool DeleteGroup(int groupId)
        {
            using (var db = new ChatexdbContext())
            {
                var entity = db.Group.FirstOrDefault(x => x.GroupId == groupId);
                entity.IsDeleted = true;
                db.SaveChanges();
            }
            return true;
        }

        public void RemoveUsersFromGroup(IEnumerable<GroupUserModel> groupUserModels)
        {
            using (var db = new ChatexdbContext())
            {
                var entities = groupUserModels.Select(x => GroupUserMapper.MapGroupUserModelToEntity(x));
                db.GroupUser.RemoveRange(entities);
                db.SaveChanges();
            }
        }

        public void AddRolesToGroup(IEnumerable<GroupRoleModel> groupRoleModels)
        {
            using (var db = new ChatexdbContext())
            {
                var entities = groupRoleModels.Select(x => GroupRoleMapper.MapGroupRoleModelToEntity(x));
                db.GroupRole.AddRange(entities);
                db.SaveChanges();
            }
        }

        public void RemoveRolesFromGroup(IEnumerable<GroupRoleModel> groupRoleModels)
        {
            using (var db = new ChatexdbContext())
            {
                var entities = groupRoleModels.Select(x => GroupRoleMapper.MapGroupRoleModelToEntity(x));
                db.GroupRole.RemoveRange(entities);
                db.SaveChanges();
            }
        }

        public bool SetUserAdministratorOnGroup(GroupUserModel groupUserModel)
        {
            using (var db = new ChatexdbContext())
            {
                var groupUser = GroupUserMapper.MapGroupUserModelToEntity(groupUserModel);
                var existing = db.GroupUser.FirstOrDefault(gu => gu.GroupId == groupUser.GroupId && gu.UserId == groupUser.UserId);
                if (existing == null) return false;

                existing.IsAdministrator = groupUser.IsAdministrator;

                db.SaveChanges();
            }

            return true;
        }
       
        public GroupUserModel GetGroupUser (int groupId, int loggedInUser)
        {
            using (var db = new ChatexdbContext())
            {
                return GroupUserMapper.MapGroupUserEntityToModel(
                                       db.GroupUser.FirstOrDefault(x => x.GroupId == groupId && x.UserId == loggedInUser));
            }
        }

        public IEnumerable<UserModel> GetAllGroupUsers(int groupId)
        {
            using (var db = new ChatexdbContext())
            {
                IQueryable<User> groupUsers = db.GroupUser
                    .Where(gu => gu.GroupId == groupId)
                    .Select(gu => gu.User);


                //Select User from UserRole where the role id is in the collection:
                //  Select role id from GroupRole where the group id is matched
                IQueryable<User> userMatchingGroupRole = db.UserRole
                    .Where(ur => db.GroupRole
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
            using (var db = new ChatexdbContext())
            {
                List<UserModel> userMatchingAdmin = db.GroupUser
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

                //Select Group from GroupRole where the role id is in the collection:
                //  Select role id from UserRole where the user id is matched
                IQueryable<Group> groupsRole = context.GroupRole
                    .Where(gr => context.UserRole
                        .Where(ur => ur.UserId == userId)
                        .Select(ur => ur.Role.RoleId)
                        .Contains(gr.RoleId))
                    .Select(gr => gr.Group);

                return groupsUser.Union(groupsRole)
                    .Where(g => g.IsDeleted == false)
                    .Include(g => g.Channel)
                    .Select(g => GroupMapper.MapGroupEntityToModel(g))
                    .ToList();
            }
        }

        /// <summary>
        /// changing the group name of a spcific group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>

        public void UpdateGroup(int groupId, string groupName)
        {
            using (var db = new ChatexdbContext())
            {
                var entity = db.Group.FirstOrDefault(x => x.GroupId == groupId);
                entity.Name = groupName; 
                db.SaveChanges();             
            }

        }

        public void UpdateGroup(int groupId, string groupName, int callerId)
        {
            throw new NotImplementedException();
        }
    }
}
