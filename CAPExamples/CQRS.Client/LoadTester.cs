/**********************************************************************
 * 
 * The CAP Theorem and its Implications
 * 
 * Michael L Perry
 * http://qedcode.com
 * 
 * Copyright 2010
 * Creative Commons Attribution 3.0
 * 
 **********************************************************************/

using System.Collections.Generic;
using System.Linq;
using UpdateControls;

namespace CQRS.Client
{
    public class LoadTester
    {
        private List<LoadTestThread> _threads = new List<LoadTestThread>();
        private Independent _indThreads = new Independent();

        public int ThreadCount
        {
            get { _indThreads.OnGet(); return _threads.Count; }
            set
            {
                _indThreads.OnSet();
                while (_threads.Count < value)
                {
                    LoadTestThread thread = new LoadTestThread();
                    _threads.Add(thread);
                    thread.Start();
                }
                while (_threads.Count > value)
                {
                    LoadTestThread lastThread = _threads.Last();
                    lastThread.Interrupt();
                    _threads.Remove(lastThread);
                }
            }
        }
    }
}
