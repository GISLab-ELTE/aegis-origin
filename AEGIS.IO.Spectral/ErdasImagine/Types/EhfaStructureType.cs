// <copyright file="EhfaStructureType.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.IO.ErdasImagine.Types
{
    /// <summary>
    /// Represents a structure type in the HFA file structure.
    /// </summary>
    /// <author>Tamas Nagy</author>
    public class EhfaStructureType : IEhfaObjectType
    {
        #region Private constants

        /// <summary>
        /// Object definition pattern. This field is constant.
        /// </summary>
        /// <remarks>
        /// Each object definition contain one or more item definition and it has a name.
        /// </remarks>
        private const String ObjectDefinitonPattern = @"\{(?<itemDefinitions>(.|\s)+)\}(?<name>[^\{\}]+),";

        /// <summary>
        /// The EHFA file type definition. This field is constant.
        /// </summary>
        /// <remarks>
        /// This is the type of the root entry.
        /// </remarks>
        private const String EhfaFileTypeDefinition = "{1:lversion,1:LfreeList,1:LrootEntryPtr,1:sentryHeaderLength,1:LdictionaryPtr,}Ehfa_File,";

        /// <summary>
        /// The EHFA header type definition. This field is constant.
        /// </summary>
        /// <remarks>
        /// This is the type of the file header.
        /// </remarks>
        private const String EhfaHeaderTypeDefinition = "{16:clabel,1:LheaderPtr,}Ehfa_HeaderTag,";

        /// <summary>
        /// The EDMS RLC parameters type definition. This field is constant.
        /// </summary>
        /// <remarks>
        /// This is the type of structure which holds the block compression info.
        /// </remarks>
        private const String EdmsRLCParamsTypeDefinition = "{1:Lmin,1:Lnumsegments,1:Ldataoffset,1:Cnumbitspervalue,}Edms_RLCParams,";

        #endregion

        #region Private static fields

        /// <summary>
        /// The regular expression of the object definition. This field is read-only.
        /// </summary>
        private static readonly Regex ObjectDefinitionExpression = new Regex(ObjectDefinitonPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EhfaStructureType" /> class.
        /// </summary>
        /// <param name="name">The structure type name.</param>
        /// <param name="dictionary">The type dictionary.</param>
        public EhfaStructureType(String name, HfaDictionary dictionary)
        {
            DataPlacement = DataPlacement.Internal;
            ItemName = name;
            Dictionary = dictionary;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EhfaStructureType" /> class.
        /// </summary>
        /// <param name="name">The structure type name.</param>
        /// <param name="dictionary">The type dictionary.</param>
        /// <param name="definitions">The item definitions.</param>
        private EhfaStructureType(String name, HfaDictionary dictionary, String definitions)
        {
            DataPlacement = DataPlacement.Internal;
            ItemName = name;
            Dictionary = dictionary;

            ParseItemDefinitions(definitions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EhfaStructureType" /> class.
        /// </summary>
        /// <param name="other">The other structure type.</param>
        /// <param name="dataPlacement">The data placement mode.</param>
        private EhfaStructureType(EhfaStructureType other, DataPlacement dataPlacement)
        {
            Items = other.Items;
            DataPlacement = dataPlacement;
            ItemName = other.ItemName;
            Dictionary = other.Dictionary;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the items this structure contains.
        /// </summary>
        /// <value>The items contained by this structure.</value>
        public OrderedDictionary Items { get; private set; }

        /// <summary>
        /// Gets the name of the structure.
        /// </summary>
        /// <value>
        /// The name of the structure.
        /// </value>
        public String ItemName { get; private set; }

        /// <summary>
        /// Gets the type this object type contains.
        /// </summary>
        /// <value>
        /// The type what this object type contains.
        /// </value>
        public ItemType ItemType { get { return ItemType.Structure; } }

        /// <summary>
        /// Gets the data placement mode.
        /// </summary>
        /// <value>The data placement mode.</value>
        public DataPlacement DataPlacement { get; private set; }

        /// <summary>
        /// Gets the global type dictionary for the structure.
        /// </summary>
        /// <value>The global type dictionary.</value>
        public HfaDictionary Dictionary { get; private set; }

        /// <summary>
        /// Gets the type of the header tag.
        /// </summary>
        /// <value>The type of the header tag.</value>
        public static EhfaStructureType EhfaHeaderTagType { get { return ParseObjectDefinition(EhfaHeaderTypeDefinition, 0, EhfaHeaderTypeDefinition.Length, null); } }

        /// <summary>
        /// Gets the type of the root file entry.
        /// </summary>
        /// <value>
        /// The type of the root file entry.
        /// </value>
        public static EhfaStructureType EhfaFileType { get { return ParseObjectDefinition(EhfaFileTypeDefinition, 0, EhfaFileTypeDefinition.Length, null); } }

        /// <summary>
        /// Gets the type of structure which holds the block compression info.
        /// </summary>
        /// <value>
        /// The type of structure which holds the block compression info.
        /// </value>
        public static EhfaStructureType EdmsRLCParamsType { get { return ParseObjectDefinition(EdmsRLCParamsTypeDefinition, 0, EdmsRLCParamsTypeDefinition.Length, null); } }
        #endregion

        #region Public static methods

        /// <summary>
        /// Returns a structure type from the object type definition.
        /// </summary>
        /// <param name="definition">The object type definition string.</param>
        /// <param name="dictionary">The type dictionary where the definition is located.</param>
        /// <returns>The produced structure type.</returns>
        /// <exception cref="ArgumentNullException">The definition is null.</exception>
        public static EhfaStructureType FromObjectDefinition(String definition, HfaDictionary dictionary)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition), "The definition is null.");

            return ParseObjectDefinition(definition, 0, definition.Length, dictionary);
        }

        /// <summary>
        /// Returns a structure type from the object type definition.
        /// </summary>
        /// <param name="definition">The object type definition string.</param>
        /// <param name="startIndex">The zero-based starting position of the definition within the string.</param>
        /// <param name="length">The length of the definition.</param>
        /// <param name="dictionary">The type dictionary where the definition is located.</param>
        /// <returns>The produced structure type.</returns>
        /// <exception cref="ArgumentNullException">The definition is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is greater than or equal to the length of the definition.
        /// </exception>
        public static EhfaStructureType FromObjectDefinition(String definition, Int32 startIndex, Int32 length, HfaDictionary dictionary)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition), "The definition is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "The starting index is less than 0.");
            if (startIndex >= definition.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "The starting index is greater than or equal to the length of the definition.");

            return ParseObjectDefinition(definition, startIndex, length, dictionary);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Parses the object definition.
        /// </summary>
        /// <param name="definition">The object definition string.</param>
        /// <param name="startIndex">The zero-based starting position of the definition within the string.</param>
        /// <param name="length">The length of the definition.</param>
        /// <param name="dictionary">The HFA dictionary where the definition is located.</param>
        /// <returns>The produced structure type.</returns>
        private static EhfaStructureType ParseObjectDefinition(String definition, Int32 startIndex, Int32 length, HfaDictionary dictionary)
        {
            Match objectMatch = ObjectDefinitionExpression.Match(definition, startIndex, length);

            if (!objectMatch.Success)
                return null;

            return new EhfaStructureType(objectMatch.Groups["name"].Value, dictionary, objectMatch.Groups["itemDefinitions"].Value);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Parses the item definitions.
        /// </summary>
        /// <param name="definitions">The item definitions.</param>
        private void ParseItemDefinitions(String definitions)
        {
            Items = new OrderedDictionary();

            Int32 index = 0;
            while (index < definitions.Length)
            {
                Int32 itemNumber = GetItemNumber(definitions, ref index);
                DataPlacement placement = GetDataPlacement(definitions, ref index);
                ItemType itemType = GetItemType(definitions, ref index);

                List<String> enumItems = null;
                IEhfaObjectType ehfaObjectType = null;

                if (itemType == ItemType.Enum)
                    enumItems = GetEnumItems(definitions, ref index);

                if (itemType == ItemType.PreviouslyDefined)
                    ehfaObjectType = GetPreviouslyDefinedObjectType(definitions, placement, ref index);

                if (itemType == ItemType.FollowedDefined)
                    ehfaObjectType = GetFollowingDefinedObjectType(definitions, ref index);

                String itemName = GetItemName(definitions, ref index);

                if (ehfaObjectType == null)
                {
                    if (itemType == ItemType.Enum)
                    {
                        ehfaObjectType = new EhfaPrimitiveType(itemName, itemNumber, placement, enumItems);
                    }
                    else
                    {
                        ehfaObjectType = new EhfaPrimitiveType(itemName, itemNumber, itemType, placement);
                    }
                }

                Items.Add(itemName, ehfaObjectType);
            }
        }

        /// <summary>
        /// Gets the type of a structure that is embedded in another structure.
        /// </summary>
        /// <param name="definitions">The item definitions.</param>
        /// <param name="index">The zero-based index of the type within the definition.</param>
        /// <returns>The structure type.</returns>
        private IEhfaObjectType GetFollowingDefinedObjectType(String definitions, ref Int32 index)
        {
            if (Dictionary == null)
                return null;

            Int32 length = HfaDictionary.GetObjectLength(definitions, 0) + 1;

            index += length;

            return FromObjectDefinition(definitions, index, length, Dictionary);
        }

        /// <summary>
        /// Gets the type of a structure that was previously defined.
        /// </summary>
        /// <param name="definitions">The item definitions.</param>
        /// <param name="index">The zero-based index of the type within the definition.</param>
        /// <param name="placement">The data placement mode.</param>
        /// <returns>The structure type.</returns>
        private IEhfaObjectType GetPreviouslyDefinedObjectType(String definitions, DataPlacement placement, ref Int32 index)
        {
            if (Dictionary == null)
                return null;

            Int32 endIndex = definitions.IndexOf(',', index);
            String typeName = definitions.Substring(index, endIndex - index);

            index = endIndex + 1;

            if (!Dictionary.ContainsKey(typeName))
                return null;

            return new EhfaStructureType(Dictionary[typeName], placement);
        }

        /// <summary>
        /// Returns the type of the item.
        /// </summary>
        /// <param name="definitions">The item definitions string.</param>
        /// <param name="index">The zero-based index of the type within the definition.</param>
        /// <returns>The item type.</returns>
        private ItemType GetItemType(String definitions, ref Int32 index)
        {
            Char itemTypeChar = definitions[index];

            ItemType itemType = ItemType.Undefined;
            if (Enum.IsDefined(typeof(ItemType), (Int32)itemTypeChar))
                itemType = (ItemType)itemTypeChar;

            index++;

            return itemType;
        }

        /// <summary>
        /// Returns the name of the item.
        /// </summary>
        /// <param name="definitions">The item definitions string.</param>
        /// <param name="index">The zero-based index of the name within the definition.</param>
        /// <returns>The item name.</returns>
        private String GetItemName(String definitions, ref Int32 index)
        {
            Int32 endIndex = definitions.IndexOf(',', index);
            String itemName = definitions.Substring(index, endIndex - index);

            index = endIndex + 1;

            return itemName;
        }

        /// <summary>
        /// Returns the item number.
        /// </summary>
        /// <param name="definition">The item definition string.</param>
        /// <param name="index">The zero-based index of the number within the definition.</param>
        /// <returns>The item number.</returns>
        private Int32 GetItemNumber(String definition, ref Int32 index)
        {
            Int32 endIndex = definition.IndexOf(':', index);
            Int32 itemNumber = Int32.Parse(definition.Substring(index, endIndex - index));

            index = endIndex + 1;

            return itemNumber;
        }

        /// <summary>
        /// Returns the data placement mode.
        /// </summary>
        /// <param name="definition">The item definitions.</param>
        /// <param name="index">The zero-based index of the placement within the definition.</param>
        /// <returns>The data placement mode.</returns>
        private DataPlacement GetDataPlacement(String definition, ref Int32 index)
        {
            DataPlacement placement = DataPlacement.Internal;

            if (definition[index] == 'p')
            {
                placement = DataPlacement.ExternalDynamicSize;
                index++;
            }
            else if (definition[index] == '*')
            {
                placement = DataPlacement.ExternalFixedSize;
                index++;
            }

            return placement;
        }

        /// <summary>
        /// Returns the list of enumerated items.
        /// </summary>
        /// <param name="definition">The item definitions.</param>
        /// <param name="index">The zero-based index of the placement within the definition.</param>
        /// <returns>The list of items.</returns>
        private List<String> GetEnumItems(String definition, ref Int32 index)
        {
            Int32 endIndex = definition.IndexOf(':', index);

            Int32 numberOfItems = Int32.Parse(definition.Substring(index, endIndex - index));

            List<String> enumItems = new List<String>(numberOfItems);

            index = endIndex + 1;

            for (Int32 itemIndex = 0; itemIndex < numberOfItems; itemIndex++)
            {
                endIndex = definition.IndexOf(',', index);
                String enumItem = definition.Substring(index, endIndex - index);
                enumItems.Add(enumItem);

                index = endIndex + 1;
            }

            return enumItems;
        }

        #endregion
    }
}
