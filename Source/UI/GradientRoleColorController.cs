namespace FancyUI.UI;
public class GradientRoleColorController : MonoBehaviour
{
    public RoleCardPanelBackground __instance;

    private Coroutine activeCoroutine;
    private readonly float duration = 10f;
    private float value = 0f;

    private void Start()
    {
        activeCoroutine = StartCoroutine(ChangeValueOverTime(__instance.currentFaction, __instance.currentRole));
    }

    private void OnDestroy()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }

    private IEnumerator ChangeValueOverTime(FactionType faction, Role role)
    {
        var grad = faction.GetChangedGradient(role);

        if (grad == null)
        {
            yield break;
        }

        while (true)
        {
            for (var t = 0f; t < duration; t += Time.deltaTime)
            {
                value = Mathf.Lerp(0f, 1f, t / duration);
                if (__instance != null && __instance.rolecardBackgroundInstance != null)
                {
                    __instance.rolecardBackgroundInstance.SetColor(grad.Evaluate(value));
                }
                yield return null;
            }

            for (var t2 = 0f; t2 < duration; t2 += Time.deltaTime)
            {
                value = Mathf.Lerp(1f, 0f, t2 / duration);
                if (__instance != null && __instance.rolecardBackgroundInstance != null)
                {
                    __instance.rolecardBackgroundInstance.SetColor(grad.Evaluate(value));
                }
                yield return null;
            }
        }
    }
}
