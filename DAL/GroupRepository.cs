using System;
using Business;
using Business.Models;
using DAL.Models;
using DAL.Mapper;
using System.Linq;
using System.Collections.Generic;

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

        public void RemoveUsersFromGroups(IEnumerable<GroupUserModel> groupUserModels)
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
                //var entities = groupRoleModels.Select(x => GroupRoleMapper.MapGroupUserModelToEntity(x));
                //db.GroupUser.RemoveRange(entities);
                //db.SaveChanges();
            }
        }

        public void RemoveRolesFromGroup(IEnumerable<GroupRoleModel> groupRoleModels)
        {

        }

        public void MarkUserAsAdministrator(GroupUserModel groupUserModel)
        {

        }

        public GroupUserModel GetGroupUser (int groupId, int loggedInUser)
        {
            using (var db = new ChatexdbContext())
            {
                return GroupUserMapper.MapGroupUserEntityToModel(
                                       db.GroupUser.FirstOrDefault(x => x.GroupId == groupId && x.UserId == loggedInUser));
            }
        }
    }
}
