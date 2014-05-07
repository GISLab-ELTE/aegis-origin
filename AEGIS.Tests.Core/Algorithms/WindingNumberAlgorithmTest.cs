/// <copyright file="WindingNumberAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System.Collections.Generic;
using ELTE.AEGIS.Algorithms;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.Algorithms
{
    [TestFixture]
    public class WindingNumberAlgorithmTest
    {
        [TestCase]
        public void WindingNumberAlgorithmIsInsidePolygonTest()
        {
            // test case 1: simple polygon

            Coordinate[] shell = new Coordinate[] { 
                new Coordinate(0, 0), new Coordinate(10, 0), 
                new Coordinate(10, 10), new Coordinate(0, 10) 
            };

            Coordinate coordinate = new Coordinate(5, 5);
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, coordinate));

            coordinate = new Coordinate(10, 5);
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, coordinate));

            coordinate = new Coordinate(15, 5);
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, coordinate));

            
            // test case 2: polygon with holes

            shell = new Coordinate[] { 
                new Coordinate(0, 0), new Coordinate(20, 0), 
                new Coordinate(20, 20), new Coordinate(0, 20) 
            };
            Coordinate[] hole1 = new Coordinate[] {
                new Coordinate(5, 5), new Coordinate(5, 10), 
                new Coordinate(10, 10), new Coordinate(10, 5) 
            };
            Coordinate[] hole2 = new Coordinate[] {
                new Coordinate(10, 10), new Coordinate(5, 15), 
                new Coordinate(15, 15), new Coordinate(15, 10) 
            };

            coordinate = new Coordinate(10, 10);
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(18, 8);
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(12, 2);
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(12, 12);
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(8, 8);
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, coordinate));
        }
    }
}
