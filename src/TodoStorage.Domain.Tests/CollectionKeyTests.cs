﻿/*
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

namespace TodoStorage.Domain.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class CollectionKeyTests
    {
        [Test]
        public void Ctor_GivenEmptyGuid_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new CollectionKey(Guid.Empty);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenGuid_SetsIdentifier()
        {
            var identifier = Guid.NewGuid();
            var sut = new CollectionKey(identifier);

            Assert.That(sut.Identifier, Is.EqualTo(identifier));
        }
    }
}