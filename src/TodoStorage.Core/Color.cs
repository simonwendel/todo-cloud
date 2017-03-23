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

namespace TodoStorage.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using SimonWendel.GuardStatements;

    public class Color
    {
        private readonly string name;

        private readonly string value;

        private Color(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public static Color Crimson { get; } = new Color("CRIMSON__COLOR", "crimson");

        public static Color DarkBlue { get; } = new Color("DARKBLUE__COLOR", "darkblue");

        public static Color Purple { get; } = new Color("PURPLE__COLOR", "purple");

        public static Color SeaGreen { get; } = new Color("SEAGREEN__COLOR", "seagreen");

        public static Color Tomato { get; } = new Color("TOMATO__COLOR", "tomato");

        public static Color Violet { get; } = new Color("VIOLET__COLOR", "violet");

        public static IEnumerable<Color> Available { get; } = new[] { Crimson, DarkBlue, Purple, SeaGreen, Tomato, Violet };

        public string Name => name;

        public string Value => value;

        public static Color Pick(string colorValue)
        {
            Guard.EnsureNotNull(colorValue, nameof(colorValue));

            var color = Available.FirstOrDefault(c => c.value.Equals(colorValue));
            if (color == null)
            {
                throw new IllegalValueException();
            }

            return color;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherColor = obj as Color;
            return Name.Equals(otherColor.Name)
                && Value.Equals(otherColor.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 486187739) + name.GetHashCode();
                hash = (hash * 486187739) + value.GetHashCode();
                return hash;
            }
        }
    }
}
