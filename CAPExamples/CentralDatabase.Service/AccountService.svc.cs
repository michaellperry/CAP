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
using System.ServiceModel;
using System.Transactions;

namespace CentralDatabase.Service
{
    public class AccountService : IAccountService
    {
        // TODO 1b: Query for the account balance.
        public decimal GetAccountBalance(string account)
        {
            try
            {
                using (CAP_CentralDatabaseEntities entities = new CAP_CentralDatabaseEntities())
                {
                    AccountBalance entity = GetAccountEntity(account, entities);
                    return entity.Balance;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        // TODO 1c: Transfer funds within a transaction.
        public void Transfer(string fromAccount, string toAccount, decimal amount)
        {
            try
            {
                using (CAP_CentralDatabaseEntities entities = new CAP_CentralDatabaseEntities())
                {
                    using (var tx = new TransactionScope())
                    {
                        AccountBalance from = GetAccountEntity(fromAccount, entities);
                        AccountBalance to = GetAccountEntity(toAccount, entities);

                        if (from.Balance < amount)
                            throw new ApplicationException("Insufficient funds.");

                        from.Balance -= amount;
                        to.Balance += amount;

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

        private static AccountBalance GetAccountEntity(string account, CAP_CentralDatabaseEntities entities)
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
