/// <copyright file="DouglasPeuckerAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Bence Molnár</author>

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="DouglasPeuckerAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class DouglasPeuckerAlgorithmTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="Simplify" /> method.
        /// </summary>
        [Test]
        public void DouglasPeuckerAlgorithmSimplifyTest()
        {
            // first line string

            Double delta = 5.0;

            List<Coordinate> coordinates = new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(1.0, 1.0, 0.0),
                new Coordinate(2.0, 5.0, 0.0),
                new Coordinate(5.0, 2.0, 0.0)
            });

            IList<Coordinate> result = DouglasPeuckerAlgorithm.Simplify(coordinates, delta);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].X == 1.0 && result[0].Y == 1.0);
            Assert.IsTrue(result[1].X == 5.0 && result[1].Y == 2.0);


            // third line string

            delta = 2.0;

            coordinates = new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(0.0, 3.0, 0.0),
                new Coordinate(0.5, 3.0, 1.0),
                new Coordinate(1.5, 2.0, 0.0),
                new Coordinate(2.5, 1.0, 0.0),
                new Coordinate(3.7, 3.9, 0.0),
                new Coordinate(5.0, 3.0, 0.0)
            });

            result = DouglasPeuckerAlgorithm.Simplify(coordinates, delta);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].X == 0.0 && result[0].Y == 3.0 && result[0].Z == .0);
            Assert.IsTrue(result[1].X == 5.0 && result[1].Y == 3.0 && result[1].Z == .0);


            // third line string

            delta = 2.0;

            coordinates = new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(0.0, 3.0, 0.0),
                new Coordinate(0.0, 3.0, 3.0),
                new Coordinate(1.5, 2.0, 0.0),
                new Coordinate(2.5, 1.0, 0.0),
                new Coordinate(3.7, 3.9, 0.0),
                new Coordinate(5.0, 3.0, 0.0)
            });

            result = DouglasPeuckerAlgorithm.Simplify(coordinates, delta);

            Assert.IsTrue(result.Count == 3);
            Assert.IsTrue(result[0].X == 0.0 && result[0].Y == 3.0 && result[0].Z == .0);
            Assert.IsTrue(result[1].X == 0.0 && result[1].Y == 3.0 && result[1].Z == 3.0);
            Assert.IsTrue(result[2].X == 5.0 && result[2].Y == 3.0 && result[2].Z == .0);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => DouglasPeuckerAlgorithm.Simplify((IList<Coordinate>)null, 1.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => DouglasPeuckerAlgorithm.Simplify(coordinates, 0));
        }

        #endregion
    }
}
