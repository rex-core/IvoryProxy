﻿using IvoryProxy.Core;

namespace IvoryProxy.Tests.Interceptors
{
    public class IncrementResultInterceptor : IInterceptor
    {
        /// <inheritdoc />
        public void Intercept(IMethodInvocation invocation)
        {
            invocation.Proceed();
            if (invocation.ReturnValue is int intResult)
            {
                invocation.TrySetReturnValue(intResult + 1);
            }
        }

        public bool CanIntercept(IMethodPreExecutionContext context)
        {
            return context.TargetMethod.ReturnType == typeof(int);
        }
    }
}