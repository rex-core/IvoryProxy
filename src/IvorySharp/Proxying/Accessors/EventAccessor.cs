﻿using System.Reflection;
using System.Reflection.Emit;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace IvorySharp.Proxying.Accessors
{
    /// <summary>
    /// Вспомогательный класс для доступа к событиями.
    /// </summary>
    internal sealed class EventAccessor
    {
        public MethodInfo InterfaceAddMethod { get; }
        public MethodInfo InterfaceRemoveMethod { get; }
        public MethodInfo InterfaceRaiseMethod { get; }
        public MethodBuilder AddMethodBuilder { get; set; }
        public MethodBuilder RemoveMethodBuilder { get; set; }
        public MethodBuilder RaiseMethodBuilder { get; set; }

        public EventAccessor(MethodInfo interfaceAddMethod, MethodInfo interfaceRemoveMethod,
            MethodInfo interfaceRaiseMethod)
        {
            InterfaceAddMethod = interfaceAddMethod;
            InterfaceRemoveMethod = interfaceRemoveMethod;
            InterfaceRaiseMethod = interfaceRaiseMethod;
        }
    }
}