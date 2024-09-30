using UnityEngine.EventSystems;

namespace FancyUI.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
    public Button.ButtonClickedEvent OnMouseOver;
    public Button.ButtonClickedEvent OnMouseOut;

    public void Awake()
    {
        OnMouseOver = new();
        OnMouseOut = new();
    }

    public void OnPointerEnter(PointerEventData eventData) => OnMouseOver.Invoke();

    public void OnPointerExit(PointerEventData eventData) => OnMouseOut.Invoke();
}