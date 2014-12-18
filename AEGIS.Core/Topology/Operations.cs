using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Topology
{
    public static class Operations
    {
        //private static HalfedgeGraph Merge(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        //{
        //    HalfedgeGraph graphA = new HalfedgeGraph();
        //    foreach (var geometry in geometriesA)
        //        graphA.MergeGeometry(geometry);

        //    HalfedgeGraph graphB = new HalfedgeGraph();
        //    foreach (var geometry in geometriesB)
        //        graphB.MergeGeometry(geometry);

        //    graphA.MergeGraph(graphB);
        //    return graphA;
        //}

        //public static Boolean Equals(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        //{
        //    HalfedgeGraph graph = Merge(geometriesA, geometriesB);

        //    if (graph.Faces.Any(face => !face.Tag.HasFlag(Tag.A) || !face.Tag.HasFlag(Tag.B)))
        //        return false;
        //    if (graph.Edges.Any(edge => !edge.Tag.HasFlag(Tag.A) || !edge.Tag.HasFlag(Tag.B)))
        //        return false;
        //    if (graph.Vertices.Any(vertex => !vertex.Tag.HasFlag(Tag.A) || !vertex.Tag.HasFlag(Tag.B)))
        //        return false;
        //    return true;
        //}

        //public static Boolean Disjoint(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        //{
        //    HalfedgeGraph graph = Merge(geometriesA, geometriesB);

        //    if (graph.Faces.Any(face => face.Tag.HasFlag(Tag.A) && face.Tag.HasFlag(Tag.B)))
        //        return false;
        //    if (graph.Edges.Any(edge => edge.Tag.HasFlag(Tag.A) && edge.Tag.HasFlag(Tag.B)))
        //        return false;
        //    if (graph.Vertices.Any(vertex => vertex.Tag.HasFlag(Tag.A) && vertex.Tag.HasFlag(Tag.B)))
        //        return false;
        //    return true;
        //}

        //public static Boolean Touches(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        //{
        //    HalfedgeGraph graph = Merge(geometriesA, geometriesB);

        //    if (graph.Faces.Any(face => face.Tag.HasFlag(Tag.A) && face.Tag.HasFlag(Tag.B)))
        //        return false;
        //    if (graph.Edges.Any(edge => edge.Tag.HasFlag(Tag.A) && edge.Tag.HasFlag(Tag.B)))
        //        return true;
        //    if (graph.Vertices.Any(vertex => vertex.Tag.HasFlag(Tag.A) && vertex.Tag.HasFlag(Tag.B)))
        //        return true;
        //    return false;
        //}

        //public static Boolean Contains(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        //{
        //    HalfedgeGraph graph = Merge(geometriesA, geometriesB);

        //    if (graph.Faces.Any(face => !face.Tag.HasFlag(Tag.B) || face.Tag.HasFlag(Tag.A)))
        //        return false;
        //    if (graph.Edges.Any(edge => !edge.Tag.HasFlag(Tag.B) || edge.Tag.HasFlag(Tag.A)))
        //        return false;
        //    if (graph.Vertices.Any(vertex => !vertex.Tag.HasFlag(Tag.B) || vertex.Tag.HasFlag(Tag.A)))
        //        return false;
        //    if (graph.Faces.Any(face => face.Tag.HasFlag(Tag.A) && !face.Tag.HasFlag(Tag.B)))
        //        return true;
        //    return false;
        //}

        //public static Boolean Covers(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        //{
        //    HalfedgeGraph graph = Merge(geometriesA, geometriesB);

        //    if (graph.Faces.Any(face => !face.Tag.HasFlag(Tag.B) || face.Tag.HasFlag(Tag.A)))
        //        return false;
        //    if (graph.Edges.Any(edge => !edge.Tag.HasFlag(Tag.B) || edge.Tag.HasFlag(Tag.A)))
        //        return false;
        //    if (graph.Vertices.Any(vertex => !vertex.Tag.HasFlag(Tag.B) || vertex.Tag.HasFlag(Tag.A)))
        //        return false;
        //    return true;
        //}

        //public static Boolean Overlaps(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        //{
        //    HalfedgeGraph graph = Merge(geometriesA, geometriesB);

        //    if (graph.Vertices.All(vertex => vertex.Tag == Tag.None || vertex.Tag == Tag.Both))
        //        return false;
        //    if (graph.Vertices.Any(vertex => vertex.Tag == Tag.A) && graph.Vertices.Any(vertex => vertex.Tag == Tag.B))
        //        return true;
        //    return false;
        //}

        //public static IGeometry[] Union(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        //{
        //    HalfedgeGraph graph = Merge(geometriesA, geometriesB);
        //    return graph.ToGeometry();
        //}

        /*
        public static IGeometry[] Intersection(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        {
            Graph graph = Merge(geometriesA, geometriesB);
            return graph.Faces.Where(face => !face.IsHole && face.Tag == Tag.Both).Select(face => face.ToGeometry()).ToArray();
        }

        public static IGeometry[] Subtraction(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        {
            Graph graph = Merge(geometriesA, geometriesB);
            return graph.Faces.Where(face => !face.IsHole && face.Tag == Tag.A).Select(face => face.ToGeometry()).ToArray();
        }

        public static IGeometry[] SymmetricDifference(IEnumerable<IGeometry> geometriesA, IEnumerable<IGeometry> geometriesB)
        {
            Graph graph = Merge(geometriesA, geometriesB);
            return graph.Faces.Where(face => !face.IsHole && (face.Tag == Tag.A || face.Tag == Tag.B)).Select(face => face.ToGeometry()).ToArray();
        }
        */
    }
}
