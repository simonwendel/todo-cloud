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

namespace TodoStorage.Api.Tests.Utilities
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    internal class FakeHttpActionContextTests
    {
        private FakeHttpActionContext sut;

        [SetUp]
        public void Setup()
        {
            sut = new FakeHttpActionContext();
        }

        [Test]
        public void Ctor_FakesMethod()
        {
            Assert.That(sut.Request.Method, Is.EqualTo(FakeHttpActionContext.FakeMethod));
        }

        [Test]
        public void Ctor_FakesUri()
        {
            Assert.That(sut.Request.RequestUri, Is.EqualTo(FakeHttpActionContext.FakeUri));
        }

        [Test]
        public async Task Ctor_FakesContent()
        {
            var contentTask = await sut.Request.Content.ReadAsStringAsync();

            Assert.That(contentTask, Is.EqualTo(FakeHttpActionContext.FakeContent));
        }

        [Test]
        public void Ctor_FakesAuthorizationHeader()
        {
            var scheme = sut.Request.Headers.Authorization.Scheme;
            var parameter = sut.Request.Headers.Authorization.Parameter;

            Assert.That(scheme, Is.EqualTo(FakeHttpActionContext.FakeScheme));
            Assert.That(parameter, Is.EqualTo(FakeHttpActionContext.FakeParameter));
        }
    }
}
