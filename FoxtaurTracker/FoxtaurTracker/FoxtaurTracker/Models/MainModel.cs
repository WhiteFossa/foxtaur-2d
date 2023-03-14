namespace FoxtaurTracker.Models
{
    /// <summary>
    /// Root model for everything
    /// </summary>
    public class MainModel
    {
        /// <summary>
        /// User with information about hir
        /// </summary>
        public User User { get; set; } = new User();
    }
}
