namespace FoxtaurTracker.Models
{
    /// <summary>
    /// User and hir credentials
    /// </summary>
    public class User
    {
        /// <summary>
        /// Is user logged in?
        /// </summary>
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// User login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Security token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Security token expiration time
        /// </summary>
        public DateTime TokenExpirationTime { get; set; }
    }
}
