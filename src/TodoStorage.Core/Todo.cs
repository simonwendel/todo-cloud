﻿/*
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

namespace TodoStorage.Core
{
    using System;

    /// <remarks>
    /// Will be outputted according to the following javascript validation spec:
    /// <code>
    /// const argsSpec = {
    /// id: {
    ///     isa: 'whole',
    ///     optional: true
    /// },
    /// title: {
    ///     isa: 'string',
    ///     optional: true
    /// },
    /// description: {
    ///     isa: 'string',
    ///     optional: true
    /// },
    /// created: {
    ///     isa: 'date',
    ///     optional: true
    /// },
    /// recurring: {
    ///     isa: 'whole',
    ///     optional: true
    /// },
    /// nextOccurrence: {
    ///     isa: 'date',
    ///     optional: true
    /// },
    /// color: {
    ///     isa: Color,
    ///     optional: true
    /// }
    /// };
    /// </code>
    /// </remarks>
    public class Todo
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? Created { get; set; }

        public int? Recurring { get; set; }

        public DateTime? NextOccurrence { get; set; }

        public Color Color { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherTodo = obj as Todo;
            return Id.Equals(otherTodo.Id)
                && Title.Equals(otherTodo.Title)
                && Description.Equals(otherTodo.Description)
                && Created.Equals(otherTodo.Created)
                && Recurring.Equals(otherTodo.Recurring)
                && NextOccurrence.Equals(otherTodo.NextOccurrence)
                && Color.Equals(otherTodo.Color);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 486187739) + Id.GetHashCode();
                hash = (hash * 486187739) + Title.GetHashCode();
                hash = (hash * 486187739) + Description.GetHashCode();
                hash = (hash * 486187739) + Created.GetHashCode();
                hash = (hash * 486187739) + Recurring.GetHashCode();
                hash = (hash * 486187739) + NextOccurrence.GetHashCode();
                hash = (hash * 486187739) + Color.GetHashCode();
                return hash;
            }
        }
    }
}
