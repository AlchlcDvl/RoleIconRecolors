using UnityEngine.UI;

namespace IconPacks;

public class Animation : MonoBehaviour
{
    public List<Sprite> Frames { get; set; }
    public Image Render { get; set; }
    private float TimePassed;
    private Sprite InitSprite { get; set; }

    public void Start() => Frames = new();

    public void SetFrames(Image render, List<Sprite> frames)
    {
        InitSprite = render.sprite;
        Render = render;
        Frames = frames;
        TimePassed = 0;

        if (Frames.Count > 0)
            Render.sprite = Frames[0];
    }

    public void RemoveFrames()
    {
        if (Render)
            Render.sprite = InitSprite;

        InitSprite = null;
        Render = null;
        Frames.Clear();
        TimePassed = 0;
    }

    public void Update()
    {
        Recolors.LogMessage("1");

        if (!Render)
            Render = gameObject?.GetComponent<Image>();

        Recolors.LogMessage("2");

        if (!Render || Frames.Count == 0)
            return;

        Recolors.LogMessage("3");
        Utils.Cycle(ref TimePassed, 0, Constants.AnimationDuration, true, Time.deltaTime);
        var index = (int)(Frames.Count * (TimePassed / Constants.AnimationDuration));
        Render.sprite = Frames[index];
        Recolors.LogMessage("4");
    }
}