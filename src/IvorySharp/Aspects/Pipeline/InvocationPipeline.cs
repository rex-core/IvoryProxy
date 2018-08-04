﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using IvorySharp.Core;
using IvorySharp.Exceptions;
using IvorySharp.Extensions;

namespace IvorySharp.Aspects.Pipeline
{
    /// <summary>
    /// Базовый пайплайн выполнения метода.
    /// </summary>
    internal abstract class InvocationPipeline : IInvocationPipeline
    {
        private static readonly object SyncRoot = new object();
        private readonly IDictionary<Type, object> _pipelineData;

        /// <summary>
        /// Текущий выполняемый аспект.
        /// </summary>
        internal MethodAspect CurrentExecutingAspect { get; set; }

        /// <summary>
        /// Модель вызова.
        /// </summary>
        internal IInvocation Invocation { get; }

        /// <inheritdoc />
        public InvocationContext Context { get; }

        /// <inheritdoc />
        public Exception CurrentException { get; set; }

        /// <inheritdoc />
        public FlowBehavior FlowBehavior { get; set; }

        /// <inheritdoc />
        public bool CanReturnResult { get; }

        /// <inheritdoc />
        public object AspectExecutionState {
            get => GetAspectState();
            set => SetAspectState(value);
        }

        /// <summary>
        /// Инициализирует экземпляр <see cref="InvocationPipeline"/>.
        /// </summary>
        /// <param name="invocation">Модель выполнения метода.</param>
        internal InvocationPipeline(IInvocation invocation)
        {
            _pipelineData = new ConcurrentDictionary<Type, object>();

            Context = invocation.Context;
            CanReturnResult = !Context.Method.IsVoidReturn();
            Invocation = invocation;
        }

        /// <inheritdoc />
        public void ReturnValue(object returnValue)
        {
            CurrentException = null;
            FlowBehavior = FlowBehavior.Return;
            
            if (Context.Method.IsVoidReturn())
            {
                throw new IvorySharpException(
                    $"Невозможно вернуть значение '{returnValue}' из аспекта '{CurrentExecutingAspect?.GetType().FullName}'. " +
                    $"Метод '{Context.Method.Name}' типа '{Context.DeclaringType.FullName}' " +
                    $"не имеет возвращаемого значения (void). " +
                    $"Для возврата используйте перегрузку '{nameof(ReturnValue)}' без параметров.");
            }

            if (returnValue == null)
            {
                Context.ReturnValue = Context.Method.ReturnType.GetDefaultValue();
            }
            else
            {
                if (!Context.Method.ReturnType.IsInstanceOfType(returnValue))
                {
                    throw new IvorySharpException(
                        $"Невозможно вернуть значение '{returnValue}' из аспекта '{CurrentExecutingAspect?.GetType().FullName}'. " +
                        $"Тип результата '{returnValue.GetType().FullName}' невозможно привести к возвращаемому типу '{Context.Method.ReturnType.FullName}' " +
                        $"метода '{Context.Method.Name}' сервиса '{Context.DeclaringType.FullName}'.");
                }

                Context.ReturnValue = returnValue;
            }
        }

        /// <inheritdoc />
        public void ReturnDefault()
        {
            FlowBehavior = FlowBehavior.Return;
            Context.ReturnValue = Context.Method.ReturnType.GetDefaultValue();
        }

        /// <inheritdoc />
        public void Return()
        {
            FlowBehavior = FlowBehavior.Return;

            // Если забыли указать результат, то ставим результат по умолчанию
            // Возможно лучше отдельно проверять этот кейс и кидать исключение
            if (Context.ReturnValue == null)
            {
                Context.ReturnValue = Context.Method.ReturnType.GetDefaultValue();
            }
        }

        /// <inheritdoc />
        public void ThrowException(Exception exception)
        {
            CurrentException = exception ?? throw new ArgumentNullException(nameof(exception));
            FlowBehavior = FlowBehavior.ThrowException;
        }

        /// <inheritdoc />
        public void RethrowException(Exception exception)
        {
            CurrentException = exception ?? throw new ArgumentNullException(nameof(exception));
            FlowBehavior = FlowBehavior.RethrowException;
        }

        private object GetAspectState()
        {
            lock (SyncRoot)
            {
                if (CurrentExecutingAspect == null)
                    return null;
                
                return _pipelineData.TryGetValue(CurrentExecutingAspect.GetType(), out var data)
                    ? data 
                    : null;
            }
        }

        private void SetAspectState(object newState)
        {
            lock (SyncRoot)
            {
                if (CurrentExecutingAspect == null)
                    return;

                _pipelineData[CurrentExecutingAspect.GetType()] = newState;
            }
        }
    }
}