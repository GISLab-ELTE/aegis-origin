/// <copyright file="GeometryAlgorithmsTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Máté Cserép</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Geometry;
using NUnit.Framework;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="GeometryAlgorithms" /> class.
    /// </summary>
    [TestFixture]
    public class GeometryAlgorithmsTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="Validate" /> method.
        /// </summary>
        [Test]
        public void GeometryAlgorithmsValidateTest()
        {
            #region testing polygon validation with precision model application

            // polygon remains unchanged after validation
            PrecisionModel precisionModel = PrecisionModel.Default;
            GeometryFactory factory = new GeometryFactory(precisionModel, null);
            IPolygon polygon = factory.CreatePolygon(new Coordinate[]
            {
                new Coordinate(0.000, 0.000),
                new Coordinate(0.444, 0.000),
                new Coordinate(0.444, 0.040),
                new Coordinate(0.440, 0.040), 
            });

            Assert.IsTrue(polygon.IsValid);
            IGeometry result = GeometryAlgorithms.Validate(polygon);
            Assert.IsInstanceOf<IPolygon>(result);
            Assert.AreEqual(5, ((IPolygon) result).Shell.Count);


            // polygon becomes another polygon after validation
            precisionModel = new PrecisionModel(100);
            factory = new GeometryFactory(precisionModel, null);
            polygon = factory.CreatePolygon(new Coordinate[]
            {
                new Coordinate(0.000, 0.000),
                new Coordinate(0.444, 0.000),
                new Coordinate(0.444, 0.040),
                new Coordinate(0.440, 0.040),  
            });

            Assert.IsFalse(polygon.IsValid);
            result = GeometryAlgorithms.Validate(polygon);
            Assert.IsInstanceOf<IPolygon>(result);
            Assert.AreEqual(4, ((IPolygon)result).Shell.Count);


            // polygon becomes a line after validation
            precisionModel = new PrecisionModel(10);
            factory = new GeometryFactory(precisionModel, null);
            polygon = factory.CreatePolygon(new Coordinate[]
            {
                new Coordinate(0.000, 0.000),
                new Coordinate(0.444, 0.000),
                new Coordinate(0.444, 0.040),
                new Coordinate(0.440, 0.040), 
            });

            Assert.IsFalse(polygon.IsValid);
            result = GeometryAlgorithms.Validate(polygon);
            Assert.IsInstanceOf<ILineString>(result);
            Assert.AreEqual(2, ((ILineString)result).Count);


            // polygon becomes a point after validation
            precisionModel = new PrecisionModel(1);
            factory = new GeometryFactory(precisionModel, null);
            polygon = factory.CreatePolygon(new Coordinate[]
            {
                new Coordinate(0.000, 0.000),
                new Coordinate(0.444, 0.000),
                new Coordinate(0.444, 0.040),
                new Coordinate(0.440, 0.040),
            });

            Assert.IsFalse(polygon.IsValid);
            result = GeometryAlgorithms.Validate(polygon);
            Assert.IsInstanceOf<IPoint>(result);
            Assert.AreEqual(new Coordinate(0, 0), ((IPoint)result).Coordinate);

            #endregion
        }

        #endregion
    }
}
