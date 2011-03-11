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
using System.Transactions;
using System.ServiceModel;
using System.Messaging;
using CQRS.Messages;

namespace CQRS.Service
{
    public class AccountCommandService : IAccountCommandService
    {
        private const string IncreaseQueueName = @".\Private$\cqrs_increase";
        private const string DecreaseQueueName = @".\Private$\cqrs_decrease";

        // TODO 2d: Command doesn't update current state.
        public void Transfer(string fromAccount, string toAccount, decimal amount)
        {
            try
            {
                using (CAP_CQRSEntities entities = new CAP_CQRSEntities())
                {
                    using (var tx = new TransactionScope())
                    {
                        AccountBalance from = GetAccountEntity(fromAccount, entities);

                        if (from.Balance < amount)
                            throw new ApplicationException("Insufficient funds.");

                        // INSERT
                        entities.AddToTransfers(new Transfer()
                        {
                            From = fromAccount,
                            To = toAccount,
                            Amount = amount
                        });

                        // PUBLISH
                        TransferMessage message = new TransferMessage()
                        {
                            FromAccount = fromAccount,
                            ToAccount = toAccount,
                            Amount = amount
                        };
                        using (var queue = new MessageQueue(IncreaseQueueName))
                        {
                            queue.Send(message, MessageQueueTransactionType.Automatic);
                        }
                        using (var queue = new MessageQueue(DecreaseQueueName))
                        {
                            queue.Send(message, MessageQueueTransactionType.Automatic);
                        }

                        entities.SaveChanges();
                        tx.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        private static AccountBalance GetAccountEntity(string account, CAP_CQRSEntities entities)
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
    }
}
