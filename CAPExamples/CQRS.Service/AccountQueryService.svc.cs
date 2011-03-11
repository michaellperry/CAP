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

namespace CQRS.Service
{
    public class AccountQueryService : IAccountQueryService
    {
        // TODO 2c: Query is the same as before.
        public decimal GetAccountBalance(string account)
        {
            try
            {
                using (CAP_CQRSEntities entities = new CAP_CQRSEntities())
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
