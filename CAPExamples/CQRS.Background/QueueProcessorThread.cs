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

using System;
using System.Linq;
using System.Messaging;
using System.Threading;
using System.Transactions;
using CQRS.Messages;

namespace CQRS.Background
{
    public abstract class QueueProcessorThread
    {
        private const int TimeoutErrorCode = -2147467259;
        private static readonly XmlMessageFormatter Formatter = new XmlMessageFormatter(new Type[] { typeof(TransferMessage) });

        private string _threadName;
        private string _queueName;
        private Thread _thread;
        private ManualResetEvent _stopping;
        private bool _wroteEmptyMessage = false;

        public QueueProcessorThread(string threadName, string queueName)
        {
            _threadName = threadName;
            _queueName = queueName;
            _thread = new Thread(ThreadProc);
            _thread.Name = _threadName;
            _stopping = new ManualResetEvent(false);
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _stopping.Set();
            _thread.Join();
        }

        private void ThreadProc()
        {
            while (!_stopping.WaitOne(10))
            {
                try
                {
                    using (CAP_CQRSEntities entities = new CAP_CQRSEntities())
                    {
                        using (var tx = new TransactionScope())
                        {
                            using (MessageQueue queue = new MessageQueue(_queueName))
                            {
                                Message message = queue.Receive(TimeSpan.FromSeconds(1.0), MessageQueueTransactionType.Automatic);
                                _wroteEmptyMessage = false;

                                message.Formatter = Formatter;
                                TransferMessage transfer = (TransferMessage)message.Body;

                                HandleTransfer(entities, transfer);

                                entities.SaveChanges();
                                tx.Complete();
                            }
                        }
                    }
                }
                catch (MessageQueueException mqex)
                {
                    if (mqex.ErrorCode != TimeoutErrorCode)
                        Console.WriteLine(mqex.Message);
                    else
                    {
                        if (!_wroteEmptyMessage)
                            Console.WriteLine(String.Format("{0} queue empty.", _threadName));
                        _wroteEmptyMessage = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected static AccountBalance GetAccountEntity(string account, CAP_CQRSEntities entities)
        {
            IQueryable<AccountBalance> query =
                from b in entities.AccountBalances
                where b.Account == account
                select b;
            AccountBalance entity = query.FirstOrDefault();
            if (entity == null)
                throw new ApplicationException("No such account.");
            return entity;
        }

        protected abstract void HandleTransfer(CAP_CQRSEntities entities, TransferMessage transfer);
    }
}
