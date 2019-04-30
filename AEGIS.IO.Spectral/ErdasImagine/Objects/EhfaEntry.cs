/// <copyright file="EhfaEntry.cs" company="Eötvös Loránd University (ELTE)">
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
using System.IO;
using ELTE.AEGIS.IO.ErdasImagine.Types;

namespace ELTE.AEGIS.IO.ErdasImagine.Objects
{
    /// <summary>
    /// Represents a HFA file entry. 
    /// </summary>
    public class EhfaEntry : EhfaStructure
    {
        #region Private fields

        /// <summary>
        /// The location of the next entry.
        /// </summary>
        public Int32? _nextLocation;

        /// <summary>
        /// The location of the previous entry.
        /// </summary>
        public Int32? _prevLocation;

        /// <summary>
        /// The location of the previous entry.
        /// </summary>
        public Int32? _parentLocation;

        /// <summary>
        /// The location of the child entry.
        /// </summary>
        public Int32? _childLocation;

        /// <summary>
        /// The location of the data.
        /// </summary>
        public Int32? _dataLocation;

        /// <summary>
        /// Gets size of the data.
        /// </summary>
        public Int32? _dataSize;

        /// <summary>
        /// The type of the data within the entry.
        /// </summary>
        public String _dataType;

        /// <summary>
        /// The name of the entry.
        /// </summary>
        public String _name;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EhfaEntry" /> class.
        /// </summary>
        /// <param name="structureType">The EHFA structure type.</param>
        /// <param name="dictionary">The HFA dictionary where the entry is located.</param>
        public EhfaEntry(EhfaStructureType structureType, HfaDictionary dictionary) : base(structureType)
        {
            Dictionary = dictionary;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the location of the next <see cref="EhfaEntry" />.
        /// </summary>
        /// <value>The location of the next <see cref="EhfaEntry" />.</value>
        public Int32 NextLocation { get { return (_nextLocation ?? (_nextLocation = GetValue<Int32>("next"))).Value; } }

        /// <summary>
        /// Gets the location of the previous <see cref="EhfaEntry" />.
        /// </summary>
        /// <value>The location of the previous <see cref="EhfaEntry" />.</value>
        public Int32 PrevLocation { get { return (_prevLocation ?? (_prevLocation = GetValue<Int32>("prev"))).Value; } }

        /// <summary>
        /// Gets the location of the previous <see cref="EhfaEntry" />.
        /// </summary>
        /// <value>The location of the previous <see cref="EhfaEntry" />.</value>
        public Int32 ParentLocation { get { return (_parentLocation ?? (_parentLocation = GetValue<Int32>("parent"))).Value; } }

        /// <summary>
        /// Gets the location of the child <see cref="EhfaEntry" />.
        /// </summary>
        /// <value>The location of the child <see cref="EhfaEntry" />.</value>
        public Int32 ChildLocation { get { return (_childLocation ?? (_childLocation = GetValue<Int32>("child"))).Value; } }

        /// <summary>
        /// Gets the location of the data.
        /// </summary>
        /// <value>The location of the data.</value>
        public Int32 DataLocation { get { return (_dataLocation ?? (_dataLocation = GetValue<Int32>("data"))).Value; } }

        /// <summary>
        /// Gets size of the data.
        /// </summary>
        /// <value>The size of the data.</value>
        public Int32 DataSize { get { return (_dataSize ?? (_dataSize = GetValue<Int32>("dataSize"))).Value; } }

        /// <summary>
        /// Gets the type of the data within the <see cref="EhfaEntry"/>.
        /// </summary>
        /// <value>The type of the data within the <see cref="EhfaEntry"/>.</value>
        public String DataType { get { return _dataType ?? (_dataType = GetValue<String>("type")); } }

        /// <summary>
        /// Gets the name of the <see cref="EhfaEntry"/>.
        /// </summary>
        /// <value>The name of the <see cref="EhfaEntry"/>.</value>
        public String Name { get { return _name ?? (_name = GetValue<String>("name")); } }

        /// <summary>
        /// Gets the next <see cref="EhfaEntry"/>.
        /// </summary>
        /// <value>The next <see cref="EhfaEntry"/>.</value>
        public EhfaEntry Next { get; private set; }

        /// <summary>
        /// Gets the previous <see cref="EhfaEntry"/>.
        /// </summary>
        /// <value>The previous <see cref="EhfaEntry"/>.</value>
        public EhfaEntry Previous { get; private set; }

        /// <summary>
        /// Gets the parent <see cref="EhfaEntry"/>.
        /// </summary>
        /// <value>The parent <see cref="EhfaEntry"/>.</value>
        public EhfaEntry Parent { get; private set; }

        /// <summary>
        /// Gets the child <see cref="EhfaEntry"/>.
        /// </summary>
        /// <value>The child <see cref="EhfaEntry"/>.</value>
        public EhfaEntry Child { get; private set; }

        /// <summary>
        /// Gets or sets the data of the <see cref="EhfaEntry"/>.
        /// </summary>
        /// <value>The data of the <see cref="EhfaEntry"/>.</value>
        public EhfaStructure Data { get; set; }

        /// <summary>
        /// Gets the type dictionary.
        /// </summary>
        /// <value>
        /// The type dictionary.
        /// </value>
        public HfaDictionary Dictionary { get; private set; }

        /// <summary>
        /// Gets the children of the <see cref="EhfaEntry" />.
        /// </summary>
        /// <value>The collection of children of the <see cref="EhfaEntry" />.</value>
        public IEnumerable<EhfaEntry> Children
        {
            get
            {
                EhfaEntry current = Child;
                while (current != null)
                {
                    yield return current;
                    current = current.Next;
                }
            }
        }
        #endregion

        #region IEhfaObject methods

        /// <summary>
        /// Reads the contents of the <see cref="EhfaEntry" /> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public override void Read(Stream stream)
        {
            base.Read(stream);

            if (NextLocation != 0)
            {
                Next = new EhfaEntry(Dictionary[Type.ItemName], Dictionary)
                {
                    Previous = this
                };

                stream.Seek(NextLocation, SeekOrigin.Begin);
                Next.Read(stream);
            }

            if (ChildLocation != 0)
            {
                Child = new EhfaEntry(Dictionary[Type.ItemName], Dictionary) {Parent = this};

                stream.Seek(ChildLocation, SeekOrigin.Begin);
                Child.Read(stream);
            }
        }

        /// <summary>
        /// Reads the data of the <see cref="EhfaEntry" />.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The structure within the entry.</returns>
        public EhfaStructure ReadData(Stream stream)
        {
            if (Data != null)
                return Data;

            if (DataLocation == 0)
                return null;

            Data = new EhfaStructure(Dictionary[DataType]);
            stream.Seek(DataLocation, SeekOrigin.Begin);

            Data.Read(stream);

            return Data;
        }

        #endregion
    }
}
