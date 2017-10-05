using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models.Threading
{
    public class MessageAwaitQueue
    {
        private ManualResetEvent eventLock = new ManualResetEvent(false);

        public void Await()
        {
            eventLock.Reset();
            eventLock.WaitOne();
        }

        public void Notify()
        {
            eventLock.Set();
        }
    }
}
