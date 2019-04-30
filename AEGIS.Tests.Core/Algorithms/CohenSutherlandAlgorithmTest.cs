/// <copyright file="CohenSutherlandAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="CohenSutherlandAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class CohenSutherlandAlgorithmTest
    {
        #region Private fields

        private Envelope _envelope;
        private List<List<Coordinate>> _lineStrings;
        private IBasicLineString _basicLineString;
        private List<IBasicPolygon> _basicPolygons;
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
                    new Coordinate(5, 15),
                    new Coordinate(14, 15),
                    new Coordinate(18, 15),
                    new Coordinate(25, 15)
                },

                new List<Coordinate>()
                {
                    new Coordinate(5, 15),
                    new Coordinate(25, 15)
                },

                new List<Coordinate>()
                {
                    new Coordinate(5, 12),
                    new Coordinate(25, 12),
                    new Coordinate(25, 18),
                    new Coordinate(5, 18)
                },
            };
            
            _basicLineString = new BasicLineString(
                new List<Coordinate>()
                {
                    new Coordinate(5, 15),
                    new Coordinate(15, 15),
                    new Coordinate(25, 15)
                });

            _basicPolygons = new List<IBasicPolygon>()
            {
                new BasicPolygon(
                new List<Coordinate>()
                {
                    new Coordinate(5, 12),
                    new Coordinate(25, 12),
                    new Coordinate(25, 18),
                    new Coordinate(5, 18)
                }),
                new BasicPolygon(
                new List<Coordinate>()
                {
                    new Coordinate(5, 12),
                    new Coordinate(25, 12),
                    new Coordinate(25, 18),
                    new Coordinate(5, 18)
                })
            };

            _comparer = new GeometryComparer();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="Compute" /> method.
        /// </summary>
        [Test]
        public void CohenSutherlandAlgorithmComputeTest()
        {
            List<BasicLineString> totalExpected = new List<BasicLineString>();

            CohenSutherlandAlgorithm cohenSutherlandAlgorithm = new CohenSutherlandAlgorithm(_lineStrings[0], _envelope);
            cohenSutherlandAlgorithm.Compute();

            List<BasicLineString> actual = cohenSutherlandAlgorithm.Result.Select(lineString => new BasicLineString(lineString)).ToList();
            List<BasicLineString> expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 15),
                    new Coordinate(15, 15),
                    new Coordinate(20, 15)
                })
            };
            totalExpected.AddRange(expected);
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            cohenSutherlandAlgorithm = new CohenSutherlandAlgorithm(_lineStrings[1], _envelope);
            cohenSutherlandAlgorithm.Compute();

            actual = cohenSutherlandAlgorithm.Result
                .Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 15),
                    new Coordinate(14, 15),
                    new Coordinate(18, 15),
                    new Coordinate(20, 15)
                })
            };
            totalExpected.AddRange(expected);
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            cohenSutherlandAlgorithm = new CohenSutherlandAlgorithm(_lineStrings[2], _envelope);
            cohenSutherlandAlgorithm.Compute();
            
            actual = cohenSutherlandAlgorithm.Result
                .Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 15),
                    new Coordinate(20, 15)
                })
            };
            totalExpected.AddRange(expected);
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            cohenSutherlandAlgorithm = new CohenSutherlandAlgorithm(_lineStrings[3], _envelope);
            cohenSutherlandAlgorithm.Compute();
            
            actual = cohenSutherlandAlgorithm.Result
                .Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 12),
                    new Coordinate(20, 12)
                }),

                new BasicLineString(new Coordinate[] {
                    new Coordinate(20, 18),
                    new Coordinate(10, 18)
                })
            };
            totalExpected.AddRange(expected);
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            cohenSutherlandAlgorithm = new CohenSutherlandAlgorithm(_lineStrings, _envelope);
            cohenSutherlandAlgorithm.Compute();
            
            List<BasicLineString> totalActualLinestrings = cohenSutherlandAlgorithm.Result
                .Select(l => new BasicLineString(l)).ToList();
            Assert.AreEqual(0, _comparer.Compare(totalActualLinestrings, totalExpected));
        }

        /// <summary>
        /// Tests the <see cref="Clip" /> method.
        /// </summary>
        [Test]
        public void CohenSutherlandAlgorithmClipTest()
        {
            List<BasicLineString> actual = CohenSutherlandAlgorithm.Clip(_basicLineString, _envelope)
                .Select(l => new BasicLineString(l)).ToList();
            List<BasicLineString> expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 15),
                    new Coordinate(15, 15),
                    new Coordinate(20, 15)
                })
            };
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            actual = CohenSutherlandAlgorithm.Clip(_basicPolygons[0], _envelope)
                .Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 12),
                    new Coordinate(20, 12)
                }),

                new BasicLineString(new Coordinate[] {
                    new Coordinate(20, 18),
                    new Coordinate(10, 18)
                })
            };
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            actual = CohenSutherlandAlgorithm.Clip(_basicPolygons, _envelope)
                .Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 12),
                    new Coordinate(20, 12)
                }),

                new BasicLineString(new Coordinate[] {
                    new Coordinate(20, 18),
                    new Coordinate(10, 18)
                }),

                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 12),
                    new Coordinate(20, 12)
                }),

                new BasicLineString(new Coordinate[] {
                    new Coordinate(20, 18),
                    new Coordinate(10, 18)
                })
            };
            Assert.AreEqual(0, _comparer.Compare(actual, expected));
        }

        #endregion
    }
}