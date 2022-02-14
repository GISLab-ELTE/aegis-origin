/// <copyright file="Angles.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of the known geographic angles.
    /// </summary>
    public static class Angles
    {
        #region Private static fields

        private static Angle? _equator;
        private static Angle? _northPole;
        private static Angle? _southPole;
        private static Angle? _arcticCircle;
        private static Angle? _antarcticCircle;
        private static Angle? _tropicOfCancer;
        private static Angle? _tropicOfCapricorn;

        #endregion

        #region Public static properties

        /// <summary>
        /// The equator.
        /// </summary>
        public static Angle Equator { get { return (_equator ?? (_equator = Angle.FromDegree(0))).Value; } }

        /// <summary>
        /// The north pole.
        /// </summary>
        public static Angle NorthPole { get { return (_northPole ?? (_northPole = Angle.FromDegree(90))).Value; } }

        /// <summary>
        /// The south pole.
        /// </summary>
        public static Angle SouthPole { get { return (_southPole ?? (_southPole = Angle.FromDegree(-90))).Value; } }

        /// <summary>
        /// The arctic circle.
        /// </summary>
        public static Angle ArcticCircle { get { return (_arcticCircle ?? (_arcticCircle = Angle.FromDegree(66.5622))).Value; } }

        /// <summary>
        /// The antarctic circle.
        /// </summary>
        public static Angle AntarcticCircle { get { return (_antarcticCircle ?? (_antarcticCircle = Angle.FromDegree(-66.5622))).Value; } }

        /// <summary>
        /// The tropic of cancer.
        /// </summary>
        public static Angle TropicOfCancer { get { return (_tropicOfCancer ?? (_tropicOfCancer = Angle.FromDegree(23.43777778))).Value; } }

        /// <summary>
        /// The tropic of Capricorn.
        /// </summary>
        public static Angle TropicOfCapricorn { get { return (_tropicOfCapricorn ?? (_tropicOfCapricorn = Angle.FromDegree(-23.43777778))).Value; } }

        #endregion
    }
}
