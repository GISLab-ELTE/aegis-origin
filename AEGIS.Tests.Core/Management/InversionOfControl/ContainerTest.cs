/// <copyright file="ContainerTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Management.InversionOfControl;
using ELTE.AEGIS.Metadata;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Management.InversionOfControl
{
    [TestFixture]
    public class ContainerTest
    {
        private Container _container;

        [SetUp]
        public void SetUp()
        {
            _container = new Container();
        }


        [TestCase]
        public void ContainerRegistrationTest()
        {
            _container.Register<IMetadataFactory, MetadataFactory>();
            _container.Register<IGeometryFactory, GeometryFactory>();

            Assert.IsTrue(_container.IsRegistered<IMetadataFactory>() && _container.IsRegistered<IGeometryFactory>());

            try
            {
                _container.Register<IGeometryFactory, GeometryFactory>();
                Assert.Fail();
            }
            catch (InvalidOperationException) { }

            _container.Register<IGeometryFactory, GeometryFactory>(true);
            Assert.IsTrue(_container.IsRegistered<IGeometryFactory>());

            Assert.IsTrue(_container.Unregister<IGeometryFactory>());
            Assert.IsFalse(_container.IsRegistered<IGeometryFactory>());
            Assert.IsFalse(_container.Unregister<IGeometryFactory>());
        }

        [TestCase]
        public void ContainerResolveTest()
        {
            // test case 1: resolve unregistered service
            try
            {
                _container.Resolve<IGeometryFactory>();
                Assert.Fail();
            }
            catch (InvalidOperationException) {  }

            // test case 2: service with no parameter

            _container.Register<IMetadataFactory, MetadataFactory>();
            _container.Register<IGeometryFactory, GeometryFactory>();
            _container.RegisterInstance<IReferenceSystem>(null);

            IMetadataFactory metadataFactory = _container.Resolve<IMetadataFactory>();
            Assert.IsNotNull(metadataFactory);
            Assert.IsInstanceOf<MetadataFactory>(metadataFactory);

            // test case 3: service with parameter

            try
            {
                // should not resolve geometry factory, as no reference system is specified
                _container.Resolve<IGeometryFactory>();
                Assert.Fail();
            }
            catch (InvalidOperationException) { }

            // should resolve with reference system parameter
            IGeometryFactory geometryFactory = _container.Resolve<IGeometryFactory>((IReferenceSystem)null);
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
        }        

        [TestCase]
        public void ContainerResolveInstanceTest()
        {
            IGeometryFactory factory = Factory.DefaultInstance<IGeometryFactory>();

            // test case 1: resolve unregistered instance

            try
            {
                _container.ResolveInstance<IGeometryFactory>();
                Assert.Fail();
            }
            catch (InvalidOperationException) { }

            // test case 2: resolve registeres instance

            _container.RegisterInstance<IGeometryFactory>(factory);

            IGeometryFactory resolvedFactory = _container.ResolveInstance<IGeometryFactory>();

            Assert.IsNotNull(resolvedFactory);
            Assert.AreEqual(factory, resolvedFactory);
        }

        [TestCase]
        public void ContainerTryResolveTest()
        {
            // test case 1: resolve unregistered service

            IMetadataFactory metadataFactory = null;

            Assert.IsFalse(_container.TryResolve<IMetadataFactory>(out metadataFactory));
            Assert.IsNull(metadataFactory);

            // test case 2: service with no parameter

            _container.Register<IMetadataFactory, MetadataFactory>();
            _container.Register<IGeometryFactory, GeometryFactory>();     

            Assert.IsTrue(_container.TryResolve<IMetadataFactory>(out metadataFactory));
            Assert.IsNotNull(metadataFactory);
            Assert.IsInstanceOf<MetadataFactory>(metadataFactory);

            // test case 3: service with parameter

            // should not resolve geometry factory, as no reference system is specified
            IGeometryFactory geometryFactory = null;
            Assert.IsFalse(_container.TryResolve<IGeometryFactory>(out geometryFactory));
            Assert.IsNull(geometryFactory);

            // should resolve with reference system parameter
            Assert.IsTrue(_container.TryResolve<IGeometryFactory>(new Object[]{ (IReferenceSystem)null }, out geometryFactory));
            Assert.IsNotNull(geometryFactory);

            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
        }

        [TestCase]
        public void ContainerTryResolveInstanceTest()
        {
            // test case 1: resolve unregistered instance

            IGeometryFactory resolvedGeometryFactory;

            Assert.IsFalse(_container.TryResolveInstance<IGeometryFactory>(out resolvedGeometryFactory));
            Assert.IsNull(resolvedGeometryFactory);

            // test case 2: resolve registered instance

            IGeometryFactory geometryfactory = Factory.DefaultInstance<IGeometryFactory>();

            _container.RegisterInstance<IGeometryFactory>(geometryfactory);

            Assert.IsTrue(_container.TryResolveInstance<IGeometryFactory>(out resolvedGeometryFactory));
            Assert.IsNotNull(resolvedGeometryFactory);

            Assert.AreEqual(geometryfactory, resolvedGeometryFactory);
        }
    }
}
