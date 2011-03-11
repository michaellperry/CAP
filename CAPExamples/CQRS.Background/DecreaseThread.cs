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
    public class DecreaseThread : QueueProcessorThread
    {
        public DecreaseThread()
            : base("Decrease", @".\Private$\cqrs_decrease")
        {
        }

        protected override void HandleTransfer(CAP_CQRSEntities entities, TransferMessage transfer)
        {
            Console.WriteLine(String.Format("Decrease {0}.", transfer.FromAccount));

            AccountBalance from = GetAccountEntity(transfer.FromAccount, entities);
            from.Balance -= transfer.Amount;
        }
    }
}
