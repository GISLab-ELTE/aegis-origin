/// <copyright file="ConcurrentContainerTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Management.InversionOfControl;
using ELTE.AEGIS.Metadata;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Management.InversionOfControl
{
    /// <summary>
    /// Test fixture for the <see cref="ConcurrentContainer" /> class.
    /// </summary>
    [TestFixture]
    public class ConcurrentContainerTest
    {
        #region Private fields

        /// <summary>
        /// The container.
        /// </summary>
        private ConcurrentContainer _container;

        #endregion

        #region Test setup and teardown

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _container = new ConcurrentContainer();
        }

        /// <summary>
        /// Test teardown.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="Register" /> method.
        /// </summary>
        [Test]
        public void ConcurrentContainerRegisterTest()
        {
            // registration by type

            Assert.IsFalse(_container.IsRegistered<IMetadataFactory>());
            Assert.IsFalse(_container.IsRegistered<IGeometryFactory>());

            _container.Register<IMetadataFactory, MetadataFactory>();
            _container.Register<IGeometryFactory, GeometryFactory>();

            Assert.IsTrue(_container.IsRegistered<IMetadataFactory>());
            Assert.IsTrue(_container.IsRegistered<IMetadataFactory, MetadataFactory>());
            Assert.IsTrue(_container.IsRegistered<IGeometryFactory>());
            Assert.IsTrue(_container.IsRegistered<IGeometryFactory, GeometryFactory>());

            Assert.IsTrue(_container.IsRegistered(typeof(IMetadataFactory)));
            Assert.IsTrue(_container.IsRegistered(typeof(IMetadataFactory).FullName));
            Assert.IsTrue(_container.IsRegistered(typeof(IMetadataFactory), typeof(MetadataFactory)));
            Assert.IsTrue(_container.IsRegistered<IMetadataFactory>());

            Assert.Throws<ArgumentNullException>(() => _container.Register(null, typeof(GeometryFactory)));
            Assert.Throws<ArgumentNullException>(() => _container.Register(typeof(IGeometryFactory), null));
            Assert.Throws<InvalidOperationException>(() => _container.Register(typeof(IGeometryFactory), typeof(MetadataFactory)));
            Assert.Throws<InvalidOperationException>(() => _container.Register(typeof(IGeometryFactory), typeof(IGeometryFactory)));
            Assert.Throws<InvalidOperationException>(() => _container.Register<IGeometryFactory, GeometryFactory>());
            Assert.Throws<InvalidOperationException>(() => _container.Register<GeometryFactory, GeometryFactory>());

            _container.Register<IGeometryFactory, GeometryFactory>(true); // overwrite registration
            Assert.IsTrue(_container.IsRegistered<IGeometryFactory>());

            _container.Register(typeof(IGeometryFactory), typeof(GeometryFactory), true);
            Assert.IsTrue(_container.IsRegistered<IGeometryFactory>());

            Assert.IsTrue(_container.Unregister<IGeometryFactory>()); // unregister
            Assert.IsFalse(_container.IsRegistered<IGeometryFactory>());
            Assert.IsFalse(_container.Unregister<IGeometryFactory>());


            // registration by name

            Assert.IsFalse(_container.IsRegistered("GeometryFactoryRegistration"));

            _container.Register<IGeometryFactory, GeometryFactory>("GeometryFactoryRegistration");
            Assert.IsTrue(_container.IsRegistered("GeometryFactoryRegistration"));

            _container.Register<IGeometryFactory, GeometryFactory>("GeometryFactoryRegistration", true);
            Assert.IsTrue(_container.IsRegistered("GeometryFactoryRegistration"));

            _container.Register("GeometryFactoryRegistration", typeof(IGeometryFactory), typeof(GeometryFactory), true);
            Assert.IsTrue(_container.IsRegistered("GeometryFactoryRegistration"));

            Assert.IsTrue(_container.Unregister("GeometryFactoryRegistration")); // unregister
            Assert.IsFalse(_container.IsRegistered("GeometryFactoryRegistration"));
            Assert.IsFalse(_container.Unregister("GeometryFactoryRegistration"));

            _container.Register("GeometryFactoryRegistration", typeof(IGeometryFactory), typeof(GeometryFactory));
            Assert.IsTrue(_container.IsRegistered("GeometryFactoryRegistration"));


            // exceptions

            Assert.Throws<ArgumentNullException>(() => _container.Unregister((String)null));
            Assert.Throws<ArgumentNullException>(() => _container.Unregister((Type)null));
            Assert.Throws<InvalidOperationException>(() => _container.Register<IMetadataFactory, IMetadataFactory>(true));
            Assert.Throws<InvalidOperationException>(() => _container.Register<IGeometryFactory, GeometryFactory>("GeometryFactoryRegistration"));

            Assert.Throws<ArgumentNullException>(() => _container.Register((Type)null, (Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.Register((Type)null, (Type)null, true));
            Assert.Throws<ArgumentNullException>(() => _container.Register(typeof(IMetadataFactory), (Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.Register((String)null, (Type)null, (Type)null, true));
            Assert.Throws<ArgumentNullException>(() => _container.Register(typeof(IMetadataFactory).FullName, (Type)null, (Type)null, true));
            Assert.Throws<ArgumentNullException>(() => _container.Register(typeof(IMetadataFactory).FullName, typeof(IMetadataFactory), (Type)null, true));

            Assert.Throws<ArgumentNullException>(() => _container.IsRegistered((String)null));
            Assert.Throws<ArgumentNullException>(() => _container.IsRegistered((Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.IsRegistered((Type)null, (Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.IsRegistered(typeof(IMetadataFactory), (Type)null));
        }

        /// <summary>
        /// Tests the <see cref="RegisterInstance" /> method.
        /// </summary>
        [Test]
        public void ConcurrentContainerRegisterInstanceTest()
        {
            // registration by type

            Assert.IsFalse(_container.IsRegisteredInstance<IMetadataFactory>());

            MetadataFactory unnamedFactory = new MetadataFactory();

            _container.RegisterInstance<IMetadataFactory>(unnamedFactory);

            Assert.IsTrue(_container.IsRegisteredInstance<IMetadataFactory>());
            Assert.IsTrue(_container.IsRegisteredInstance(typeof(IMetadataFactory)));
            Assert.IsTrue(_container.IsRegisteredInstance(typeof(IMetadataFactory).FullName));
            Assert.IsTrue(_container.IsRegisteredInstance(unnamedFactory));

            Assert.Throws<ArgumentNullException>(() => _container.RegisterInstance(null, typeof(GeometryFactory)));
            Assert.Throws<ArgumentNullException>(() => _container.RegisterInstance(typeof(IGeometryFactory), null));
            Assert.Throws<ArgumentException>(() => _container.RegisterInstance(typeof(IGeometryFactory), unnamedFactory));
            Assert.Throws<InvalidOperationException>(() => _container.RegisterInstance(typeof(IMetadataFactory), unnamedFactory));

            _container.RegisterInstance<IMetadataFactory>(unnamedFactory, true);
            Assert.IsTrue(_container.IsRegisteredInstance<IMetadataFactory>());
            Assert.IsTrue(_container.IsRegisteredInstance(unnamedFactory));

            _container.RegisterInstance(typeof(IMetadataFactory), unnamedFactory, true);
            Assert.IsTrue(_container.IsRegisteredInstance<IMetadataFactory>());

            Assert.IsTrue(_container.UnregisterInstance<IMetadataFactory>()); // unregister
            Assert.IsFalse(_container.IsRegisteredInstance<IMetadataFactory>());
            Assert.IsFalse(_container.UnregisterInstance<IMetadataFactory>());

            _container.RegisterInstance<IMetadataFactory>(unnamedFactory);
            Assert.IsTrue(_container.UnregisterInstance(unnamedFactory));
            Assert.IsFalse(_container.IsRegisteredInstance<IMetadataFactory>());

            Assert.IsFalse(_container.UnregisterInstance(new Object()));


            // registration by name

            MetadataFactory namedFactory = new MetadataFactory();

            _container.RegisterInstance<IMetadataFactory>("MetadataFactoryRegistration", namedFactory);
            Assert.IsTrue(_container.IsRegisteredInstance("MetadataFactoryRegistration"));
            Assert.IsTrue(_container.IsRegisteredInstance(namedFactory));

            _container.RegisterInstance<IMetadataFactory>("MetadataFactoryRegistration", namedFactory, true);
            Assert.IsTrue(_container.IsRegisteredInstance("MetadataFactoryRegistration"));

            _container.RegisterInstance("MetadataFactoryRegistration", typeof(IMetadataFactory), namedFactory, true);
            Assert.IsTrue(_container.IsRegisteredInstance("MetadataFactoryRegistration"));

            Assert.IsTrue(_container.UnregisterInstance("MetadataFactoryRegistration")); // unregister
            Assert.IsFalse(_container.IsRegisteredInstance("MetadataFactoryRegistration"));
            Assert.IsFalse(_container.UnregisterInstance("MetadataFactoryRegistration"));

            _container.RegisterInstance("MetadataFactoryRegistration", typeof(IMetadataFactory), namedFactory);
            Assert.IsTrue(_container.IsRegisteredInstance("MetadataFactoryRegistration"));


            // exceptions

            Assert.Throws<ArgumentNullException>(() => _container.UnregisterInstance((String)null));
            Assert.Throws<ArgumentNullException>(() => _container.UnregisterInstance((Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.UnregisterInstance((Object)null));
            Assert.Throws<InvalidOperationException>(() => _container.RegisterInstance<IMetadataFactory>("MetadataFactoryRegistration", unnamedFactory));

            Assert.Throws<ArgumentNullException>(() => _container.RegisterInstance((Type)null, (Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.RegisterInstance((Type)null, (Type)null, true));
            Assert.Throws<ArgumentNullException>(() => _container.RegisterInstance(typeof(IMetadataFactory), (Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.RegisterInstance((String)null, (Type)null, (Type)null, true));
            Assert.Throws<ArgumentNullException>(() => _container.RegisterInstance(typeof(IMetadataFactory).FullName, (Type)null, (Type)null, true));
            Assert.Throws<ArgumentNullException>(() => _container.RegisterInstance(typeof(IMetadataFactory).FullName, typeof(IMetadataFactory), (Type)null, true));

            Assert.Throws<ArgumentNullException>(() => _container.IsRegisteredInstance((String)null));
            Assert.Throws<ArgumentNullException>(() => _container.IsRegisteredInstance((Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.IsRegisteredInstance((Object)null));
        }

        /// <summary>
        /// Tests the <see cref="Resolve" /> method.
        /// </summary>
        [Test]
        public void ConcurrentContainerResolveTest()
        {
            // unnamed registration

            _container.Register<IMetadataFactory, MetadataFactory>();

            IMetadataFactory resolvedFactory = _container.Resolve<IMetadataFactory>();
            Assert.IsNotNull(resolvedFactory);
            Assert.IsInstanceOf<MetadataFactory>(resolvedFactory);

            resolvedFactory = _container.Resolve<IMetadataFactory>(typeof(IMetadataFactory).FullName);
            Assert.IsNotNull(resolvedFactory);
            Assert.IsInstanceOf<MetadataFactory>(resolvedFactory);

            Object resolvedObject = _container.Resolve(typeof(IMetadataFactory));
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<MetadataFactory>(resolvedObject);

            resolvedObject = _container.Resolve(typeof(IMetadataFactory).FullName);
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<MetadataFactory>(resolvedObject);

            resolvedObject = _container.Resolve(typeof(IMetadataFactory), typeof(IMetadataFactory).FullName);
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<MetadataFactory>(resolvedObject);


            // named registration

            _container.Register<IMetadataFactory, MetadataFactory>("MetadataFactoryRegistration");

            resolvedFactory = _container.Resolve<IMetadataFactory>("MetadataFactoryRegistration");
            Assert.IsNotNull(resolvedFactory);
            Assert.IsInstanceOf<MetadataFactory>(resolvedFactory);

            resolvedObject = _container.Resolve("MetadataFactoryRegistration");
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<MetadataFactory>(resolvedObject);

            resolvedObject = _container.Resolve(typeof(IMetadataFactory), "MetadataFactoryRegistration");
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<MetadataFactory>(resolvedObject);


            // parameterized

            _container.Register<IGeometryFactory, GeometryFactory>();
            IGeometryFactory geometryFactory = _container.Resolve<IGeometryFactory>(PrecisionModel.Default, (IReferenceSystem)null);
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
            Assert.AreEqual(PrecisionModel.Default, geometryFactory.PrecisionModel);
            Assert.IsNull(geometryFactory.ReferenceSystem);

            geometryFactory = _container.Resolve<IGeometryFactory>(typeof(IGeometryFactory).FullName, PrecisionModel.Default, (IReferenceSystem)null);
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
            Assert.AreEqual(PrecisionModel.Default, geometryFactory.PrecisionModel);
            Assert.IsNull(geometryFactory.ReferenceSystem);

            geometryFactory = (IGeometryFactory)_container.Resolve(typeof(IGeometryFactory), PrecisionModel.Default, (IReferenceSystem)null);
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
            Assert.AreEqual(PrecisionModel.Default, geometryFactory.PrecisionModel);
            Assert.IsNull(geometryFactory.ReferenceSystem);

            geometryFactory = (IGeometryFactory)_container.Resolve(typeof(IGeometryFactory), typeof(IGeometryFactory).FullName, PrecisionModel.Default, (IReferenceSystem)null);
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
            Assert.AreEqual(PrecisionModel.Default, geometryFactory.PrecisionModel);
            Assert.IsNull(geometryFactory.ReferenceSystem);

            geometryFactory = (IGeometryFactory)_container.Resolve(typeof(IGeometryFactory).FullName, PrecisionModel.Default, (IReferenceSystem)null);
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
            Assert.AreEqual(PrecisionModel.Default, geometryFactory.PrecisionModel);
            Assert.IsNull(geometryFactory.ReferenceSystem);

            geometryFactory = _container.Resolve<IGeometryFactory>(typeof(IGeometryFactory).FullName, PrecisionModel.Default, (IReferenceSystem)null);
            Assert.IsNotNull(geometryFactory);
            Assert.IsInstanceOf<GeometryFactory>(geometryFactory);
            Assert.AreEqual(PrecisionModel.Default, geometryFactory.PrecisionModel);
            Assert.IsNull(geometryFactory.ReferenceSystem);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => _container.Resolve<IMetadataFactory>((String)null));
            Assert.Throws<ArgumentNullException>(() => _container.Resolve<IMetadataFactory>((String)null));
            Assert.Throws<InvalidOperationException>(() => _container.Resolve<IMetadataFactory>("InvalidName"));
            Assert.Throws<InvalidOperationException>(() => _container.Resolve<IGeometryFactory>("MetadataFactoryRegistration"));
            Assert.Throws<ArgumentNullException>(() => _container.Resolve((Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.Resolve((String)null));
            Assert.Throws<ArgumentNullException>(() => _container.Resolve((String)null, null));
            Assert.Throws<ArgumentNullException>(() => _container.Resolve((Type)null, "InvalidName"));
            Assert.Throws<ArgumentNullException>(() => _container.Resolve((Type)null, "InvalidName", null));
            Assert.Throws<ArgumentNullException>(() => _container.Resolve(typeof(IGeometryFactory), null, null));
            Assert.Throws<InvalidOperationException>(() => _container.Resolve("InvalidName"));
            Assert.Throws<InvalidOperationException>(() => _container.Resolve("InvalidName", null));
        }

        /// <summary>
        /// Tests the <see cref="ResolveInstance" /> method.
        /// </summary>
        [Test]
        public void ConcurrentContainerResolveInstanceTest()
        {
            // unnamed registration

            IMetadataFactory unnamedFactory = new MetadataFactory();
            _container.RegisterInstance<IMetadataFactory>(unnamedFactory);

            IMetadataFactory resolvedFactory = _container.ResolveInstance<IMetadataFactory>();
            Assert.IsNotNull(resolvedFactory);
            Assert.AreEqual(unnamedFactory, resolvedFactory);

            resolvedFactory = _container.ResolveInstance<IMetadataFactory>(typeof(IMetadataFactory).FullName);
            Assert.IsNotNull(resolvedFactory);
            Assert.AreEqual(unnamedFactory, resolvedFactory);

            Object resolvedObject = _container.ResolveInstance(typeof(IMetadataFactory));
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(unnamedFactory, resolvedObject);

            resolvedObject = _container.ResolveInstance(typeof(IMetadataFactory).FullName);
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(unnamedFactory, resolvedObject);

            resolvedObject = _container.ResolveInstance(typeof(IMetadataFactory), typeof(IMetadataFactory).FullName);
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(unnamedFactory, resolvedObject);


            // named registration

            IMetadataFactory namedFactory = new MetadataFactory();
            _container.RegisterInstance<IMetadataFactory>("MetadataFactoryRegistration", namedFactory);

            resolvedFactory = _container.ResolveInstance<IMetadataFactory>("MetadataFactoryRegistration");
            Assert.IsNotNull(resolvedFactory);
            Assert.AreEqual(namedFactory, resolvedFactory);

            resolvedObject = _container.ResolveInstance("MetadataFactoryRegistration");
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(namedFactory, resolvedObject);

            resolvedObject = _container.ResolveInstance(typeof(IMetadataFactory), "MetadataFactoryRegistration");
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(namedFactory, resolvedObject);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => _container.ResolveInstance<IMetadataFactory>(null));
            Assert.Throws<InvalidOperationException>(() => _container.ResolveInstance<IMetadataFactory>("InvalidName"));
            Assert.Throws<InvalidOperationException>(() => _container.ResolveInstance<IGeometryFactory>("MetadataFactoryRegistration"));
            Assert.Throws<InvalidOperationException>(() => _container.ResolveInstance<IGeometryFactory>());

            Assert.Throws<ArgumentNullException>(() => _container.ResolveInstance((Type)null));
            Assert.Throws<ArgumentNullException>(() => _container.ResolveInstance((String)null));
            Assert.Throws<ArgumentNullException>(() => _container.ResolveInstance((Type)null, (String)null));
        }

        /// <summary>
        /// Tests the <see cref="TryResolve" /> method.
        /// </summary>
        [Test]
        public void ConcurrentContainerTryResolveTest()
        {
            // unnamed registration

            _container.Register<IMetadataFactory, MetadataFactory>();

            IMetadataFactory resolvedMetadataFactory;
            Object resolvedObject;

            Assert.IsTrue(_container.TryResolve<IMetadataFactory>(out resolvedMetadataFactory));
            Assert.IsNotNull(resolvedMetadataFactory);
            Assert.IsInstanceOf<MetadataFactory>(resolvedMetadataFactory);

            Assert.IsTrue(_container.TryResolve(typeof(IMetadataFactory), out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<MetadataFactory>(resolvedObject);

            Assert.IsTrue(_container.TryResolve(typeof(IMetadataFactory).FullName, out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<MetadataFactory>(resolvedObject);


            // named registration

            _container.Register<IMetadataFactory, MetadataFactory>("MetadataFactoryRegistration");

            Assert.IsTrue(_container.TryResolve<IMetadataFactory>(out resolvedMetadataFactory));
            Assert.IsNotNull(resolvedMetadataFactory);
            Assert.IsInstanceOf<MetadataFactory>(resolvedMetadataFactory);

            Assert.IsTrue(_container.TryResolve("MetadataFactoryRegistration", out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<MetadataFactory>(resolvedObject);


            // parameterized

            _container.Register<IGeometryFactory, GeometryFactory>();
            Assert.IsTrue(_container.TryResolve(typeof(IGeometryFactory), new Object[] { PrecisionModel.Default, (IReferenceSystem)null }, out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.IsInstanceOf<GeometryFactory>(resolvedObject);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => _container.TryResolve((Type)null, out resolvedObject));
            Assert.Throws<ArgumentNullException>(() => _container.TryResolve((String)null, out resolvedObject));
        }

        /// <summary>
        /// Tests the <see cref="TryResolveInstance" /> method.
        /// </summary>
        [Test]
        public void ConcurrentContainerTryResolveInstanceTest()
        {
            // unnamed registration

            IMetadataFactory unnamedFactory = new MetadataFactory();
            _container.RegisterInstance<IMetadataFactory>(unnamedFactory);

            IMetadataFactory resolvedMetadataFactory;
            Object resolvedObject;

            Assert.IsTrue(_container.TryResolveInstance<IMetadataFactory>(out resolvedMetadataFactory));
            Assert.IsNotNull(resolvedMetadataFactory);
            Assert.AreEqual(unnamedFactory, resolvedMetadataFactory);

            Assert.IsTrue(_container.TryResolveInstance<IMetadataFactory>(typeof(IMetadataFactory).FullName, out resolvedMetadataFactory));
            Assert.IsNotNull(resolvedMetadataFactory);
            Assert.AreEqual(unnamedFactory, resolvedMetadataFactory);

            Assert.IsTrue(_container.TryResolveInstance(typeof(IMetadataFactory), out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(unnamedFactory, resolvedObject);

            Assert.IsTrue(_container.TryResolveInstance(typeof(IMetadataFactory).FullName, out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(unnamedFactory, resolvedObject);

            Assert.IsTrue(_container.TryResolveInstance(typeof(IMetadataFactory), typeof(IMetadataFactory).FullName, out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(unnamedFactory, resolvedObject);

            IGeometryFactory resolvedGeometryFactory;
            Assert.IsFalse(_container.TryResolveInstance<IGeometryFactory>(out resolvedGeometryFactory));


            // named registration

            IMetadataFactory namedFactory = new MetadataFactory();
            _container.RegisterInstance<IMetadataFactory>("MetadataFactoryRegistration", namedFactory);

            Assert.IsTrue(_container.TryResolveInstance<IMetadataFactory>("MetadataFactoryRegistration", out resolvedMetadataFactory));
            Assert.IsNotNull(resolvedMetadataFactory);
            Assert.AreEqual(namedFactory, resolvedMetadataFactory);

            Assert.IsTrue(_container.TryResolveInstance("MetadataFactoryRegistration", out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(namedFactory, resolvedObject);

            Assert.IsTrue(_container.TryResolveInstance(typeof(IMetadataFactory), "MetadataFactoryRegistration", out resolvedObject));
            Assert.IsNotNull(resolvedObject);
            Assert.AreEqual(namedFactory, resolvedObject);

            Assert.IsFalse(_container.TryResolveInstance<IMetadataFactory>("InvalidName", out resolvedMetadataFactory));
            Assert.IsFalse(_container.TryResolveInstance<IGeometryFactory>("MetadataFactoryRegistration", out resolvedGeometryFactory));


            // exceptions

            Assert.Throws<ArgumentNullException>(() => _container.TryResolveInstance<IMetadataFactory>(null, out resolvedMetadataFactory));
            Assert.Throws<ArgumentNullException>(() => _container.TryResolveInstance((Type)null, out resolvedObject));
            Assert.Throws<ArgumentNullException>(() => _container.TryResolveInstance((String)null, out resolvedObject));
            Assert.Throws<ArgumentNullException>(() => _container.TryResolveInstance((Type)null, (String)null, out resolvedObject));
        }

        #endregion
    }
}
