// <copyright file="JsonCreationConverter.cs" company="Eötvös Loránd University (ELTE)">
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ELTE.AEGIS.IO.JSON
{
    /// <summary>
    /// Helper class to convert to and from JSON.
    /// </summary>
    /// <author>Norbert Vass</author>
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object.
        /// </summary>
        /// <param name="objectType">Type of object expected.</param>
        /// <param name="jObject">
        /// Contents of JSON object that will be deserialized.
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Loads object from file, converts it to desired type, populates it's properties, then returns with it.
        /// </summary>
        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer)
        {
            try
            {
                // Load JObject from stream
                JObject jObject = JObject.Load(reader);

                // Create target object based on JObject
                T target = Create(objectType, jObject);

                // Populate the object properties
                serializer.Populate(jObject.CreateReader(), target);

                return target;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid JSON file", ex);
            }
        }
    }
}
