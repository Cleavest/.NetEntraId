namespace AdminPanel.Models;

public class LocalUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Viewer";
    public string Email { get; set; } = string.Empty;
    public List<string> AllowedCms { get; set; } = new();
}

public static class LocalUserStore
{
    public static List<LocalUser> Users { get; } = new()
    {
        new LocalUser
        {
            Username = "admin",
            Password = "admin123",
            Role = "Admin",
            Email = "admin@local",
            AllowedCms = new List<string> { "WordPress CMS", "Umbraco CMS", "Strapi CMS", "Sitecore CMS" }
        },
        new LocalUser
        {
            Username = "editor",
            Password = "editor123",
            Role = "Editor",
            Email = "editor@local",
            AllowedCms = new List<string> { "WordPress CMS", "Umbraco CMS" }
        },
        new LocalUser
        {
            Username = "viewer",
            Password = "viewer123",
            Role = "Viewer",
            Email = "viewer@local",
            AllowedCms = new List<string>()
        }
    };
}
