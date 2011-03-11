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

namespace EventSourcing.Service
{
    public class AccountUtility
    {
        public static DateTime GetDateOfBusiness(CAP_EventSourcingEntities entities)
        {
            return entities.Current.Single().DateOfBusiness;
        }

        // TODO 3c: Query is more complex.
        public static decimal GetAccountBalance(string account, DateTime snapshotDate, DateTime closingDate, CAP_EventSourcingEntities entities)
        {
            AccountBalance snapshot = GetSnapshot(account, snapshotDate, entities);
            IQueryable<decimal?> transfersFrom =
                from t in entities.Transfers
                where
                    t.From == account &&
                    t.DateOfBusiness > snapshotDate &&
                    t.DateOfBusiness <= closingDate
                select (decimal?)t.Amount;
            IQueryable<decimal?> transfersTo =
                from t in entities.Transfers
                where
                    t.To == account &&
                    t.DateOfBusiness > snapshotDate &&
                    t.DateOfBusiness <= closingDate
                select (decimal?)t.Amount;
            return snapshot.Balance - (transfersFrom.Sum() ?? 0.0m) + (transfersTo.Sum() ?? 0.0m);
        }

        private static AccountBalance GetSnapshot(string account, DateTime snapshotDate, CAP_EventSourcingEntities entities)
        {
            IQueryable<AccountBalance> snapshots =
                from b in entities.AccountBalances
                where
                    b.Account == account &&
                    b.DateOfBusiness == snapshotDate
                select b;
            AccountBalance snapshot = snapshots.FirstOrDefault();
            if (snapshot == null)
                throw new ApplicationException("No such account.");
            return snapshot;
        }
    }
}