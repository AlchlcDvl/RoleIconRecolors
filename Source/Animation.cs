/*using UnityEngine.UI;

namespace IconPacks;

public class Animation : MonoBehaviour
{
    public List<Sprite> Frames { get; set; }
    public Image Render { get; set; }
    private int Index { get; set; }

    public void Start()
    {
        Frames = new();
        Index = 0;
    }

    public void SetFrames(Image render, List<Sprite> frames)
    {
        Render = render;
        Frames = frames;
        Index = 0;

        if (Frames.Count > 0)
            Render.sprite = Frames[0];
    }

    public void Update()
    {
        if (!Render)
        {
            Render = gameObject.GetComponent<Image>();

            if (!Render)
                return;
        }

        if (Frames.Count == 0)
            return;

        Render.sprite = Frames[Index];
        Index++;

        if (Index >= Frames.Count)
            Index = 0;
    }
}*/