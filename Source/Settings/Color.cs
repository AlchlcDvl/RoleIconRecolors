using UnityEngine;
using UnityEngine.UI;

namespace FancyUI.Settings
{
    public class ColorSetting : BaseInputSetting
    {
        public Image ValueBg { get; set; }

        public override void Awake()
        {
            base.Awake();
            ValueBg = Input.transform.GetComponent<Image>();
        }

        public override void Refresh()
        {
            base.Refresh();

            if (BoxedOption is ColorOption colorOption &&
                ColorUtility.TryParseHtmlString(colorOption.ValueString, out var color))
            {
                ValueBg.color = color;
                Input.SetTextWithoutNotify(colorOption.ValueString);
            }
        }
    }
}
