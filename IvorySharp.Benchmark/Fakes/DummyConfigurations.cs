﻿using System;
using IvorySharp.Aspects.Configuration;
using IServiceProvider = IvorySharp.Aspects.Dependency.IServiceProvider;

namespace IvorySharp.Benchmark.Fakes
{
    internal class DummyConfigurations : IAspectsWeavingSettings
    {
        public Type ExplicitWeavingAttributeType { get; set; } = null;
        public IServiceProvider ServiceProvider { get; set; } = null;
    }
}