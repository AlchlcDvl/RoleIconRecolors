namespace FancyUI.UI;

public sealed class GradientRoleColorController : MonoBehaviour
{
    private const float Duration = 10f;

    public RoleCardPanelBackground Instance;

    private Coroutine ActiveCoroutine;

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
                if (Instance?.rolecardBackgroundInstance)
                {
                    var col = grad.Evaluate(t);
                    Instance.rolecardBackgroundInstance.SetColor(col, col);
                }
            });
            yield return Coroutines.CoPerformTimedAction(Duration, t =>
            {
                if (Instance?.rolecardBackgroundInstance)
                {
                    var col = grad.Evaluate(1f - t);
                    Instance.rolecardBackgroundInstance.SetColor(col, col);
                }
            });
        }
    }
}