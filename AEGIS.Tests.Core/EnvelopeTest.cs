// <copyright file="EnvelopeTest.cs" company="Eötvös Loránd University (ELTE)">
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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests
{
    /// <summary>
    /// Test fixture for the <see cref="Envelope" /> class.
    /// </summary>
    [TestFixture]
    public class EnvelopeTest
    {
        #region Test methods

        /// <summary>
        /// Test method for the constructor.
        /// </summary>
        [Test]
        public void EnvelopeConstructorTest()
        {
            // random values

            Random random = new Random();

            for (Int32 i = 0; i < 1000; i++)
            {
                Double firstX = random.NextDouble() * random.Next();
                Double firstY = random.NextDouble() * random.Next();
                Double firstZ = random.NextDouble() * random.Next();
                Double secondX = random.NextDouble() * random.Next();
                Double secondY = random.NextDouble() * random.Next();
                Double secondZ = random.NextDouble() * random.Next();

                Envelope envelope = new Envelope(firstX, secondX, firstY, secondY, firstZ, secondZ);

                Assert.AreEqual(Math.Min(firstX, secondX), envelope.Minimum.X);
                Assert.AreEqual(Math.Min(firstX, secondX), envelope.MinX);
                Assert.AreEqual(Math.Min(firstY, secondY), envelope.Minimum.Y);
                Assert.AreEqual(Math.Min(firstY, secondY), envelope.MinY);
                Assert.AreEqual(Math.Min(firstZ, secondZ), envelope.Minimum.Z);
                Assert.AreEqual(Math.Min(firstZ, secondZ), envelope.MinZ);
                Assert.AreEqual(Math.Max(firstX, secondX), envelope.Maximum.X);
                Assert.AreEqual(Math.Max(firstX, secondX), envelope.MaxX);
                Assert.AreEqual(Math.Max(firstY, secondY), envelope.Maximum.Y);
                Assert.AreEqual(Math.Max(firstY, secondY), envelope.MaxY);
                Assert.AreEqual(Math.Max(firstZ, secondZ), envelope.Maximum.Z);
                Assert.AreEqual(Math.Max(firstZ, secondZ), envelope.MaxZ);
                Assert.AreEqual("(" + envelope.MinX + " " + envelope.MinY + " " + envelope.MinZ + ", " + envelope.MaxX + " " + envelope.MaxY + " " + envelope.MaxZ + ")",
                                envelope.ToString());

                Envelope other = new Envelope(firstX, secondX, firstY, secondY, firstZ, secondZ);

                Assert.AreEqual(envelope.GetHashCode(), other.GetHashCode());
                Assert.AreEqual(envelope.ToString(), other.ToString());
            }


            // empty envelope

            Assert.AreEqual("EMPTY (0 0 0)", new Envelope(0, 0, 0, 0, 0, 0).ToString());
            Assert.AreEqual("EMPTY (0 10 100)", new Envelope(0, 0, 10, 10, 100, 100).ToString());


            // undefined envelope

            Assert.AreEqual(Envelope.Undefined.GetHashCode(), new Envelope(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN).GetHashCode());
            Assert.AreEqual(Envelope.Undefined.ToString(), new Envelope(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN).ToString());
        }

        /// <summary>
        /// Test method for the properties.
        /// </summary>
        [Test]
        public void EnvelopePropertiesTest()
        {
            // static instances

            Assert.IsFalse(Envelope.Infinity.IsEmpty);
            Assert.IsFalse(Envelope.Infinity.IsPlanar);
            Assert.IsTrue(Envelope.Infinity.IsValid);

            Assert.IsFalse(Envelope.Undefined.IsEmpty);
            Assert.IsFalse(Envelope.Undefined.IsPlanar);
            Assert.IsFalse(Envelope.Undefined.IsValid);

            // empty instance

            Envelope envelope = new Envelope(10, 10, 10, 10, 10, 10);

            Assert.IsTrue(envelope.IsEmpty);
            Assert.IsTrue(envelope.IsPlanar);
            Assert.IsTrue(envelope.IsValid);
            Assert.AreEqual(new Coordinate(10, 10, 10), envelope.Center);
            Assert.AreEqual(0, envelope.Surface);
            Assert.AreEqual(0, envelope.Volume);

            // planar instance

            envelope = new Envelope(10, 20, 100, 200, 10, 10);

            Assert.IsFalse(envelope.IsEmpty);
            Assert.IsTrue(envelope.IsPlanar);
            Assert.IsTrue(envelope.IsValid);
            Assert.AreEqual(new Coordinate(15, 150, 10), envelope.Center);
            Assert.AreEqual(1000, envelope.Surface);
            Assert.AreEqual(0, envelope.Volume);


            // general instance

            envelope = new Envelope(10, 20, 100, 200, 1000, 2000);

            Assert.IsFalse(envelope.IsEmpty);
            Assert.IsFalse(envelope.IsPlanar);
            Assert.IsTrue(envelope.IsValid);
            Assert.AreEqual(new Coordinate(15, 150, 1500), envelope.Center);
            Assert.AreEqual(222000, envelope.Surface);
            Assert.AreEqual(1000000, envelope.Volume);
        }

        /// <summary>
        /// Test case for the <see cref="Equals" /> method.
        /// </summary>
        [Test]
        public void EnvelopeEqualsTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);
            Envelope second = new Envelope(10, 20, 100, 200, 1000, 2000);

            Envelope[] others = new Envelope[] 
            {
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001)
            };


            // IEquatable method

            Assert.IsFalse(first.Equals((Envelope)null));
            Assert.IsTrue(first.Equals(first));
            Assert.IsTrue(first.Equals(second));
            Assert.IsFalse(first.Equals(Envelope.Undefined));

            foreach (Envelope envelope in others)
                Assert.IsFalse(first.Equals(envelope));


            // Object method

            Assert.IsFalse(first.Equals((Object)null));
            Assert.IsTrue(first.Equals((Object)first));
            Assert.IsTrue(first.Equals((Object)second));

            foreach (Envelope envelope in others)
                Assert.IsFalse(first.Equals((Object)envelope));


            // static method

            Assert.IsTrue(Envelope.Equals(first.Minimum, first.Maximum, first.Minimum, first.Maximum));
            Assert.IsTrue(Envelope.Equals(first.Minimum, first.Maximum, second.Minimum, second.Maximum));

            foreach (Envelope envelope in others)
                Assert.IsFalse(Envelope.Equals(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
        }

        /// <summary>
        /// Test case for the <see cref="Contains" /> method.
        /// </summary>
        [Test]
        public void EnvelopeContainsTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // coordinate containment, object method

            Assert.IsFalse(first.Contains(Coordinate.Empty));
            Assert.IsFalse(first.Contains(Coordinate.Undefined));
            Assert.IsTrue(first.Contains(new Coordinate(15, 150, 1500)));
            Assert.IsFalse(Envelope.Undefined.Contains(Coordinate.Empty));
            Assert.IsFalse(Envelope.Undefined.Contains(Coordinate.Undefined));


            // coordinate containment, static method

            Assert.IsFalse(Envelope.Contains(first.Minimum, first.Maximum, Coordinate.Empty));
            Assert.IsFalse(Envelope.Contains(first.Minimum, first.Maximum, Coordinate.Undefined));
            Assert.IsTrue(Envelope.Contains(first.Minimum, first.Maximum, new Coordinate(15, 150, 1500)));
            Assert.IsFalse(Envelope.Contains(Envelope.Undefined.Minimum, Envelope.Undefined.Maximum, Coordinate.Empty));
            Assert.IsFalse(Envelope.Contains(Envelope.Undefined.Minimum, Envelope.Undefined.Maximum, Coordinate.Undefined));


            // envelope containment, false cases

            Assert.IsFalse(first.Contains(null));

            Envelope[] others = new Envelope[] 
            {
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(9, 20, 100, 200, 1000, 2000),
                new Envelope(10, 20, 99, 200, 1000, 2000),
                new Envelope(10, 20, 100, 200, 999, 2000),
                new Envelope(9, 21, 99, 201, 999, 2001)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsFalse(first.Contains(envelope));
                Assert.IsFalse(Envelope.Contains(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }


            // envelope containment, true cases

            Assert.IsTrue(first.Contains(first));
            Assert.IsTrue(Envelope.Contains(first.Minimum, first.Maximum, first.Minimum, first.Maximum));

            others = new Envelope[] 
            {
                new Envelope(11, 19, 101, 199, 1001, 1999),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 19, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 199, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 1999)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsTrue(first.Contains(envelope));
                Assert.IsTrue(Envelope.Contains(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }
        }

        /// <summary>
        /// Test case for the <see cref="Crosses" /> method.
        /// </summary>
        [Test]
        public void EnvelopeCrossesTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);


            // false cases

            Assert.IsFalse(first.Crosses(null));
            Assert.IsFalse(first.Crosses(first));
            Assert.IsFalse(Envelope.Crosses(first.Minimum, first.Maximum, first.Minimum, first.Maximum));

            Envelope[] others = new Envelope[] 
            {
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsFalse(first.Crosses(envelope));
                Assert.IsFalse(Envelope.Crosses(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }


            // true cases

            others = new Envelope[] 
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsTrue(first.Crosses(envelope));
                Assert.IsTrue(Envelope.Crosses(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }
        }

        /// <summary>
        /// Test case for the <see cref="Disjoint" /> method.
        /// </summary>
        [Test]
        public void EnvelopeDisjointTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);


            // false cases

            Assert.IsFalse(first.Disjoint(null));
            Assert.IsFalse(first.Disjoint(first));
            Assert.IsFalse(Envelope.Disjoint(first.Minimum, first.Maximum, first.Minimum, first.Maximum));

            Envelope[] others = new Envelope[] 
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(20, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 200, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 1000, 2000),
                new Envelope(10, 20, 200, 300, 2000, 3000),
                new Envelope(20, 30, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 2000, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsFalse(first.Disjoint(envelope));
                Assert.IsFalse(Envelope.Disjoint(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }


            // true cases

            others = new Envelope[] 
            {
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsTrue(first.Disjoint(envelope));
                Assert.IsTrue(Envelope.Disjoint(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }
        }

        /// <summary>
        /// Test case for the <see cref="Intersects" /> method.
        /// </summary>
        [Test]
        public void EnvelopeIntersectsTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);


            // false cases

            Assert.IsFalse(first.Intersects(null));

            Envelope[] others = new Envelope[] 
            {
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsFalse(first.Intersects(envelope));
                Assert.IsFalse(Envelope.Intersects(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }


            // true cases

            Assert.IsTrue(first.Intersects(first));
            Assert.IsTrue(Envelope.Intersects(first.Minimum, first.Maximum, first.Minimum, first.Maximum));

            others = new Envelope[] 
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(20, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 200, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 1000, 2000),
                new Envelope(10, 20, 200, 300, 2000, 3000),
                new Envelope(20, 30, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 2000, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsTrue(first.Intersects(envelope));
                Assert.IsTrue(Envelope.Intersects(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }
        }

        /// <summary>
        /// Test case for the <see cref="Overlaps" /> method.
        /// </summary>
        [Test]
        public void EnvelopeOverlapsTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);


            // false cases

            Assert.IsFalse(first.Overlaps(null));
            Assert.IsFalse(first.Overlaps(first));
            Assert.IsFalse(Envelope.Overlaps(first.Minimum, first.Maximum, first.Minimum, first.Maximum));

            Envelope[] others = new Envelope[] 
            {
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsFalse(first.Overlaps(envelope));
                Assert.IsFalse(Envelope.Overlaps(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }


            // true cases

            others = new Envelope[] 
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsTrue(first.Overlaps(envelope));
                Assert.IsTrue(Envelope.Overlaps(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }
        }

        /// <summary>
        /// Test case for the <see cref="Touches" /> method.
        /// </summary>
        [Test]
        public void EnvelopeTouchesTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);


            // false cases

            Assert.IsFalse(first.Touches(null));
            Assert.IsFalse(first.Touches(first));
            Assert.IsFalse(Envelope.Touches(first.Minimum, first.Maximum, first.Minimum, first.Maximum));

            Envelope[] others = new Envelope[] 
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsFalse(first.Touches(envelope));
                Assert.IsFalse(Envelope.Touches(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }


            // true cases

            others = new Envelope[] 
            {
                new Envelope(20, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 200, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 1000, 2000),
                new Envelope(10, 20, 200, 300, 2000, 3000),
                new Envelope(20, 30, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 2000, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsTrue(first.Touches(envelope));
                Assert.IsTrue(Envelope.Touches(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }
        }

        /// <summary>
        /// Test case for the <see cref="Within" /> method.
        /// </summary>
        [Test]
        public void EnvelopeWithinTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // false cases

            Assert.IsFalse(first.Within(null));

            Envelope[] others = new Envelope[] 
            {
                new Envelope(11, 19, 101, 199, 1001, 1999),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 19, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 199, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 1999),
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsFalse(first.Within(envelope));
                Assert.IsFalse(Envelope.Within(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }


            // true cases

            Assert.IsTrue(first.Within(first));
            Assert.IsTrue(Envelope.Within(first.Minimum, first.Maximum, first.Minimum, first.Maximum));

            others = new Envelope[] 
            {
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(9, 20, 100, 200, 1000, 2000),
                new Envelope(10, 20, 99, 200, 1000, 2000),
                new Envelope(10, 20, 100, 200, 999, 2000),
                new Envelope(9, 21, 99, 201, 999, 2001)
            };

            foreach (Envelope envelope in others)
            {
                Assert.IsTrue(first.Within(envelope));
                Assert.IsTrue(Envelope.Within(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum));
            }
        }

        /// <summary>
        /// Test case for the <see cref="FromCoordinates" /> method.
        /// </summary>
        [Test]
        public void EnvelopeFromCoordinatesTest()
        {
            Assert.IsFalse(Envelope.FromCoordinates(null).IsValid);
            Assert.IsFalse(Envelope.FromCoordinates((IEnumerable<Coordinate>)null).IsValid);
            Assert.IsFalse(Envelope.FromCoordinates(new Coordinate[] { }).IsValid);

            Coordinate[] source = Enumerable.Range(0, 11).Select(value => new Coordinate(value, value, value)).ToArray();
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 10), Envelope.FromCoordinates(source));
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 10), Envelope.FromCoordinates(source.ToList()));

            source = Enumerable.Range(0, 11).Select(value => new Coordinate(value, 10 - value, Math.Abs(value - 5))).ToArray();
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 5), Envelope.FromCoordinates(source));
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 5), Envelope.FromCoordinates(source.ToList()));

            source = Enumerable.Repeat(0, 11).Select(value => new Coordinate(value, value, value)).ToArray();
            Assert.AreEqual(new Envelope(0, 0, 0, 0, 0, 0), Envelope.FromCoordinates(source));
            Assert.AreEqual(new Envelope(0, 0, 0, 0, 0, 0), Envelope.FromCoordinates(source.ToList()));
        }

        /// <summary>
        /// Test case for the <see cref="FromEnvelopes" /> method.
        /// </summary>
        [Test]
        public void EnvelopeFromEnvelopesTest()
        {
            Assert.IsFalse(Envelope.FromEnvelopes(null).IsValid);
            Assert.IsFalse(Envelope.FromEnvelopes((IEnumerable<Envelope>)null).IsValid);
            Assert.IsFalse(Envelope.FromEnvelopes(new Envelope[] { }).IsValid);

            Envelope[] source = Enumerable.Range(0, 11).Select(value => new Envelope(value, value, value, value, value, value)).ToArray();
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 10), Envelope.FromEnvelopes(source));
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 10), Envelope.FromEnvelopes(source.ToList()));

            source = Enumerable.Range(0, 11).Select(value => new Envelope(value, value, 10 - value, 10 - value, Math.Abs(value - 5), Math.Abs(value - 5))).ToArray();
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 5), Envelope.FromEnvelopes(source));
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 5), Envelope.FromEnvelopes(source.ToList()));

            source = Enumerable.Range(0, 11).Select(value => new Envelope(value, 10 - value, Math.Abs(value - 5), 10 - value, Math.Abs(value - 5), value)).ToArray();
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 10), Envelope.FromEnvelopes(source));
            Assert.AreEqual(new Envelope(0, 10, 0, 10, 0, 10), Envelope.FromEnvelopes(source.ToList()));

            source = Enumerable.Repeat(0, 11).Select(value => new Envelope(value, value, value, value, value, value)).ToArray();
            Assert.AreEqual(new Envelope(0, 0, 0, 0, 0, 0), Envelope.FromEnvelopes(source));
            Assert.AreEqual(new Envelope(0, 0, 0, 0, 0, 0), Envelope.FromEnvelopes(source.ToList()));
        }

        #endregion
    }
}
