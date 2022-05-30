/// <copyright file="Segment.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roland Krisztandl</author>

using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELTE.AEGIS.LiDAR.Operations
{
    /// <summary>
    /// Determines the type of the segment.
    /// </summary>
    public enum SegmentType { INSIDE, OUTSIDE }

    /// <summary>
    /// Implements a method that returns a part of the point cloud.
    /// Inside: every point within the envelope.
    /// Outside: every point outside the envelope.
    /// </summary>
    public class Segment
    {
        PointOctree<LasPointBase> octree;
        Envelope envelope;
        SegmentType type;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="envelope">The envelope used to create a segment.</param>
        /// <param name="type">The type of the segment.</param>
        public Segment(ref PointOctree<LasPointBase> octree, Envelope envelope, SegmentType type)
        {
            this.octree = octree;
            this.envelope = envelope;
            this.type = type;
        }

        /// <summary>
        /// Executes method.
        /// </summary>
        /// <returns>The points located in the segment.</returns>
        public List<PointQuadTree<LasPointBase>.TreeObject> Execute()
        {
            if (type == SegmentType.INSIDE)
            {
                return octree.SearchWithCoords(envelope);
            }
            else //if(type == SegmentType.OUTSIDE)
            {
                List<PointQuadTree<LasPointBase>.TreeObject> outside = new List<PointQuadTree<LasPointBase>.TreeObject>();
                foreach(var obj in octree.GetAllWithCoords())
                {
                    if (!envelope.Contains(obj.Point))
                        outside.Add(obj);
                }
                return outside;
            }
        }

        /// <summary>
        /// Executes method.
        /// </summary>
        /// <returns>The points located in the segment.</returns>
        public async Task<List<PointQuadTree<LasPointBase>.TreeObject>> ExecuteAsync() => await Task.Run(() => Execute());
    }
}
