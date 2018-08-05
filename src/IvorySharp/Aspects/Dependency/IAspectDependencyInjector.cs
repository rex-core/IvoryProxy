﻿using System;

namespace IvorySharp.Aspects.Dependency
{
    /// <summary>
    /// Компонент для внедрения зависимостей в аспекты.
    /// </summary>
    public interface IAspectDependencyInjector
    {
        /// <summary>
        /// Выполняет внедрение зависимостей в аспект.
        /// </summary>
        /// <param name="aspect">Модель аспекта.</param>
        void InjectPropertyDependencies(MethodAspect aspect);

        /// <summary>
        /// Получает информацию о зависимостях на типе аспекта.
        /// </summary>
        /// <param name="aspectType">Тип аспекта.</param>
        /// <returns>Информация о зависимостях аспекта.</returns>
        AspectPropertyDependency[] GetPropertyDependencies(Type aspectType);
    }
}