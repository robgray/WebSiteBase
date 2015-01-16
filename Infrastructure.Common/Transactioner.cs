using System;
using Castle.Transactions;

namespace Infrastructure.Common
{    
    // Use Transactioner if you're missing the .WithTransaction method in most Mammoth projects.
    // Castle Windsor's AutoTx Facility using Castle.Transactions means if we want transactions 
    // we just need to decorate a virutal method call with the Transaction attribute
    // and it'll figure the rest out. 
    
    // We could of course just create another method in the class under execution and decorate
    // that method with [Transaction]...
    
    public interface ITransactioner
    {
        void InAmbient(Action transactional);
        void InFork(Action transactional);
    }

    public class Transactioner : ITransactioner
    {        
        [Transaction] 
        public virtual void InAmbient(Action transactional)
        {
            transactional();
        }

        [Transaction(Fork = true)]
        public virtual void InFork(Action transactional)
        {
            transactional();
        }
    }
}