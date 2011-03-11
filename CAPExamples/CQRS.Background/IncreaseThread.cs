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
using CQRS.Messages;

namespace CQRS.Background
{
    // TODO 2e: Background thread updates current state.
    public class IncreaseThread : QueueProcessorThread
    {
        public IncreaseThread()
            : base("Increase", @".\Private$\cqrs_increase")
        {
        }

        protected override void HandleTransfer(CAP_CQRSEntities entities, TransferMessage transfer)
        {
            Console.WriteLine(String.Format("Increase {0}.", transfer.ToAccount));

            AccountBalance to = GetAccountEntity(transfer.ToAccount, entities);
            to.Balance += transfer.Amount;
        }
    }
}
