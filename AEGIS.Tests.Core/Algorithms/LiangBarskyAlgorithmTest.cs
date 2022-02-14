/// <copyright file="LiangBarskyAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="LiangBarskyAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class LiangBarskyAlgorithmTest
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

                new List<Coordinate>()
                {
                    new Coordinate(5, 25),
                    new Coordinate(25, 25)
                }
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
        public void LiangBarskyAlgorithmComputeTest()
        {
            List<BasicLineString> totalExpected = new List<BasicLineString>();

            LiangBarskyAlgorithm liangBarskyAlgorithm = new LiangBarskyAlgorithm(_lineStrings[0], _envelope);
            liangBarskyAlgorithm.Compute();

            List<BasicLineString> actual = liangBarskyAlgorithm.Result.Select(lineString => new BasicLineString(lineString)).ToList();
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

            liangBarskyAlgorithm = new LiangBarskyAlgorithm(_lineStrings[1], _envelope);
            liangBarskyAlgorithm.Compute();

            actual = liangBarskyAlgorithm.Result.Select(l => new BasicLineString(l)).ToList();
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

            liangBarskyAlgorithm = new LiangBarskyAlgorithm(_lineStrings[2], _envelope);
            liangBarskyAlgorithm.Compute();

            actual = liangBarskyAlgorithm.Result.Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 15),
                    new Coordinate(20, 15)
                })
            };
            totalExpected.AddRange(expected);
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            liangBarskyAlgorithm = new LiangBarskyAlgorithm(_lineStrings[3], _envelope);
            liangBarskyAlgorithm.Compute();

            actual = liangBarskyAlgorithm.Result.Select(l => new BasicLineString(l)).ToList();
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

            liangBarskyAlgorithm = new LiangBarskyAlgorithm(_lineStrings[4], _envelope);
            liangBarskyAlgorithm.Compute();

            actual = liangBarskyAlgorithm.Result.Select(l => new BasicLineString(l)).ToList();
            expected = new List<BasicLineString>();
            totalExpected.AddRange(expected);
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            liangBarskyAlgorithm = new LiangBarskyAlgorithm(_lineStrings, _envelope);
            liangBarskyAlgorithm.Compute();

            List<BasicLineString> totalActualLinestrings = liangBarskyAlgorithm.Result.Select(l => new BasicLineString(l)).ToList();
            Assert.AreEqual(0, _comparer.Compare(totalActualLinestrings, totalExpected));
        }

        /// <summary>
        /// Tests the <see cref="Clip" /> method.
        /// </summary>
        [Test]
        public void LiangBarskyAlgorithmClipTest()
        {
            List<BasicLineString> actual = LiangBarskyAlgorithm.Clip(_basicLineString, _envelope).Select(l => new BasicLineString(l)).ToList();
            List<BasicLineString> expected = new List<BasicLineString>()
            {
                new BasicLineString(new Coordinate[] {
                    new Coordinate(10, 15),
                    new Coordinate(15, 15),
                    new Coordinate(20, 15)
                })
            };
            Assert.AreEqual(0, _comparer.Compare(actual, expected));

            actual = LiangBarskyAlgorithm.Clip(_basicPolygons[0], _envelope).Select(l => new BasicLineString(l)).ToList();
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

            actual = LiangBarskyAlgorithm.Clip(_basicPolygons, _envelope).Select(l => new BasicLineString(l)).ToList();
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
