namespace AdminPanel.Models;

public class CmsLink
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool SsoEnabled { get; set; }
}

public class CmsLinksOptions
{
    public List<CmsLink> Links { get; set; } = new();
}
