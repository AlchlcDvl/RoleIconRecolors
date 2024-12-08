using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FancyUI.UI;

public class HoverEffect : TooltipTrigger
{
    private Button.ButtonClickedEvent OnMouseOver { get; set; }
    private Button.ButtonClickedEvent OnMouseOut { get; set; }
    public List<(string Key, string Value)> FillInKeys { get; set; }

    public void Awake()
    {
        OnMouseOver = new();
        OnMouseOut = new();
    }

    public void AddOnOverListener(UnityAction listener) => OnMouseOver.AddListener(listener);

    public void AddOnOutListener(UnityAction listener) => OnMouseOut.AddListener(listener);

    public void AddOnOverListeners(params UnityAction[] listeners) => listeners.ForEach(OnMouseOver.AddListener);

    public void AddOnOutListeners(params UnityAction[] listeners) => listeners.ForEach(OnMouseOut.AddListener);

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        OnMouseOver.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        OnMouseOut.Invoke();
    }

    public override void StartHover(GameObject targetObject)
    {
        if (!TooltipView.Instance(TargetTooltipView))
            return;

        TooltipView.Instance(TargetTooltipView).TargetObject = targetObject;
        var text = string.IsNullOrEmpty(NonLocalizedString) ? l10n(LookupKey) : NonLocalizedString;
        FillInKeys?.ForEach(x => text = text.Replace(x.Key, x.Value));
        var flag = false;

        if (OverrideCameraFromRootCanvas)
        {
            var canvas = transform.GetComponentInParent<Canvas>();

            if (canvas)
                canvas = canvas.rootCanvas;

            if (canvas.isRootCanvas)
            {
                flag = canvas.renderMode == RenderMode.ScreenSpaceCamera;

                if (flag)
                    TargetTooltipView.ParentRectTransform = canvas.GetComponent<RectTransform>();
            }
        }

        TooltipView.Instance(TargetTooltipView).Show(text, flag);
    }
}