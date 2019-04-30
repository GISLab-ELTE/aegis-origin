/// <copyright file="HfaDictionary.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Tamas Nagy</author>

using System;
using System.Collections.Generic;
using ELTE.AEGIS.IO.ErdasImagine.Types;

namespace ELTE.AEGIS.IO.ErdasImagine
{
    /// <summary>
    /// Represents a type dictionary for a HFA file.
    /// </summary>
    public class HfaDictionary : Dictionary<String, EhfaStructureType>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaDictionary" /> class.
        /// </summary>
        public HfaDictionary() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Parses a string to a HFA dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary string.</param>
        /// <returns>The produced dictionary.</returns>
        /// <exception cref="ArgumentNullException">The dictionary string is null.</exception>
        public static HfaDictionary Parse(String dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary), "The dictionary string is null.");

            HfaDictionary result = new HfaDictionary();

            Int32 startIndex = 0;
            while (startIndex < dictionary.Length && !dictionary[startIndex].Equals('.'))
            {
                Int32 length = GetObjectLength(dictionary, startIndex);
                EhfaStructureType structureType = EhfaStructureType.FromObjectDefinition(dictionary, startIndex, length, result);

                if (structureType == null)
                    continue;

                result.Add(structureType.ItemName, structureType);
                startIndex += length;
            }

            return result;
        }

        /// <summary>
        /// Gets the length of the object definition.
        /// </summary>
        /// <param name="definition">The item definition string.</param>
        /// <returns>The ending index of the object definition.</returns>
        /// <exception cref="InvalidDataException">The specified object definition data is invalid.</exception>
        public static Int32 GetObjectLength(String definition, Int32 startIndex)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition), "The definition is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "The starting index is less than 0.");
            if (startIndex >= definition.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "The starting index is greater than or equal to the length of the definition.");

            Int32 index = startIndex;
            Int32 braceDepth = 0;
            do
            {
                if (definition[index] == '{')
                    braceDepth++;
                else if (definition[index] == '}')
                    braceDepth--;

                index++;

            } while (braceDepth > 0 && index < definition.Length);

            if (braceDepth != 0)
                return 0;

            // after the closing brace, there will be the name of the object definition, and a closing comma
            return definition.IndexOf(',', index) - startIndex + 1;
        }

        #endregion
    }
}
