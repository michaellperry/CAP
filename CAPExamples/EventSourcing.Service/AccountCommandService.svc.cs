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

namespace EventSourcing.Service
{
    public class AccountCommandService : IAccountCommandService
    {
        // TODO 3d: Command is just an insert.
        public void Transfer(string fromAccount, string toAccount, decimal amount)
        {
            using (CAP_EventSourcingEntities entities = new CAP_EventSourcingEntities())
            {
                DateTime dateOfBusiness = AccountUtility.GetDateOfBusiness(entities);
                DateTime snapshotDate = dateOfBusiness - TimeSpan.FromDays(2.0);
                decimal accountBalance = AccountUtility.GetAccountBalance(fromAccount, snapshotDate, dateOfBusiness, entities);

                if (accountBalance < amount)
                    throw new ApplicationException("Insufficient funds");

                entities.AddToTransfers(new Transfer()
                {
                    From = fromAccount,
                    To = toAccount,
                    Amount = amount,
                    DateOfBusiness = dateOfBusiness
                });

                entities.SaveChanges();
            }
        }
    }
}
