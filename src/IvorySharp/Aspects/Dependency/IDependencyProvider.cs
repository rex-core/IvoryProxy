﻿using System;
using IvorySharp.Components;
using JetBrains.Annotations;

namespace IvorySharp.Aspects.Dependency
{
    /// <summary>
    /// Провайдер сервисов (сервис локатор).
    /// </summary>
    [PublicAPI]
    public interface IDependencyProvider : IComponent
    {
        /// <summary>
        /// Получает экземпляр сервиса по типу.
        /// </summary>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        /// <returns>Экземпляр сервиса.</returns>
         TService GetService<TService>() where TService : class;
        
        /// <summary>
        /// Получает экземпляр сервиса без обвязки (не проксированный).
        /// </summary>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        /// <returns>Экземпляр сервиса.</returns>
        TService GetTransparentService<TService>() where TService : class;
        
        /// <summary>
        /// Получает экземпляр именованного сервиса.
        /// </summary>
        /// <param name="key">Ключ сервиса.</param>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        /// <returns>Экземпляр сервиса.</returns>
        TService GetNamedService<TService>(string key) where TService : class;    
        
        /// <summary>
        /// Получает экземпляр именованного сервиса без обвязки (не проксированный).
        /// </summary>
        /// <param name="key">Ключ сервиса.</param>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        /// <returns>Экземпляр сервиса.</returns>
        TService GetTransparentNamedService<TService>(string key) where TService : class;       
        
        /// <summary>
        /// Получает экземпляр сервиса по типу.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <returns>Экземпляр сервиса.</returns>
        object GetService(Type serviceType);

        /// <summary>
        /// Получает экземпляр сервиса без обвязки (не проксированный).
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <returns>Экземпляр сервиса.</returns>
        object GetTransparentService(Type serviceType);
        
        /// <summary>
        /// Получает экземпляр именованного сервиса по типу.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <param name="key">Ключ сервиса.</param>
        /// <returns>Экземпляр сервиса.</returns>
        object GetNamedService(Type serviceType, string key);
        
        /// <summary>
        /// Получает экземпляр именованного сервиса по типу без обвязки (не проксированный).
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <param name="key">Ключ сервиса.</param>
        /// <returns>Экземпляр сервиса.</returns>
        object GetTransparentNamedService(Type serviceType, string key);
    }
}