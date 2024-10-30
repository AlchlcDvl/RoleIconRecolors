using UnityEngine.Events;
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

    public void AddOnOverListener(UnityAction listener) => OnMouseOver.AddListener(listener);

    public void AddOnOutListener(UnityAction listener) => OnMouseOut.AddListener(listener);

    public void AddOnOverListeners(params UnityAction[] listeners) => listeners.ForEach(OnMouseOver.AddListener);

    public void AddOnOutListeners(params UnityAction[] listeners) => listeners.ForEach(OnMouseOut.AddListener);

    public void OnPointerEnter(PointerEventData eventData) => OnMouseOver.Invoke();

    public void OnPointerExit(PointerEventData eventData) => OnMouseOut.Invoke();
}