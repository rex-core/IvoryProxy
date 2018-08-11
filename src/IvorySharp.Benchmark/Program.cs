﻿using BenchmarkDotNet.Running;

namespace IvorySharp.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher
                .FromAssembly(typeof(BenchmarkDispatch).Assembly)
                .Run(args);
        }
    }
}