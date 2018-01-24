using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Business.Channels
{
    /// <summary>
    /// Instances of this class is used to manage the different actions in a channel that clients must be informed of. This includes sending, deleting, and editing messages, and deleting and renaming the channel. This logic has been extracted to a separate class, because both the MessageManager and ChannelManager must cooperate to collect information about these events.
    /// </summary>
    class ChannelEventManager
    {
        private readonly ChannelLockStore lockStore;
        private readonly IMessageRepository messageRepository;
        private readonly IChannelRepository channelRepository;

        public ChannelEventManager(IMessageRepository messageRepository, IChannelRepository channelRepository)
        {
            lockStore = new ChannelLockStore();
            this.messageRepository = messageRepository;
            this.channelRepository = channelRepository;
        }

        /// <summary>
        /// Acquire a writer lock for the channel with the specified id. The lock must be released again by calling UnlockChannelForWrite.
        /// </summary>
        /// <param name="channelId">The id of the channel to acquire the lock for</param>
        public void LockChannelForWrite(int channelId)
        {
            ReaderWriterLock channelLock = lockStore.GetLockForChannel(channelId);
            channelLock.AcquireWriterLock(Timeout.Infinite);
        }

        /// <summary>
        /// Release the writer lock for the channel with the specified id. This will automatically inform all waiting threads in the channel that a new even might have occurred.
        /// </summary>
        /// <param name="channelId">The id of the channel to release the lock for</param>
        public void UnlockChannelForWrite(int channelId)
        {
            Console.WriteLine($"New event in channel {channelId}");

            ReaderWriterLock channelLock = lockStore.PeekLockForChannel(channelId);
            if (channelLock == null)
            {
                //The calling thread did certainly not own the write lock
                return;
            }

            lockStore.BroadcastChannelEvent(channelId);
            channelLock.ReleaseWriterLock();
            lockStore.DropLockForChannel(channelId);
        }

        /// <summary>
        /// Acquire all events in the speciied channel since a specific point in time. This method will block until either a new event is ready or the operation is cancelled. This method is entirely thread safe.
        /// </summary>
        /// <param name="channelId">The id of the channel to acquire events for</param>
        /// <param name="since">The time since which events should be acquired</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>A collection of events in the channel, or null if the operation was cancelled</returns>
        public IEnumerable<ChannelEventModel> GetChannelEvents(int channelId, DateTime since, CancellationToken cancellationToken)
        {
            Console.WriteLine($"New client listens for events in channel {channelId}");

            ReaderWriterLock channelLock = lockStore.GetLockForChannel(channelId);
            Boolean keepGoing = true;

            try
            {
                while (keepGoing)
                {
                    //By acquiring a read lock we ensure that events are not generated before we are ready to receive new ones
                    channelLock.AcquireReaderLock(Timeout.Infinite);
                    IEnumerable<ChannelEventModel> newChannelEvents = getChannelEventsInternal(channelId, since);

                    if (newChannelEvents.Any())
                    {
                        Console.WriteLine($"Returning event information to client from channel {channelId}");
                        return newChannelEvents;
                    }

                    //We acquire the wait handle before releasing the lock. This way, if a new event is generated after we release the lock, the acquired wait handle will be notified.
                    ManualResetEvent channelHandle = lockStore.GetCurrentWaitHandleForChannel(channelId);
                    channelLock.ReleaseReaderLock();

                    Console.WriteLine($"Client did not discover new events in channel {channelId}, waiting...");
                    keepGoing = shouldKeepFetchingEvents(channelHandle, cancellationToken);
                }
            }
            finally
            {
                channelLock.ReleaseLock();
                lockStore.DropLockForChannel(channelId);
            }

            Console.WriteLine($"Client gave up listening to channel {channelId}");
            return null;
        }

        /// <summary>
        /// Wait until either the ManualResetEvent is notified or the CancellationToken is cancelled.
        /// </summary>
        /// <returns>True if the ManualResetEvent was notified, false is the CancellationToken was cancelled</returns>
        private bool shouldKeepFetchingEvents(ManualResetEvent channelHandle, CancellationToken cancellationToken)
        {
            WaitHandle.WaitAny(new WaitHandle[] { channelHandle, cancellationToken.WaitHandle });
            return !cancellationToken.IsCancellationRequested;
        }

        /// <summary>
        /// Internal method for collecting message- and channel level events. This method is not threadsafe, and synchronization is expected to happen elsewhere.
        /// </summary>
        /// <param name="channelId">The id of the channel to get events for</param>
        /// <param name="since">The point in time since which events should be acquired</param>
        /// <returns></returns>
        private IEnumerable<ChannelEventModel> getChannelEventsInternal(int channelId, DateTime since)
        {
            List<ChannelEventModel> channelEvents = getMessageLevelEvents(channelId, since);
            channelEvents.AddRange(getChannelLevelEvents(channelId, since));

            return channelEvents;
        }

        private List<ChannelEventModel> getMessageLevelEvents(int channelId, DateTime since)
        {
            List<ChannelEventModel> messageEvents = new List<ChannelEventModel>();

            IEnumerable<MessageModel> newMessages = messageRepository.GetMessagesSince(channelId, since.ToUniversalTime());
            IEnumerable<MessageModel> deletedMessages = messageRepository.GetDeletedMessagesSince(channelId, since.ToUniversalTime());
            IEnumerable<MessageModel> editedMessages = messageRepository.GetEditedMessagesSince(channelId, since.ToUniversalTime());

            messageEvents.AddRange(newMessages.Select(m => new ChannelEventModel()
            {
                Type = ChannelEventType.NewMessage,
                TimeOfOccurrence = m.CreationTime,
                Message = m
            }));

            messageEvents.AddRange(deletedMessages.Select(m => new ChannelEventModel()
            {
                Type = ChannelEventType.DeleteMessage,
                TimeOfOccurrence = (DateTime)m.DeletionTime,
                Message = m
            }));

            messageEvents.AddRange(editedMessages.Select(m => new ChannelEventModel()
            {
                Type = ChannelEventType.UpdateMessage,
                TimeOfOccurrence = (DateTime)m.LastEdited,
                Message = m
            }));

            return messageEvents;
        }

        private List<ChannelEventModel> getChannelLevelEvents(int channelId, DateTime since)
        {
            List<ChannelEventModel> channelEvents = new List<ChannelEventModel>();

            IEnumerable<ChannelModel> renamedChannels = channelRepository.GetChannelRenamesSince(since.ToUniversalTime());
            IEnumerable<ChannelModel> deletedChannels = channelRepository.GetChannelDeletionsSince(since.ToUniversalTime());

            channelEvents.AddRange(renamedChannels.Select(c => new ChannelEventModel()
            {
                Type = ChannelEventType.RenameChannel,
                TimeOfOccurrence = (DateTime)channelRepository.GetChannelRenameDate(channelId),
                Channel = c
            }));

            channelEvents.AddRange(deletedChannels.Select(c => new ChannelEventModel()
            {
                Type = ChannelEventType.DeleteChannel,
                TimeOfOccurrence = (DateTime)channelRepository.GetChannelDeletionDate(channelId),
                Channel = c
            }));

            return channelEvents;
        }
    }
}
