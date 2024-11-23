using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FancyUI.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button.ButtonClickedEvent OnMouseOver { get; set; }
    public Button.ButtonClickedEvent OnMouseOut { get; set; }

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