/*
 * Todo Storage for wifeys Todo app.
 * Copyright (C) 2016-2017  Simon Wendel
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

namespace TodoStorage.Core.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using TodoStorage.Core;

    [TestFixture]
    internal class ColorTests
    {
        [Test]
        public void Available_ShouldBeInitialized()
        {
            Color.Available.Should().HaveCount(6);
        }

        [Test]
        public void Pick_GivenNullColorValue_ThrowsException()
        {
            Action picking = () => Color.Pick(null);
            picking.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestCase("meh")]
        [TestCase("")]
        public void Pick_GivenInvalidValues_ThrowsException(string colorValue)
        {
            Action picking = () => Color.Pick(colorValue);
            picking.Should().ThrowExactly<IllegalValueException>();
        }

        [Test]
        public void Pick_GivenAnyAvailableValue_ReturnsColor()
        {
            foreach (var color in Color.Available)
            {
                Color.Pick(color.Value).Should().Be(color);
            }
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            var sut = Color.Crimson;
            sut.Equals(sut).Should().BeTrue();
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            var color1 = Color.Violet;
            var color2 = Color.Purple;
            var sut = Color.SeaGreen;

            sut.Equals(color1).Should().BeFalse();
            sut.Equals(color2).Should().BeFalse();
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            var sut = Color.DarkBlue;
            sut.Equals(null).Should().BeFalse();
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var sut = Color.Tomato;
            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                hash = start;
                hash = (hash * multiplier) + sut.Name.GetHashCode();
                hash = (hash * multiplier) + sut.Value.GetHashCode();
            }

            sut.GetHashCode().Should().Be(hash);
        }
    }
}
