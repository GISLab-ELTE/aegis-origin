/// <copyright file="CyrusBeckAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Daniel Ballagi</author>

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="CyrusBeckAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class CyrusBeckAlgorithmTest
    {
        #region Private fields

        private Envelope _envelope;
        private List<Coordinate> _polygon;
        private List<List<Coordinate>> _lineStrings;
        private GeometryComparer _comparer;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _envelope = new Envelope(10, 20, 10, 20, 0, 0);

            _polygon = new List<Coordinate>()
            {
                new Coordinate(1, 0),
                new Coordinate(0, 2),
                new Coordinate(3, 2),
                new Coordinate(2, 0)
            };

            _lineStrings = new List<List<Coordinate>>()
            {
                new List<Coordinate>()
                {
                    new Coordinate(5, 15),
                    new Coordinate(15, 15),
                    new Coordinate(25, 15)
                },

                new List<Coordinate>()
                {
                    new Coordinate(0, 1),
                    new Coordinate(2, 1),
                    new Coordinate(3, 1)
                },

                new List<Coordinate>()
                {
                    new Coordinate(5, 22),
                    new Coordinate(25, 22)
                }
            };

            _comparer = new GeometryComparer();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="Compute" /> method.
        /// </summary>
        [Test]
        public void CyrusBeckAlgorithmComputeTest()
        {
            CyrusBeckAlgorithm cyrusBeckAlgorithm = new CyrusBeckAlgorithm(_lineStrings[0], _envelope);
            cyrusBeckAlgorithm.Compute();

            List<BasicLineString> actual = cyrusBeckAlgorithm.Result.Select(l => new BasicLineString(l)).ToList();
            List<BasicLineString> expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 15),
                    new Coordinate(15, 15),
                    new Coordinate(20, 15)
                })
            };
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            cyrusBeckAlgorithm = new CyrusBeckAlgorithm(_lineStrings[2], _envelope);
            cyrusBeckAlgorithm.Compute();

            actual = cyrusBeckAlgorithm.Result.Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>();
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            cyrusBeckAlgorithm = new CyrusBeckAlgorithm(_lineStrings[1], _polygon);
            cyrusBeckAlgorithm.Compute();

            actual = cyrusBeckAlgorithm.Result.Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(0.5, 1),
                    new Coordinate(2, 1),
                    new Coordinate(2.5, 1)
                })
            };
            Assert.AreEqual(0, _comparer.Compare(actual, expected));
        }

        #endregion
    }
}
