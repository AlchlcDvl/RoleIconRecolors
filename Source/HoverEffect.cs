using UnityEngine.EventSystems;

namespace IconPacks;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
    public Button.ButtonClickedEvent OnMouseOver;
    public Button.ButtonClickedEvent OnMouseOut;

    public void Aawake()
    {
        OnMouseOver = new();
        OnMouseOut = new();
    }

    public void OnPointerEnter(PointerEventData eventData) => OnMouseOver.Invoke();

    public void OnPointerExit(PointerEventData eventData) => OnMouseOut.Invoke();
}