namespace FancyUI.UI;

public class RoleCardIcon : UIController
{
    public Image Background { get; set; }
    public Image Icon { get; set; }

    public void Awake()
    {
        Background = GetComponent<Image>();
        Icon = GetComponentInChildren<Image>();
    }
}