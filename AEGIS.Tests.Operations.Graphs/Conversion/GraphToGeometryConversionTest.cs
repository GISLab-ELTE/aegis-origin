﻿/// <copyright file="GraphToGeometryConversionTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Metadata;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Conversion;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spatial.Conversion
{
    [TestFixture]
    public class GraphToGeometryConversionTest
    {
        private Mock<IReferenceSystem> _referenceSystemMock;

        [SetUp]
        public void SetUp()
        {
            Guid guid = Guid.NewGuid();

            _referenceSystemMock = new Mock<IReferenceSystem>(MockBehavior.Strict);
        }


        [TestCase]
        public void GraphToGeometryConversionPointsTest()
        {
            Random random = new Random();

            List<Coordinate> coordinates = new List<Coordinate>();
            
            for (Int32 i = 0; i < 100; i++)
                coordinates.Add(new Coordinate(i, i));

            GeometryGraph graph = new GeometryGraph(_referenceSystemMock.Object, null);
            coordinates.ForEach(coordinate => graph.AddVertex(coordinate));

            // test case 1: without metadata

            Dictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, object>();
            parameters[ConversionParameters.GeometryDimension] = 0;

            GraphToGeometryConversion conversion = new GraphToGeometryConversion(graph, parameters);

            conversion.Execute();

            IGeometry result = conversion.Result;

            Assert.IsTrue(result is IMultiPoint);
            Assert.AreEqual(result.ReferenceSystem, graph.ReferenceSystem);
            Assert.AreEqual(coordinates.Count, (result as IMultiPoint).Count);

            foreach (IPoint point in result as IMultiPoint)
                Assert.IsTrue(coordinates.Contains(point.Coordinate));

            // test case 2: with metadata

            graph = new GeometryGraph(_referenceSystemMock.Object, null);
            coordinates.ForEach(coordinate =>
                {
                    IMetadataCollection metadata = new MetadataCollection();
                    metadata["CoordinateX"] = coordinate.X;

                    graph.AddVertex(coordinate, metadata);
                });

            parameters = new Dictionary<OperationParameter, object>();
            parameters[ConversionParameters.GeometryDimension] = 0;
            parameters[OperationParameters.MetadataPreservation] = true;

            conversion = new GraphToGeometryConversion(graph, parameters);

            conversion.Execute();

            result = conversion.Result;

            Assert.IsTrue(result is IMultiPoint);
            Assert.AreSame(result.ReferenceSystem, graph.ReferenceSystem);
            Assert.AreEqual(coordinates.Count, (result as IMultiPoint).Count);

            foreach (IPoint point in result as IMultiPoint)
            {
                Assert.IsTrue(coordinates.Contains(point.Coordinate));
                Assert.IsTrue(coordinates.First(c => c.Equals(point.Coordinate)).X.Equals(Convert.ToDouble(point["CoordinateX"])));
            }
        }

        [TestCase]
        public void GraphToGeometryConversionLinesTest()
        {
            Random random = new Random();

            List<Coordinate> coordinates = new List<Coordinate>();

            for (Int32 i = 0; i < 100; i++)
                coordinates.Add(new Coordinate(i, i));

            GeometryGraph graph = new GeometryGraph(_referenceSystemMock.Object, null);

            for (Int32 i = 0; i < coordinates.Count; i++)
            {
                graph.AddVertex(coordinates[i]);
                if (i > 0)
                    graph.AddEdge(graph.GetVertex(coordinates[i - 1]), graph.GetVertex(coordinates[i]));
            }

            // test case 1: only points

            Dictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, object>();
            parameters[ConversionParameters.GeometryDimension] = 0;

            GraphToGeometryConversion conversion = new GraphToGeometryConversion(graph, parameters);

            conversion.Execute();

            IGeometry result = conversion.Result;

            Assert.IsTrue(result is IMultiPoint);
            Assert.AreSame(result.ReferenceSystem, graph.ReferenceSystem);
            Assert.AreEqual(coordinates.Count, (result as IMultiPoint).Count);

            foreach (IPoint point in result as IMultiPoint)
                Assert.IsTrue(coordinates.Contains(point.Coordinate));

            // test case 2: only lines

            parameters = new Dictionary<OperationParameter, object>();
            parameters[ConversionParameters.GeometryDimension] = 1;

            conversion = new GraphToGeometryConversion(graph, parameters);

            conversion.Execute();

            result = conversion.Result;

            Assert.IsTrue(result is IMultiLineString);
            Assert.AreEqual(coordinates.Count - 1, (result as IMultiLineString).Count);

            foreach (ILineString lineString in result as IMultiLineString)
            {
                Assert.IsTrue(lineString is ILine);
                Assert.IsTrue(coordinates.Contains(lineString.StartCoordinate));
                Assert.IsTrue(coordinates.Contains(lineString.EndCoordinate));
            }

            // test case 3: points and lines

            List<Coordinate> newCoordinates = new List<Coordinate>();
            for (Int32 i = 100; i < 200; i++)
                newCoordinates.Add(new Coordinate(i, i));                 
            newCoordinates.ForEach(coordinate => graph.AddVertex(coordinate));

            parameters = new Dictionary<OperationParameter, object>();
            parameters[ConversionParameters.GeometryDimension] = 1;

            conversion = new GraphToGeometryConversion(graph, parameters);

            conversion.Execute();

            result = conversion.Result;

            Assert.IsTrue(result is IGeometryCollection);
            Assert.AreEqual(coordinates.Count - 1 + newCoordinates.Count, (result as IGeometryCollection).Count);

            foreach (IGeometry geometry in result as IGeometryCollection)
            {
                Assert.IsTrue(geometry is ILine || geometry is IPoint);

                if (geometry is ILine)
                {
                    Assert.IsTrue(coordinates.Contains((geometry as ILine).StartCoordinate));
                    Assert.IsTrue(coordinates.Contains((geometry as ILine).EndCoordinate));
                }
                if (geometry is IPoint)
                {
                    Assert.IsTrue(newCoordinates.Contains((geometry as IPoint).Coordinate));
                }
            }
        }

        [TestCase]
        public void GraphToGeometryConversionPolygonsTest()
        {
            // test case 1: single polygon

            Coordinate[] coordinates = new Coordinate[4];
            coordinates[0] = new Coordinate(10, 10);
            coordinates[1] = new Coordinate(20, 10);
            coordinates[2] = new Coordinate(20, 20);
            coordinates[3] = new Coordinate(10, 20);

            GeometryNetwork graph = new GeometryNetwork(_referenceSystemMock.Object, null);

            graph.AddVertex(coordinates[0]);

            for (Int32 i = 1; i < coordinates.Length; i++)
            {
                graph.AddVertex(coordinates[i]);
                graph.AddEdge(graph.GetVertex(coordinates[i - 1]), graph.GetVertex(coordinates[i]));
                graph.AddEdge(graph.GetVertex(coordinates[i]), graph.GetVertex(coordinates[i - 1]));
            }

            graph.AddEdge(graph.GetVertex(coordinates[coordinates.Length - 1]), graph.GetVertex(coordinates[0]));
            graph.AddEdge(graph.GetVertex(coordinates[0]), graph.GetVertex(coordinates[coordinates.Length - 1]));

            Dictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, object>();
            parameters[ConversionParameters.GeometryDimension] = 2;

            GraphToGeometryConversion conversion = new GraphToGeometryConversion(graph, parameters);

            conversion.Execute();

            IGeometry result = conversion.Result;

            Assert.IsTrue(result is IPolygon);
            Assert.AreSame(result.ReferenceSystem, graph.ReferenceSystem);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(coordinates.Length + 1, (result as IPolygon).Shell.Count);

            for (Int32 i = 0; i < coordinates.Length; i++)
                Assert.IsTrue((result as IPolygon).Shell.Coordinates.Contains(coordinates[i]));

            // test case 2: multiple polygons

            coordinates = new Coordinate[4];
            coordinates[0] = new Coordinate(110, 110);
            coordinates[1] = new Coordinate(120, 110);
            coordinates[2] = new Coordinate(120, 120);
            coordinates[3] = new Coordinate(110, 120);

            graph.AddVertex(coordinates[0]);

            for (Int32 i = 1; i < coordinates.Length; i++)
            {
                graph.AddVertex(coordinates[i]);
                graph.AddEdge(graph.GetVertex(coordinates[i - 1]), graph.GetVertex(coordinates[i]));
                graph.AddEdge(graph.GetVertex(coordinates[i]), graph.GetVertex(coordinates[i - 1]));
            }

            graph.AddEdge(graph.GetVertex(coordinates[coordinates.Length - 1]), graph.GetVertex(coordinates[0]));
            graph.AddEdge(graph.GetVertex(coordinates[0]), graph.GetVertex(coordinates[coordinates.Length - 1]));

            conversion.Execute();

            result = conversion.Result;

            Assert.IsTrue(result is IMultiPolygon);
            Assert.IsTrue((result as IMultiPolygon).Count == 2);

            // test case 3: touching polygons

            coordinates = new Coordinate[3];
            coordinates[0] = new Coordinate(10, 20);
            coordinates[1] = new Coordinate(20, 20);
            coordinates[2] = new Coordinate(15, 25);

            graph.AddVertex(coordinates[0]);

            for (Int32 i = 1; i < coordinates.Length; i++)
            {
                graph.AddVertex(coordinates[i]);
                graph.AddEdge(graph.GetVertex(coordinates[i - 1]), graph.GetVertex(coordinates[i]));
                graph.AddEdge(graph.GetVertex(coordinates[i]), graph.GetVertex(coordinates[i - 1]));
            }

            graph.AddEdge(graph.GetVertex(coordinates[coordinates.Length - 1]), graph.GetVertex(coordinates[0]));
            graph.AddEdge(graph.GetVertex(coordinates[0]), graph.GetVertex(coordinates[coordinates.Length - 1]));

            conversion.Execute();

            result = conversion.Result;

            Assert.IsTrue(result is IMultiPolygon);
            Assert.IsTrue((result as IMultiPolygon).Count == 3);

            // test case 4: arbitary shape polygon

            coordinates = new Coordinate[7];
            coordinates[0] = new Coordinate(10, 10);
            coordinates[1] = new Coordinate(20, 5);
            coordinates[2] = new Coordinate(30, 20);
            coordinates[3] = new Coordinate(20, 30);
            coordinates[4] = new Coordinate(20, 20);
            coordinates[5] = new Coordinate(10, 30);
            coordinates[6] = new Coordinate(0, 20);

            graph = new GeometryNetwork(_referenceSystemMock.Object, null);

            graph.AddVertex(coordinates[0]);

            for (Int32 i = 1; i < coordinates.Length; i++)
            {
                graph.AddVertex(coordinates[i]);
                graph.AddEdge(graph.GetVertex(coordinates[i - 1]), graph.GetVertex(coordinates[i]));
                graph.AddEdge(graph.GetVertex(coordinates[i]), graph.GetVertex(coordinates[i - 1]));
            }

            graph.AddEdge(graph.GetVertex(coordinates[coordinates.Length - 1]), graph.GetVertex(coordinates[0]));
            graph.AddEdge(graph.GetVertex(coordinates[0]), graph.GetVertex(coordinates[coordinates.Length - 1]));

            conversion = new GraphToGeometryConversion(graph, parameters);
            conversion.Execute();

            result = conversion.Result;

            Assert.IsTrue(result is IPolygon);            
            Assert.AreEqual(coordinates.Length + 1, (result as IPolygon).Shell.Count);

            for (Int32 i = 0; i < coordinates.Length; i++)
                Assert.IsTrue((result as IPolygon).Shell.Coordinates.Contains(coordinates[i]));
        }
    }
}
