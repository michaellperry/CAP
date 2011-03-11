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
    // TODO 3b: Same command interface.
    [ServiceContract]
    public interface IAccountCommandService
    {
        [OperationContract]
        void Transfer(string fromAccount, string toAccount, decimal amount);
    }
}
