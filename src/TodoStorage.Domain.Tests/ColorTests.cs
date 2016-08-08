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

namespace TodoStorage.Domain.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void Ctor_GivenNameAndValue_SetsProperties()
        {
            var sut = new Color("some name", "some value");

            Assert.That(sut.ColorName, Is.EqualTo("some name"));
            Assert.That(sut.ColorValue, Is.EqualTo("some value"));
        }

        [Test]
        public void Ctor_GivenNullColorName_ThrowsException()
        {
            TestDelegate constructorCall = 
                () => new Color(null, "some value");

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullColorValue_ThrowsException()
        {
            TestDelegate constructorCall = 
                () => new Color("some name", null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenEmptyColorName_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Color(string.Empty, "some value");

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenEmptyColorValue_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Color("some name", string.Empty);

            Assert.That(constructorCall, Throws.ArgumentException);
        }
    }
}
