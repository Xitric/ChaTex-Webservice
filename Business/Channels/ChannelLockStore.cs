using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Business.Channels
{
    /// <summary>
    /// Instances of this class are used to create, maintain, and dispose locks for channels.
    /// </summary>
    class ChannelLockStore
    {
        private readonly IDictionary<int, ReaderWriterLock> channelLocks;
        private readonly IDictionary<int, ManualResetEvent> channelWaitHandles;
        private readonly IDictionary<int, int> channelLockReferences;

        public ChannelLockStore()
        {
            channelLocks = new Dictionary<int, ReaderWriterLock>();
            channelWaitHandles = new Dictionary<int, ManualResetEvent>();
            channelLockReferences = new Dictionary<int, int>();
        }

        /// <summary>
        /// Get the lock for the channel with the specified id. This method is entirely thread safe. Call the method DropLockForChannel() when the lock is no longer required - otherwise a memory leak will occur.
        /// </summary>
        /// <param name="channelId">The id of the channel to get the lock for</param>
        /// <returns>The lock for the channel with the specified id</returns>
        public ReaderWriterLock GetLockForChannel(int channelId)
        {
            lock (channelLocks)
            {
                if (channelLocks.TryGetValue(channelId, out ReaderWriterLock channelLock))
                {
                    //Lock existed, count another reference
                    channelLockReferences[channelId] = channelLockReferences[channelId] + 1;
                }
                else
                {
                    //Lock did not exist, create a new and mark one referece
                    channelLock = new ReaderWriterLock();
                    channelLocks[channelId] = channelLock;
                    channelWaitHandles[channelId] = new ManualResetEvent(false);
                    channelLockReferences[channelId] = 1;
                }

                return channelLock;
            }
        }

        /// <summary>
        /// Get the ManualResetEvent that is currently used for notifying threads of new events in the channel with the specified id. This method will not necessarily return the same wait handle every time it is called, as a new wait handle is provided every time an event happens. The purpose of this wait handle is that it allows the caller to wait for events in the channel that happened after the wait handle was acquired, but possible before it was waited on.
        /// This method should be called after the calling thread has acquired at least a read lock on the channel. If the method DropLockForChannel() is called, this wait handle might never be informed of a new channel event before a new event handle is provided. The channel lock should thus not be dropped before the wait handle has been put to use.
        /// </summary>
        /// <param name="channelId">The id of the channel to get the wait handle for</param>
        /// <returns>The wait handle for the channel with the specified id, or null if it has no wait handle</returns>
        public ManualResetEvent GetCurrentWaitHandleForChannel(int channelId)
        {
            lock (channelLocks)
            {
                channelWaitHandles.TryGetValue(channelId, out ManualResetEvent handle);
                return handle;
            }
        }

        /// <summary>
        /// Inform waiting threads that an event happened in the channel with the specified id. This method should only be called from a thread that has acquired a write lock on the channel.
        /// </summary>
        /// <param name="channelId">The id of the channel in which an event has happened</param>
        public void BroadcastChannelEvent(int channelId)
        {
            lock (channelLocks)
            {
                if (channelWaitHandles.TryGetValue(channelId, out ManualResetEvent handle))
                {
                    //A wait handle exists, meaning that there are threads to inform
                    handle.Set();

                    //The current wait handle is now useless, so create a new one
                    channelWaitHandles[channelId] = new ManualResetEvent(false);
                }
            }
        }

        /// <summary>
        /// Inform this lock store that the lock for the channel with the specified id is no longer used. When there are no more users of a lock, it will be disposed.
        /// </summary>
        /// <param name="channelId">The id of the channel for which the lock is no longer required</param>
        public void DropLockForChannel(int channelId)
        {
            lock (channelLocks)
            {
                if (channelLockReferences.TryGetValue(channelId, out int referenceCount))
                {
                    if (referenceCount == 1)
                    {
                        //We just removed the last reference, so we can safely dispose the lock
                        channelLockReferences.Remove(channelId);
                        channelWaitHandles.Remove(channelId);
                        channelLocks.Remove(channelId);
                    }
                    else
                    {
                        channelLockReferences[channelId] = referenceCount - 1;
                    }
                }
            }
        }
    }
}
