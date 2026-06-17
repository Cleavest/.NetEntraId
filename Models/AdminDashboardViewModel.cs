namespace AdminPanel.Models;

public class AdminDashboardViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<CmsLink> CmsLinks { get; set; } = new();
}
