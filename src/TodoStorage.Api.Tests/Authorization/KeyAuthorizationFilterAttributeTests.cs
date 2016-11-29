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
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Security;

    [TestFixture]
    internal class KeyAuthorizationFilterAttributeTests
    {
        private Mock<IHashingKeyFactory> keyFactory;

        private Mock<IMessageExtractor> messageExtractor;

        [SetUp]
        public void Setup()
        {
            keyFactory = new Mock<IHashingKeyFactory>();
            messageExtractor = new Mock<IMessageExtractor>();
        }

        [Test]
        public void Ctor_GivenNullHashingKeyFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new KeyAuthorizationFilterAttribute(null, messageExtractor.Object);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullMessageExtractor_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new KeyAuthorizationFilterAttribute(keyFactory.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }
    }
}
