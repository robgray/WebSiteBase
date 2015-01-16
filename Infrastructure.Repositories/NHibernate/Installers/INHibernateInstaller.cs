// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0

using Castle.Transactions;
using FluentNHibernate.Cfg;
using NHibernate;

namespace Infrastructure.Repositories.NHibernate.Installers
{
    /// <summary>
    /// 	Register a bunch of these; one for each database.
    /// </summary>    
    public interface INHibernateInstaller
    {
        /// <summary>
        /// 	Is this the default session factory
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// 	Gets a session factory key. This key must be unique for the registered
        /// 	NHibernate installers.
        /// </summary>
        string SessionFactoryKey { get; }

        /// <summary>
        /// 	An interceptor to assign to the ISession being resolved through this session factory.
        /// </summary>
        Maybe<IInterceptor> Interceptor { get; }

        /// <summary>
        /// 	Build a fluent configuration.
        /// </summary>
        /// <returns>A non null fluent configuration instance that can
        /// 	be used to further configure NHibernate</returns>
        FluentConfiguration BuildFluent();

        /// <summary>
        /// 	Call-back to the installer, when the factory is registered
        /// 	and correctly set up in Windsor..
        /// </summary>
        /// <param name = "factory"></param>
        void Registered(ISessionFactory factory);
    }
}