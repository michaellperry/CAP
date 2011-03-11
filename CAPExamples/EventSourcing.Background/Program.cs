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

namespace EventSourcing.Background
{
    class Program
    {
        static void Main(string[] args)
        {
            SnapshotThread thread = new SnapshotThread();
            thread.Start();

            Console.WriteLine("Updating snapshots.");
            Console.ReadKey();

            thread.Stop();
        }
    }
}
