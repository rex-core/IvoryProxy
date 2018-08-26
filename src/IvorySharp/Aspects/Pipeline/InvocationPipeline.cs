using System;
using System.Collections.Generic;
using IvorySharp.Core;
using IvorySharp.Extensions;
using IvorySharp.Reflection;

namespace IvorySharp.Aspects.Pipeline
{
    /// <summary>
    /// Модель пайплайна выполнения метода.
    /// </summary>
    internal sealed class InvocationPipeline : InvocationPipelineBase
    {
        /// <inheritdoc />
        internal override bool CanReturnValue { get; }

        /// <inheritdoc />
        internal override Func<object> DefaultReturnValueGenerator { get; }

        /// <inheritdoc />
        public override object CurrentReturnValue
        {
            get => Invocation.ReturnValue;
            set => Invocation.ReturnValue = value;
        }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="InvocationPipeline"/>.
        /// </summary>
        public InvocationPipeline(
            IInvocationSignature signature,
            IReadOnlyCollection<MethodBoundaryAspect> boundaryAspects,
            MethodInterceptionAspect interceptionAspect) 
            : base(
                signature,
                boundaryAspects, 
                interceptionAspect)
        {
            CanReturnValue = !signature.Method.IsVoidReturn();
            DefaultReturnValueGenerator = CanReturnValue
                ? Expressions.CreateDefaultValueGenerator(signature.Method.ReturnType)
                : () => null;
        }

        /// <inheritdoc />
        public override void Return()
        {
            base.Return();
            
            if (!Context.Method.IsVoidReturn())
                CurrentReturnValue = Context.Method.ReturnType.GetDefaultValue();
        }

        /// <inheritdoc />
        public override void ReturnValue(object returnValue)
        {
            base.ReturnValue(returnValue);      
            CurrentReturnValue = returnValue;
        }
    }
}