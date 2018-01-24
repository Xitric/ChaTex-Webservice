using Business.Errors;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    /// <summary>
    /// Abstract base class for managers that require the ability to authenticate whether a user has access to certain resources.
    /// </summary>
    abstract class AuthenticatedManager
    {
        private IGroupRepository groupRepository;

        public AuthenticatedManager(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        /// <summary>
        /// Throw an InvalidArgumentException if the user with the specified id is not an administrator for the group with the specified id.
        /// </summary>
        /// <param name="groupId">The id of the group to test for</param>
        /// <param name="callerId">The id of the user to test for</param>
        /// <exception cref="InvalidArgumentException">If the user is not an administrator of the specified group</exception>
        protected void throwIfNotAdministrator(int groupId, int callerId)
        {
            GroupMembershipDetails membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(groupId, callerId);

            if (!membershipDetails.IsAdministrator)
            {
                throw new InvalidArgumentException("The user must be an administrator of the group to perform this action", ParamNameType.CallerId);
            }
        }

        /// <summary>
        /// Throw an InvalidArgumentException if the user with the specified id is not a member of the group with the specified id.
        /// </summary>
        /// <param name="groupId">The id of the group to test for</param>
        /// <param name="callerId">The id of the user to test for</param>
        /// <exception cref="InvalidArgumentException">If the user is not a member of the specified group</exception>
        protected void throwIfNotMember(int groupId, int callerId)
        {
            GroupMembershipDetails membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(groupId, callerId);

            if (!membershipDetails.IsMember)
            {
                throw new InvalidArgumentException("The user is not a member of the specified group", ParamNameType.CallerId);
            }
        }

        /// <summary>
        /// Throw an InvalidArgumentException if the user with the specified id does not have access to the channel with the specified id.
        /// </summary>
        /// <param name="channelId">The id of the channel to test for</param>
        /// <param name="userId">The id of the user to test for</param>
        protected void throwIfNoAccessToChannel(int channelId, int userId)
        {
            bool hasAccess = groupRepository.GetGroupsForUser(userId)
                .Where(g => g.Channels.Any(c => c.Id == channelId))
                .Any();

            if (! hasAccess)
            {
                throw new InvalidArgumentException("User does not have access to the specified channel", ParamNameType.CallerId);
            }
        }
    }
}
