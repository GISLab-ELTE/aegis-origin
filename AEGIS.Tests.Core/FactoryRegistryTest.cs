// <copyright file="FactoryRegistryTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Metadata;
using Moq;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests
{
    /// <summary>
    /// Test fixture for the <see cref="FactoryRegistry" /> class.
    /// </summary>
    [TestFixture]
    public class FactoryRegistryTest
    {
        /// <summary>
        /// Mock of the reference system.
        /// </summary>
        private Mock<IReferenceSystem> _referenceSystemMock;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _referenceSystemMock = new Mock<IReferenceSystem>();
        }

        /// <summary>
        /// Tests all methods.
        /// </summary>
        [Test]
        public void FactoryRegistryMethodsTest()
        {
            Assert.IsTrue(FactoryRegistry.AutoLocation);
            FactoryRegistry.AutoLocation = false;
            Assert.IsFalse(FactoryRegistry.AutoLocation);

            FactoryRegistry.AutoLocation = true;
            Assert.IsTrue(FactoryRegistry.AutoLocation);

            // default query

            IMetadataFactory metadataFactory = FactoryRegistry.GetFactory<IMetadataFactory>();
            Assert.IsNotNull(metadataFactory);
            Assert.IsInstanceOf<MetadataFactory>(metadataFactory);

            IGeometryFactory geometryFactory = FactoryRegistry.GetFactory<IGeometryFactory>();
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);

            geometryFactory = FactoryRegistry.GetFactoryFor<IGeometry>() as IGeometryFactory;
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);


            // parameterized query

            geometryFactory = FactoryRegistry.GetFactory<IGeometryFactory>(PrecisionModel.Default, _referenceSystemMock.Object);
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
            Assert.AreEqual(PrecisionModel.Default, geometryFactory.PrecisionModel);
            Assert.AreEqual(_referenceSystemMock.Object, geometryFactory.ReferenceSystem);

            geometryFactory = FactoryRegistry.GetFactoryFor<IGeometry>(PrecisionModel.Default, _referenceSystemMock.Object) as IGeometryFactory;
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
            Assert.AreEqual(PrecisionModel.Default, geometryFactory.PrecisionModel);
            Assert.AreEqual(_referenceSystemMock.Object, geometryFactory.ReferenceSystem);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => FactoryRegistry.GetFactory(null));
        }
    }
}
