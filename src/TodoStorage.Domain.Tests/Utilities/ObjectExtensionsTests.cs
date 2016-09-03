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

namespace TodoStorage.Domain.Tests.Utilities
{
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    internal class ObjectExtensionsTests
    {
        [Test]
        public void SetProperty_GivenNullExpression_ThrowsException()
        {
            var sut = new TestDataObject();

            Assert.That(() => sut.SetProperty(null, 2), Throws.ArgumentNullException);
        }

        [Test]
        public void SetProperty_GivenNonMemberExpression_ThrowsException()
        {
            var sut = new TestDataObject();

            Assert.That(() => sut.SetProperty(s => s.DoStuff(), 2), Throws.InvalidOperationException);
        }

        [Test]
        public void SetProperty_GivenFieldExpression_ThrowsException()
        {
            var sut = new TestDataObject();

            Assert.That(() => sut.SetProperty(s => s.Field, 2), Throws.InvalidOperationException);
        }

        [Test]
        public void SetProperty_GivenPropertyExpression_SetsProperty()
        {
            var sut = new TestDataObject();

            sut.SetProperty(s => s.Property, 2);

            Assert.That(sut.Property, Is.EqualTo(2));
        }

        [Test]
        public void SetProperty_GivenPropertyExpression_ReturnsTarget()
        {
            var sut = new TestDataObject();

            var returnedObject = sut.SetProperty(s => s.Property, 2);

            Assert.That(returnedObject, Is.SameAs(sut));
        }

        public class TestDataObject
        {
            [SuppressMessage(
                "StyleCop.CSharp.MaintainabilityRules", 
                "SA1401:FieldsMustBePrivate", 
                Justification = "Proving a point here.")]
            public int Field = 1;

            public int Property { get; set; } = 10;

            public int DoStuff()
            {
                return 5;
            }
        }
    }
}
