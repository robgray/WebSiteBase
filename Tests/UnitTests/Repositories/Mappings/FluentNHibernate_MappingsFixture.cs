using System;
using System.Collections.Generic;
using Domain.Entities;
using FluentNHibernate.Testing;
using Infrastructure.Common;
using NHibernate;
using NUnit.Framework;
using Tests.Common;
using UnitTests.Repositories.Infrastructure;

namespace UnitTests.Repositories.Mappings
{
    [TestFixture]
    public class FluentNHibernate_MappingsFixture : InMemoryDatabase
    {
        [SetUp]
        public void Setup()
        {
            CreateNewSession();
        }
    }
}
