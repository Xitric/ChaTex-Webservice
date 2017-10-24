using System;
using Business;
using Business.Models;
using DAL.Models;
using DAL.Mapper;

namespace DAL
{
    class GroupRepository : IGroupRepository
    {
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
