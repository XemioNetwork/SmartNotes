using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Client.Data
{
    public class Session
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User CurrentUser { get; set; }
    }
}