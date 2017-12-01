using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Business.Channels
{
    /// <summary>
    /// Instances of this class is used to manage the different actions in a channel that clients must be informed of. This includes sending, deleting, and editing messages, and deleting and renaming the channel. This logic has been extracted to a separate class, because both the MessageManager and ChannelManager must cooperate to collect informaiton about these events.
    /// </summary>
    class ChannelEventManager
    {
        private readonly ChannelLockStore lockStore;
        private readonly IMessageRepository messageRepository;

        public ChannelEventManager(IMessageRepository messageRepository)
        {
            lockStore = new ChannelLockStore();
            this.messageRepository = messageRepository;
        }

        public void LockChannelForWrite(int channelId)
        {
            ReaderWriterLock channelLock = lockStore.GetLockForChannel(channelId);
            channelLock.AcquireWriterLock(Timeout.Infinite);
        }

        public void UnlockChannelForWrite(int channelId)
        {
            Console.WriteLine($"New event in channel {channelId}");

            ReaderWriterLock channelLock = lockStore.GetLockForChannel(channelId);
            lockStore.BroadcastChannelEvent(channelId);
            channelLock.ReleaseWriterLock();
            lockStore.DropLockForChannel(channelId);//TODO: Should probably make another method for acquiring the lock without reference counting
            lockStore.DropLockForChannel(channelId);
        }

        public IEnumerable<ChannelEventModel> GetChannelEvents(int channelId, DateTime since, CancellationToken cancellationToken)
        {
            Console.WriteLine($"New client listens for events in channel {channelId}");

            ReaderWriterLock channelLock = lockStore.GetLockForChannel(channelId);
            Boolean keepGoing = true;

            try
            {
                while (keepGoing)
                {
                    channelLock.AcquireReaderLock(Timeout.Infinite);
                    IEnumerable<ChannelEventModel> newChannelEvents = getChannelEventsInternal(channelId, since);

                    if (newChannelEvents.Any())
                    {
                        Console.WriteLine($"Returning event information to client from channel {channelId}");
                        return newChannelEvents;
                    }

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

        private bool shouldKeepFetchingEvents(ManualResetEvent channelHandle, CancellationToken cancellationToken)
        {
            WaitHandle.WaitAny(new WaitHandle[] { channelHandle, cancellationToken.WaitHandle });
            return !cancellationToken.IsCancellationRequested;
        }

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
                Message = m
            }));

            messageEvents.AddRange(deletedMessages.Select(m => new ChannelEventModel()
            {
                Type = ChannelEventType.DeleteMessage,
                Message = m
            }));

            messageEvents.AddRange(editedMessages.Select(m => new ChannelEventModel()
            {
                Type = ChannelEventType.UpdateMessage,
                Message = m
            }));

            return messageEvents;
        }

        private List<ChannelEventModel> getChannelLevelEvents(int channelId, DateTime since)
        {
            //TODO
            return new List<ChannelEventModel>();
        }
    }
}
