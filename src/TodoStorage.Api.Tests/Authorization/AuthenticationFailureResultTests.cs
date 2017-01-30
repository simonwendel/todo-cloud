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
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;

    [TestFixture]
    internal class AuthenticationFailureResultTests
    {
        [Test]
        public void Ctor_GivenNullFailureReason_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new AuthenticationFailureResult(null, new HttpRequestMessage());

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullFailingReques_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new AuthenticationFailureResult("It just did!", null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public async Task ExecuteAsync_ReturnsUnathorizedResponse()
        {
            var request = new HttpRequestMessage();
            var sut = new AuthenticationFailureResult("It just did!", request);

            using (var response = await sut.ExecuteAsync(new CancellationToken()))
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                Assert.That(response.ReasonPhrase, Is.EqualTo("It just did!"));
                Assert.That(response.RequestMessage, Is.SameAs(request));
            } 
        }
    }
}
