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

    /// <summary>
    /// A *magical* attribute that can be applied to a parameter to mark it as being validated
    /// as not null, somewhere hence-forth. The Code Analysis engine will squash all subsequent
    /// emissions of CA1062 violation warnings. Use with caution! If you don't actually check
    /// for null somewhere you might trigger NullReferenceException:s being thrown... and, more
    /// importantly, you lied!
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
