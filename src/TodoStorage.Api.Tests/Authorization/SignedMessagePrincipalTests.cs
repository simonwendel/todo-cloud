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

namespace TodoStorage.Api.Tests.Authorization
{
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Security;

    [TestFixture]
    internal class SignedMessagePrincipalTests
    {
        private IMessage message;

        private SignedMessagePrincipal sut;

        [SetUp]
        public void Setup()
        {
            message = Mock.Of<IMessage>();
            sut = new SignedMessagePrincipal(message);
        }

        [Test]
        public void Ctor_GivenNullMessage_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new SignedMessagePrincipal(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenMessage_SetsProperty()
        {
            Assert.That(sut.Message, Is.SameAs(message));
        }

        [Test]
        public void Identity_IsNull()
        {
            Assert.That(sut.Identity, Is.Null);
        }

        [Test]
        public void IsInRole_GivenRole_ReturnsFalse()
        {
            var fixture = new Fixture();
            var roles = fixture.CreateMany<string>(100);

            // maybe a weak test, but it does test something. testing that the 
            // method is always false regardless of input is an extensive problem.
            Assert.That(roles.All(r => sut.IsInRole(r)), Is.False);
        }
    }
}
