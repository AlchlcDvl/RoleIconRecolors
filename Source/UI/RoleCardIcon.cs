namespace FancyUI.UI;

public class RoleCardIcon : UIController
{
    public Image Background { get; set; }
    public Image Icon { get; set; }
    public IconType Type { get; set; }
    public Role Role { get; set; }

    public void Awake()
    {
        Background = GetComponent<Image>();
        Icon = GetComponentInChildren<Image>();
    }

    public void UpdateIcon(Role role)
    {
        Role = role;
        Background.SetImageColor(ColorType.Metal);
    }
}