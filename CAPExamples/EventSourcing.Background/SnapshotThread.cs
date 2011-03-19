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
using System.Threading;

namespace EventSourcing.Background
{
    public class SnapshotThread
    {
        private Thread _thread;
        private ManualResetEvent _stopping;
        private bool _wroteEmptyMessage = false;

        public SnapshotThread()
        {
            _thread = new Thread(ThreadProc);
            _thread.Name = "Snapshot thread";
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
            while (!_stopping.WaitOne(1000))
            {
                try
                {
                    // TODO 3e: Background process inserts snapshots.
                    using (CAP_EventSourcingEntities entities = new CAP_EventSourcingEntities())
                    {
                        DateTime dateOfBusinees = entities.Current.Single().DateOfBusiness;
                        DateTime lastBalanceDate = dateOfBusinees - TimeSpan.FromDays(2.0);
                        DateTime thisBalanceDate = dateOfBusinees - TimeSpan.FromDays(1.0);

                        // Get a balance that needs to be calculated.
                        AccountBalance lastBalance = entities.AccountBalances
                            .Where(l =>
                                l.DateOfBusiness == lastBalanceDate &&
                                !entities.AccountBalances.Any(t =>
                                    t.Account == l.Account &&
                                    t.DateOfBusiness == thisBalanceDate)).FirstOrDefault();
                        if (lastBalance == null)
                        {
                            if (!_wroteEmptyMessage)
                                Console.WriteLine("Snapshots up to date.");
                            _wroteEmptyMessage = true;
                        }
                        else
                        {
                            Console.WriteLine(String.Format("Updating account balance for {0} on {1}.",
                                lastBalance.Account,
                                thisBalanceDate));
                            _wroteEmptyMessage = false;

                            // Calculate the balance.
                            var transfersFrom =
                                from t in entities.Transfers
                                where
                                    t.From == lastBalance.Account &&
                                    t.DateOfBusiness > lastBalanceDate &&
                                    t.DateOfBusiness <= thisBalanceDate
                                select (decimal?)t.Amount;
                            var transfersTo =
                                from t in entities.Transfers
                                where
                                    t.To == lastBalance.Account &&
                                    t.DateOfBusiness > lastBalanceDate &&
                                    t.DateOfBusiness <= thisBalanceDate
                                select (decimal?)t.Amount;
                            var thisBalance = lastBalance.Balance - (transfersFrom.Sum() ?? 0.0m) + (transfersTo.Sum() ?? 0.0m);

                            // Insert the new snapshot.
                            entities.AddToAccountBalances(new AccountBalance()
                            {
                                Account = lastBalance.Account,
                                DateOfBusiness = thisBalanceDate,
                                Balance = thisBalance
                            });

                            entities.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
