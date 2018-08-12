﻿using System;
using System.Linq;
using System.Text;
using IvorySharp.Aspects;
using IvorySharp.Core;
using Microsoft.Extensions.Caching.Memory;

namespace IvorySharp.Examples.Aspects
{
    /// <summary>
    /// Аспект кеширования.
    /// При вызове метода смотрит есть ли значение в кеше, и если есть - возвращает его вместо обращения к методу.
    /// </summary>
    public class CacheAspect : MethodInterceptionAspect
    {
        public static readonly MemoryCache MemoryCache = new MemoryCache(new MemoryCacheOptions());

        /// <inheritdoc />
        public override void OnInvoke(IInvocation invocation)
        {         
            var cacheKey = GetCacheKey(invocation);
            if (MemoryCache.TryGetValue(cacheKey, out var cached))
            {
                Console.WriteLine($"Return '{invocation.Method.ReturnType.Name}' with key '{cacheKey}' from cache");
                invocation.ReturnValue = cached;
            }
            else
            {
                invocation.Proceed();
                MemoryCache.Set(cacheKey, invocation.ReturnValue);
                Console.WriteLine($"Set '{invocation.Method.ReturnType.Name}' to cache with key '{cacheKey}'");
            }
        }
        
        private string GetCacheKey(IInvocationContext context)
        {
            var sb = new StringBuilder();

            sb.Append(context.DeclaringType.FullName);
            sb.Append(".");
            sb.Append(context.Method.Name);
            sb.Append("(");
            for (var i = 0; i < context.Arguments.Count; i++)
            {
                if (i > 0)
                    sb.Append(", ");

                sb.Append(context.Arguments.ElementAt(i));
            }
            sb.Append(')');

            return sb.ToString();
        }
    }
}