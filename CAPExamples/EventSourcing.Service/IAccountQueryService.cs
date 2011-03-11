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

namespace EventSourcing.Service
{
    // TODO 3a: Same query interface.
    [ServiceContract]
    public interface IAccountQueryService
    {
        [OperationContract]
        decimal GetAccountBalance(string account);
    }
}
