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

using System.ServiceModel;

namespace CQRS.Service
{
    // TODO 2a: Query interface.
    [ServiceContract]
    public interface IAccountQueryService
    {
        [OperationContract]
        decimal GetAccountBalance(string account);
    }
}
