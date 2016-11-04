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

namespace TodoStorage.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UpdateFailedException : Exception
    {
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1032:ImplementStandardExceptionConstructors",
            Justification = "Only the domain should ever be allowed to instantiate this class.")]
        internal UpdateFailedException()
        {
        }

        [SuppressMessage(
            "Microsoft.Design", 
            "CA1032:ImplementStandardExceptionConstructors",
            Justification = "Only the domain should ever be allowed to instantiate this class.")]
        internal UpdateFailedException(string message)
            : base(message)
        {
        }

        [SuppressMessage(
            "Microsoft.Design", 
            "CA1032:ImplementStandardExceptionConstructors",
            Justification = "Only the domain should ever be allowed to instantiate this class.")]
        internal UpdateFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        [SuppressMessage(
            "Microsoft.Design", 
            "CA1032:ImplementStandardExceptionConstructors",
            Justification = "Only the domain should ever be allowed to instantiate this class.")]
        internal UpdateFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
