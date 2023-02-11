// <copyright file="EhfaStructure.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ELTE.AEGIS.IO.ErdasImagine.Types;

namespace ELTE.AEGIS.IO.ErdasImagine.Objects
{
    /// <summary>
    /// Represents a structure in the HFA file.
    /// </summary>
    /// <author>Tamas Nagy</author>
    public class EhfaStructure : IEhfaObject
    {
        #region Private fields

        /// <summary>
        /// The type of the structure. This field is read-only.
        /// </summary>
        private readonly EhfaStructureType _type;

        /// <summary>
        /// The dictionary of internal objects. This field is read-only.
        /// </summary>
        private readonly Dictionary<String, IEhfaObject> _objects;

        /// <summary>
        /// The dictionary of internal object locations. This field is read-only.
        /// </summary>
        private readonly Dictionary<String, List<IEhfaObject>> _objectLocations;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EhfaStructure" /> class.
        /// </summary>
        /// <param name="type">The structure type.</param>
        /// <exception cref="ArgumentNullException">The type is null.</exception>
        public EhfaStructure(EhfaStructureType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "The type is null.");

            _objects = new Dictionary<String, IEhfaObject>();
            _objectLocations = new Dictionary<String, List<IEhfaObject>>();
            _type = type;
        }

        #endregion

        #region IEhfaObject properties

        /// <summary>
        /// Gets the type of the <see cref="IEhfaObject" />.
        /// </summary>
        /// <value>The type of the <see cref="IEhfaObject" />.</value>
        public IEhfaObjectType Type { get { return _type; } }

        #endregion

        #region IEhfaObject methods

        /// <summary>
        /// Reads the contents of the <see cref="EhfaStructure" /> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.ArgumentException">The stream is invalid.</exception>
        public virtual void Read(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "The stream is null.");

            try
            {
                foreach (DictionaryEntry structureItem in _type.Items)
                {
                    IEhfaObject value = null;
                    IEhfaObjectType item = (IEhfaObjectType)structureItem.Value;

                    if (item is EhfaPrimitiveType)
                    {
                        value = new EhfaObject(item as EhfaPrimitiveType);
                        value.Read(stream);
                        _objects.Add((String)structureItem.Key, value);
                    }
                    else if (item is EhfaStructureType)
                    {
                        if (item.DataPlacement == DataPlacement.Internal)
                        {
                            value = new EhfaStructure(item as EhfaStructureType);
                            value.Read(stream);
                            _objects.Add((String)structureItem.Key, value);
                        }

                        if (item.DataPlacement != DataPlacement.Internal)
                        {
                            Byte[] temp = new Byte[4];
                            stream.Read(temp, 0, 4);
                            Int32 objectRepeatCount = (Int32)EndianBitConverter.ToUInt32(temp, ByteOrder.LittleEndian);
                            stream.Read(temp, 0, 4);
                            UInt32 pointer = EndianBitConverter.ToUInt32(temp, ByteOrder.LittleEndian);

                            _objectLocations[(String)structureItem.Key] = new List<IEhfaObject>();

                            if (objectRepeatCount == 0)
                                continue;

                            while (objectRepeatCount-- > 0)
                            {
                                value = new EhfaStructure(item as EhfaStructureType);
                                value.Read(stream);
                                _objectLocations[(String)structureItem.Key].Add(value as EhfaStructure);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The stream is invalid.", nameof(stream), ex);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns an object value from the structure.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="name">The object name.</param>
        /// <returns>The value of the object.</returns>
        /// <remarks>
        /// The value can be a primitive type, an embedded structure, or a collection of these.
        /// </remarks>
        /// <exception cref="ArgumentException">The value with the specified name is not available.</exception>
        /// <exception cref="NotSupportedException">The type ({0}) if is not supported.</exception>
        public T GetValue<T>(String name)
        {
            if (!_objects.ContainsKey(name) && !_objectLocations.ContainsKey(name))
                throw new ArgumentException(String.Format("The value with the specified name ({0}) is not available.", name));

            if (_objects.ContainsKey(name))
            {
                if(_objects[name] is EhfaObject)
                    return ((EhfaObject) _objects[name]).GetValue<T>();
                else
                    return (T)_objects[name];
            }

            if (typeof(T) == typeof(EhfaStructure))
                return (T)_objectLocations[name].First();
            if (typeof(T).IsAssignableFrom(typeof(IEnumerable<EhfaStructure>)))
                return (T)_objectLocations[name].Select(item => (EhfaStructure)item);

            throw new NotSupportedException(String.Format("The type ({0}) if is not supported.", typeof(T).Name));
        }

        #endregion
    }
}
