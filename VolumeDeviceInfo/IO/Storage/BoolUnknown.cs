namespace RJCP.IO.Storage
{
    /// <summary>
    /// A tri-state boolean value.
    /// </summary>
    public enum BoolUnknown
    {
        /// <summary>
        /// False.
        /// </summary>
        False = 0,

        /// <summary>
        /// True.
        /// </summary>
        True = -1,

        /// <summary>
        /// Unkown, can either be <see cref="True"/> or <see cref="False"/>.
        /// </summary>
        Unknown = -2
    }
}
