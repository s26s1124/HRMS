using static HumanResource.Helpers.AuthHelper;

namespace HumanResource.Helpers
{
    public class Navigation
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public List<Link> Links { get; set; }

        // Additional properties
        public string Icon { get; set; } // If you want to assign an icon for each navigation item

        public Navigation()
        {
            Links = new List<Link>();
        }

    }

    public class Link
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }

        // Additional properties
        public string Target { get; set; } // For example "_blank" to open in a new tab, "_self" for the same tab
    }
}