﻿using System;
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

        public void CreateGroup(GroupModel group)
        {
            var entity = GroupMapper.MapGroupModelToEntity(group);
            using (var db = new ChatexdbContext())
            {
                db.Group.Add(entity);
                db.SaveChanges();
            }
        }

        public bool DeleteGroup(long groupId)
        {
            throw new NotImplementedException();
        }
    }
}
