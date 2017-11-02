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
