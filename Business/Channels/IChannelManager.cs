namespace Business.Channels
{
    public interface IChannelManager
    {
        /// <summary>
        /// Create a channel with the specified name in a group. The callerId must be the id of a group administrator in order to perform this operation.
        /// </summary>
        /// <param name="groupId">The id of the group to add the channel to</param>
        /// <param name="callerId">The id of the user who triggered this method</param>
        /// <param name="channelName">The name of the channel to create</param>
        /// <exception cref="ArgumentException"></exception>
        int CreateChannel(int groupId, int callerId, string channelName);

        /// <summary>
        /// Delete a channel from a group. The callerId must be the id of a group administrator in order to perform this operation.
        /// </summary>
        /// <param name="callerId">The id of the user who triggered this method</param>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <exception cref="ArgumentException"></exception>
        void DeleteChannel(int callerId, int channelId);

        /// <summary>
        /// Update a channel from a group. The callerId must be the id of a group administrator in order to perform this operation.
        /// </summary>
        /// <param name="callerId">The id of the user who triggered this method</param>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <param name="channelName">The new name of the group</param>
        /// <exception cref="ArgumentException"></exception>
        void UpdateChannel(int callerId, int channelId, string channelName);
    }
}
