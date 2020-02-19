using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WikiCrawler.Workers
{
    abstract class Worker
    {
        public bool HasWork { get; private set; }

        public Worker()
        {
            HasWork = false;
        }

        protected virtual void Work(object args)
        {
            OnWorkDone();
        }

        public void StartWork(object args)
        {
            HasWork = true;
            Thread thread = new Thread(Work);
            thread.Start(args);
        }

        private void OnWorkDone()
        {
            HasWork = false;
        }
    }
}
