/*
 * Todo Storage for wifeys Todo app.
 * Copyright (C) 2016  Simon Wendel
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace TodoStorage.Api.Tests.Configuration
{
    using System;
    using Ninject;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Configuration;
    using TodoStorage.Core;
    using TodoStorage.Security;

    [TestFixture]
    internal class NinjectConfigTests
    {
        private static Type[] publicInterfaces =
        {
            // from TodoStorage.Domain
            typeof(IAccessControlRepository),
            typeof(ITodoList),
            typeof(ITodoListFactory),
            typeof(ITodoRepository),

            // from TodoStorage.Security
            typeof(IAuthenticationRepository),
            typeof(IHashingKey),
            typeof(IHashingKeyFactory),
            typeof(IMessage),
            typeof(IMessageFactory)
        };

        private static Type[] apiInternalInterfaces =
        {
            // from TodoStorage.Api
            typeof(IAuthenticator),
            typeof(IChallenger),
            typeof(IMessageExtractor)
        };

        private IKernel kernel;

        [SetUp]
        public void Setup()
        {
            kernel = NinjectConfig.CreateKernel();
        }

        [TestCaseSource("publicInterfaces")]
        [TestCaseSource("apiInternalInterfaces")]
        public void CreateKernel_RegistersPublicInterfaces(Type service)
        {
            Assert.That(kernel.CanResolve(service), Is.True);
        }
    }
}
