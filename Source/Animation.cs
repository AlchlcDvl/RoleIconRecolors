using UnityEngine.UI;

namespace IconPacks;

public class Animation : MonoBehaviour
{
    public List<Sprite> Frames;
    public Image Render;
    private int Index;

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
}