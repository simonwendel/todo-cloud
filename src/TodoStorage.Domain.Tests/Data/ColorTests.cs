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

namespace TodoStorage.Domain.Tests.Data
{
    using Domain.Data;
    using NUnit.Framework;

    [TestFixture]
    internal class ColorTests
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

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            var sut = new Color("Name", "Value");

            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            var color = new Color("Name", "Value");
            var sut = new Color("Name", "Value");

            Assert.That(sut.Equals(color), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            var color1 = new Color("Name2", "Value");
            var color2 = new Color("Name", "Value2");
            var sut = new Color("Name", "Value");

            Assert.That(sut.Equals(color1), Is.False);
            Assert.That(sut.Equals(color2), Is.False);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            var sut = new Color("Name", "Value");

            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var sut = new Color("A Color Name", "A Color Value");
            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                hash = start;
                hash = (hash * multiplier) + sut.ColorName.GetHashCode();
                hash = (hash * multiplier) + sut.ColorValue.GetHashCode();
            }

            Assert.That(sut.GetHashCode(), Is.EqualTo(hash));
        }
    }
}
