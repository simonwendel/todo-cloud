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

namespace TodoStorage.Domain
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
    }
}
