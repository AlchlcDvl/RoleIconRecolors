namespace FancyUI.UI;

public sealed class SlotCounterHandler : MonoBehaviour
{
    public readonly List<Transform> Counters = [];

    public void Awake() => Counters.Add(transform.Find("SlotCounter1"), transform.Find("SlotCounter2"), transform.Find("SlotCounter3"), transform.Find("SlotCounter4"));

    public IEnumerable<Image> GetIconBGs()
    {
        foreach (var counter in Counters)
        {
            var bg = counter.FindRecursive("IconBG");

            if (bg)
            {
                yield return bg.GetComponent<Image>();
                continue;
            }

            for (var i = 1; i < 5; i++)
            {
                bg = counter.FindRecursive($"IconBG{i}");

                if (bg)
                    yield return bg.GetComponent<Image>();
            }
        }
    }
}