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
using System.Messaging;

namespace CQRS.Background
{
    class Program
    {
        private const string IncreaseQueueName = @".\Private$\cqrs_increase";
        private const string DecreaseQueueName = @".\Private$\cqrs_decrease";

        static void Main(string[] args)
        {
            if (!MessageQueue.Exists(IncreaseQueueName))
                MessageQueue.Create(IncreaseQueueName, true);
            if (!MessageQueue.Exists(DecreaseQueueName))
                MessageQueue.Create(DecreaseQueueName, true);

            IncreaseThread increase = new IncreaseThread();
            increase.Start();
            DecreaseThread decrease = new DecreaseThread();
            decrease.Start();

            Console.WriteLine("Processing messages from the queue.");
            Console.ReadKey();

            increase.Stop();
            decrease.Stop();
        }
    }
}
