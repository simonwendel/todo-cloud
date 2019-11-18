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
    using AutoFixture;
    using NUnit.Framework;
    using SimonWendel.ObjectExtensions;
    using TodoStorage.Core;

    [TestFixture]
    internal class TodoTests
    {
        private Todo sut;

        private Todo sameProperties;

        private Todo[] someDiffering;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            var created = fixture.Create<DateTime>();

            fixture.Customize<Todo>(c => c
                .With(t => t.Id, 6)
                .With(t => t.Title, "T1")
                .With(t => t.Description, "D1")
                .With(t => t.Created, created)
                .With(t => t.Color, Color.Crimson)
                .With(t => t.Recurring, 4)
                .With(t => t.NextOccurrence, created.AddDays(3)));

            sut = fixture.Create<Todo>();
            sameProperties = fixture.Create<Todo>();

            someDiffering = new Todo[]
            {
                fixture.Create<Todo>().SetProperty(t => t.Id, 7),
                fixture.Create<Todo>().SetProperty(t => t.Title, "T2"),
                fixture.Create<Todo>().SetProperty(t => t.Description, "D2"),
                fixture.Create<Todo>().SetProperty(t => t.Created, created.AddDays(1)),
                fixture.Create<Todo>().SetProperty(t => t.Color, Color.DarkBlue),
                fixture.Create<Todo>().SetProperty(t => t.Recurring, 5),
                fixture.Create<Todo>().SetProperty(t => t.NextOccurrence, created.AddDays(4))
            };
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            Assert.That(sut.Equals(sameProperties), Is.True);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            foreach (var otherTodo in someDiffering)
            {
                Assert.That(sut.Equals(otherTodo), Is.False);
            }
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                hash = start;
                hash = (hash * multiplier) + sut.Id.GetHashCode();
                hash = (hash * multiplier) + sut.Title.GetHashCode();
                hash = (hash * multiplier) + sut.Description.GetHashCode();
                hash = (hash * multiplier) + sut.Created.GetHashCode();
                hash = (hash * multiplier) + sut.Recurring.GetHashCode();
                hash = (hash * multiplier) + sut.NextOccurrence.GetHashCode();
                hash = (hash * multiplier) + sut.Color.GetHashCode();
            }

            Assert.That(sut.GetHashCode(), Is.EqualTo(hash));
        }
    }
}
