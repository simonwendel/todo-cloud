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

namespace TodoStorage.Domain.Validation
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Convenience class for enforcing invariants and responding by throwing an adequate exception.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Will check for a <c>null</c> parameter and throwing <see cref="ArgumentNullException"/> if so.
        /// </summary>
        /// <param name="parameter">The object to check for null condition.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="parameter"/> is <c>null</c>.</exception>
        [DebuggerHidden]
        public static void NullParameter([ValidatedNotNull] object parameter)
        {
            NullParameter(parameter, parameterName: null);
        }

        /// <summary>
        /// Will check for a <c>null</c> parameter and throwing <see cref="ArgumentNullException"/> if so.
        /// </summary>
        /// <param name="parameter">The object to check for null condition.</param>
        /// <param name="parameterName">Name of the parameter to include in an exception, if thrown.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="parameter"/> is <c>null</c>.</exception>
        [DebuggerHidden]
        public static void NullParameter([ValidatedNotNull] object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Will check a string for <c>null</c> or <c>string.Empty</c> and responding by throwing an 
        /// adequate exception.
        /// </summary>
        /// <param name="parameter">Parameter to check for <c>null</c> or <c>string.Empty</c>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="parameter"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">When <paramref name="parameter"/> is <c>string.Empty</c>.</exception>
        [DebuggerHidden]
        public static void EmptyString([ValidatedNotNull] string parameter)
        {
            EmptyString(parameter, parameterName: null);
        }

        /// <summary>
        /// Will check a string for <c>null</c> or <c>string.Empty</c> and responding by throwing an
        /// adequate exception.
        /// </summary>
        /// <param name="parameter">Parameter to check for <c>null</c> or <c>string.Empty</c>.</param>
        /// <param name="parameterName">Name of the parameter to include in an exception, if thrown.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="parameter"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">When <paramref name="parameter"/> is <c>string.Empty</c>.</exception>
        [DebuggerHidden]
        public static void EmptyString([ValidatedNotNull] string parameter, string parameterName)
        {
            NullParameter(parameter, parameterName);

            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentException(message: null, paramName: parameterName);
            }
        }
    }
}
