using System;
/*  Copyright (C) 2015 Ben Aherne

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/
namespace RecipeBook
{
    /// <summary>
    /// Exception to be thrown in case of an invalid DocumentSection or TextSection enum value.
    /// </summary>
    public class NoSectionTypeException : Exception
    {
        public NoSectionTypeException()
            : base(String.Format("Invalid Section provided "))
        {

        }

        public NoSectionTypeException(string message)
            : base(message)
        {
        }

        public NoSectionTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
