// <copyright file="Changeset.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Versioning.Internal;

namespace ELTE.AEGIS.Versioning
{
    /// <summary>
    /// Defines behaviors for version control changesets.
    /// </summary>
    /// <typeparam name="TGeometryKey">The geometry key type of the changeset.</typeparam>
    /// <author>Máté Cserép</author>
    public class Changeset<TGeometryKey>
    {
        #region Protected fields

        /// <summary>
        /// Stores the added geometries indexed with their associated geometry keys in the current changeset.
        /// </summary>
        protected Dictionary<TGeometryKey, IGeometry> _added = new Dictionary<TGeometryKey, IGeometry>();

        /// <summary>
        /// Stores the removed geometries indexed with their associated geometry keys in the current changeset.
        /// </summary>
        protected Dictionary<TGeometryKey, IGeometry> _removed = new Dictionary<TGeometryKey, IGeometry>();

        /// <summary>
        /// Stores the geometry transformation operations indexed with the associated geometry keys in the current changeset.
        /// </summary>
        protected Dictionary<TGeometryKey, List<OperationConfiguration>> _forward = new Dictionary<TGeometryKey, List<OperationConfiguration>>();

        /// <summary>
        /// Stores the reversed geometry transformation operations indexed with the associated geometry keys in the current changeset.
        /// </summary>
        protected Dictionary<TGeometryKey, List<OperationConfiguration>> _reverse = new Dictionary<TGeometryKey, List<OperationConfiguration>>();

        /// <summary>
        /// Stores the type of the changeset.
        /// </summary>
        protected readonly ChangesetType _type;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the added geometries indexed with their associated geometry keys in the current changeset.
        /// </summary>
        public IDictionary<TGeometryKey, IGeometry> Added
        {
            get { return _added.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the removed geometries indexed with their associated geometry keys in the current changeset.
        /// </summary>
        public IDictionary<TGeometryKey, IGeometry> Removed
        {
            get { return _removed.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the geometry transformation operations indexed with the associated geometry keys in the current changeset.
        /// </summary>
        public IDictionary<TGeometryKey, IList<OperationConfiguration>> Modifications
        {
            get
            {
                return _forward.ToDictionary(pair => pair.Key, pair => pair.Value.AsReadOnly() as IList<OperationConfiguration>)
                               .AsReadOnly();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current changeset is empty.
        /// </summary>
        /// <value><c>true</c> if the changeset is considered to be empty; otherwise <c>false</c>.</value>
        public Boolean IsEmpty
        {
            get { return _added.Count == 0 && _removed.Count == 0 && _forward.Count == 0; }
        }

        /// <summary>
        /// Gets the type of the changeset.
        /// </summary>
        public ChangesetType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets whether the changeset supports forward change tracking
        /// </summary>
        /// <value><c>true</c> if supported; otherwise, <c>false</c>.</value>
        public Boolean IsForwardSupported { get { return Type.HasFlag(ChangesetType.Forward); } }

        /// <summary>
        /// Gets whether the changeset supports reverse change tracking.
        /// </summary>
        /// <value><c>true</c> if supported; otherwise, <c>false</c>.</value>
        public Boolean IsReverseSupported { get { return Type.HasFlag(ChangesetType.Reverse); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Changeset{TGeometryKey}"/> class.
        /// </summary>
        /// <param name="type">The type of the changeset used for change tracking.</param>
        public Changeset(ChangesetType type = ChangesetType.Dual)
        {
            _type = type;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a new geometry to the current changeset.
        /// </summary>
        /// <param name="key">The geometry key to register the geometry object with.</param>
        /// <param name="geometry">The geometry instance to add.</param>
        public void Add(TGeometryKey key, IGeometry geometry)
        {
            if (key.Equals(default(TGeometryKey)))
                throw new ArgumentNullException("key", "The geometry key cannot be null or the default value.");
            if (geometry == null && IsForwardSupported)
                throw new ArgumentNullException("geometry", "The geometry to add cannot be null.");

            _added.Add(key, IsForwardSupported ? geometry.Clone() as IGeometry : null);
        }

        /// <summary>
        /// Adds a geometry deletion to the current changeset.
        /// </summary>
        /// <param name="key">The geometry key to register the geometry object with.</param>
        /// <param name="geometry">The geometry instance to remove.</param>
        public void Remove(TGeometryKey key, IGeometry geometry)
        {
            if (key.Equals(default(TGeometryKey)))
                throw new ArgumentNullException("key", "The geometry key cannot be null or the default value.");
            if (geometry == null && IsReverseSupported)
                throw new ArgumentNullException("geometry", "The geometry to add cannot be null.");

            if (_added.ContainsKey(key))
                _added.Remove(key);
            else
                _removed.Add(key, IsReverseSupported ? geometry.Clone() as IGeometry : null);
            _forward.Remove(key);
        }

        /// <summary>
        /// Transforms a geometry with an operation.
        /// </summary>
        /// <param name="key">The geometry key to register the geometry object with.</param>
        /// <param name="geometry">The geometry instance to alter.</param>
        /// <param name="operation">The operation to execute on the given geometry.</param>
        public void Modify(TGeometryKey key, IGeometry geometry, Operation<IGeometry, IGeometry> operation)
        {
            if (key.Equals(default(TGeometryKey)))
                throw new ArgumentNullException("key", "The geometry key cannot be null or the default value.");
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry to add cannot be null.");
            if (operation == null)
                throw new ArgumentNullException("operation", "The operation cannot be null.");

            if (IsForwardSupported)
            {
                if (!_forward.ContainsKey(key))
                    _forward.Add(key, new List<OperationConfiguration>());
                _forward[key].Add(new OperationConfiguration(operation.Method, operation.Parameters));
            }

            if (IsReverseSupported)
            {
                if (!_reverse.ContainsKey(key))
                    _reverse.Add(key, new List<OperationConfiguration>());

                if (operation.Method.IsReversible)
                {
                    IOperation<IGeometry, IGeometry> reverseOperation = operation.GetReverseOperation();
                    _reverse[key].Add(new OperationConfiguration(reverseOperation.Method, reverseOperation.Parameters));
                }
                else
                {
                    IOperation<IGeometry, IGeometry> constantOperation = new ConstantGeometryOperation(geometry.Clone() as IGeometry);
                    _reverse[key].Add(new OperationConfiguration(constantOperation.Method, constantOperation.Parameters));
                }
            }
        }

        /// <summary>
        /// Merges the current instance with another <see cref="Changeset{TGeometryKey}"/> object.
        /// </summary>
        /// <param name="other">The <see cref="Changeset{TGeometryKey}"/> instance to merge with the current object.</param>
        /// <returns>The merged changeset.</returns>
        public Changeset<TGeometryKey> Merge(Changeset<TGeometryKey> other)
        {
            var merger = new Changeset<TGeometryKey>(_type)
            {
                _added   = new Dictionary<TGeometryKey, IGeometry>(_added),
                _removed = new Dictionary<TGeometryKey, IGeometry>(_removed),
                _forward = new Dictionary<TGeometryKey, List<OperationConfiguration>>(_forward),
                _reverse = new Dictionary<TGeometryKey, List<OperationConfiguration>>(_reverse),
            };
            merger.MergeInto(other);
            return merger;
        }

        /// <summary>
        /// Merges another <see cref="Changeset{TGeometryKey}"/> object into the current instance.
        /// </summary>
        /// <param name="other">The <see cref="Changeset{TGeometryKey}"/> instance to merge into the current object.</param>
        public void MergeInto(Changeset<TGeometryKey> other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The changeset to merge cannot be null.");

            if(Type != other.Type)
                throw new ArgumentException("Both changesets must have the same type.", "other");

            foreach (KeyValuePair<TGeometryKey, IGeometry> item in other._removed)
                Remove(item.Key, item.Value);

            foreach (KeyValuePair<TGeometryKey, IGeometry> item in other._added)
                Add(item.Key, item.Value);

            foreach (KeyValuePair<TGeometryKey, List<OperationConfiguration>> modification in other._forward)
            {
                if (!_forward.ContainsKey(modification.Key))
                    _forward.Add(modification.Key, new List<OperationConfiguration>());
                _forward[modification.Key].AddRange(modification.Value);
            }

            foreach (KeyValuePair<TGeometryKey, List<OperationConfiguration>> modification in other._reverse)
            {
                if (!_reverse.ContainsKey(modification.Key))
                    _reverse.Add(modification.Key, new List<OperationConfiguration>());
                _reverse[modification.Key].AddRange(modification.Value);
            }
        }

        /// <summary>
        /// Applies the changeset in forward direction on a given geometry set.
        /// </summary>
        /// <param name="inputState">The geometry set indexed with their associated geometry keys on which the changeset will be applied.</param>
        /// <returns>The result geometry set indexed with their associated geometry keys.</returns>
        public IDictionary<TGeometryKey, IGeometry> ApplyForward(IDictionary<TGeometryKey, IGeometry> inputState = null)
        {
            if(!IsForwardSupported)
                throw new NotSupportedException("Forward application is not supported by the changeset.");

            if (inputState == null)
                inputState = new Dictionary<TGeometryKey, IGeometry>(0);

            var outputState = new Dictionary<TGeometryKey, IGeometry>(inputState);
            foreach (KeyValuePair<TGeometryKey, IGeometry> item in _removed)
                outputState.Remove(item.Key);

            foreach (KeyValuePair<TGeometryKey, IGeometry> item in _added)
                outputState.Add(item.Key, item.Value);

            var engine = new OperationsEngine();
            foreach (KeyValuePair<TGeometryKey, List<OperationConfiguration>> modification in _forward)
                if (outputState.ContainsKey(modification.Key))
                    foreach (OperationConfiguration operation in modification.Value)
                        outputState[modification.Key] = engine.ExecuteOperation(operation, outputState[modification.Key]) as IGeometry;

            return outputState;
        }

        /// <summary>
        /// Applies the changeset in reverse direction on a given geometry set.
        /// </summary>
        /// <param name="inputState">The geometry set indexed with their associated geometry keys on which the changeset will be applied.</param>
        /// <returns>The result geometry set indexed with their associated geometry keys.</returns>
        public IDictionary<TGeometryKey, IGeometry> ApplyReverse(IDictionary<TGeometryKey, IGeometry> inputState = null)
        {
            if (!IsReverseSupported)
                throw new NotSupportedException("Reverse application is not supported by the changeset.");

            if (inputState == null)
                inputState = new Dictionary<TGeometryKey, IGeometry>(0);

            var outputState = new Dictionary<TGeometryKey, IGeometry>(inputState);
            foreach (KeyValuePair<TGeometryKey, IGeometry> item in _added)
                outputState.Remove(item.Key);

            foreach (KeyValuePair<TGeometryKey, IGeometry> item in _removed)
                outputState.Add(item.Key, item.Value);

            var engine = new OperationsEngine();
            foreach (KeyValuePair<TGeometryKey, List<OperationConfiguration>> modification in _reverse)
                if (outputState.ContainsKey(modification.Key))
                    foreach (OperationConfiguration operation in modification.Value)
                        outputState[modification.Key] = engine.ExecuteOperation(operation, outputState[modification.Key]) as IGeometry;

            return outputState;
        }

        #endregion
    }
}
