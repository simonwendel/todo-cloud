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
    using SimonWendel.GuardStatements;

    public class Color
    {
        private readonly string colorName;

        private readonly string colorValue;

        public Color(string colorName, string colorValue)
        {
            Guard.EnsureNonempty(colorName, nameof(colorName));
            Guard.EnsureNonempty(colorValue, nameof(colorValue));

            this.colorName = colorName;
            this.colorValue = colorValue;
        }

        public string ColorName => colorName;

        public string ColorValue => colorValue;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherColor = obj as Color;
            return ColorName.Equals(otherColor.ColorName)
                && ColorValue.Equals(otherColor.ColorValue);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 486187739) + colorName.GetHashCode();
                hash = (hash * 486187739) + colorValue.GetHashCode();
                return hash;
            }
        }
    }
}
