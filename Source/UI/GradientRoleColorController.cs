namespace FancyUI.UI;
public class GradientRoleColorController : MonoBehaviour
{
    public RoleCardPanelBackground Instance;

    private Coroutine ActiveCoroutine;
    private readonly float Duration = 10f;
    private float Value;

    public void Start() => ActiveCoroutine = StartCoroutine(ChangeValueOverTime(Instance.currentFaction, Instance.currentRole));

    public void OnDestroy()
    {
        if (ActiveCoroutine == null)
            return;

        StopCoroutine(ActiveCoroutine);
        ActiveCoroutine = null;
    }

    private IEnumerator ChangeValueOverTime(FactionType faction, Role role)
    {
        var grad = faction.GetChangedGradient(role);

        if (grad == null)
            yield break;

        while (true)
        {
            yield return Coroutines.CoPerformTimedAction(Duration, t =>
            {
                Value = Mathf.Lerp(0f, 1f, t);

                if (Instance?.rolecardBackgroundInstance)
                    Instance.rolecardBackgroundInstance.SetColor(grad.Evaluate(Value));
            });
            yield return Coroutines.CoPerformTimedAction(Duration, t =>
            {
                Value = Mathf.Lerp(1f, 0f, t);

                if (Instance?.rolecardBackgroundInstance)
                    Instance.rolecardBackgroundInstance.SetColor(grad.Evaluate(Value));
            });
        }
    }
}