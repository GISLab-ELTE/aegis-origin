
namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Defines the domains of spectral operations.
    /// </summary>
    public enum SpectralOperationDomain
    {
        /// <summary>
        /// Operates on distinct spectral values within the raster band.
        /// </summary>
        BandLocal,
        /// <summary>
        /// Operates on distinct spectral values by using neightbour values within the raster band.
        /// </summary>
        BandFocal,
        /// <summary>
        /// Operates on distinct parts of the raster band.
        /// </summary>
        BandZonal,
        /// <summary>
        /// Operates on the entire raster band.
        /// </summary>
        BandGlobal,
        /// <summary>
        /// Operates on distinct spectral values within the raster.
        /// </summary>
        Local,
        /// <summary>
        /// Operates on distinct spectral values by using neightbour values within the raster.
        /// </summary>
        Focal,
        /// <summary>
        /// Operates on distinct parts of the raster.
        /// </summary>
        Zonal,
        /// <summary>
        /// Operates on the entire raster.
        /// </summary>
        Global
    }
}
