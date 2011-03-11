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
    public class AccountQueryService : IAccountQueryService
    {
        public decimal GetAccountBalance(string account)
        {
            using (CAP_EventSourcingEntities entities = new CAP_EventSourcingEntities())
            {
                DateTime dateOfBusiness = AccountUtility.GetDateOfBusiness(entities);
                DateTime snapshotDate = dateOfBusiness - TimeSpan.FromDays(2.0);
                return AccountUtility.GetAccountBalance(account, snapshotDate, dateOfBusiness, entities);
            }
        }
    }
}
