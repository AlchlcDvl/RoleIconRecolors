namespace FancyUI.UI;

public class SilhouetteSetTestingUI : MonoBehaviour
{
    public static SilhouetteSetTestingUI Instance { get; private set; }

    private CustomAnimator Animator;

    public void Awake()
    {
        Instance = this;

        Animator = transform.Find("Animator").AddComponent<CustomAnimator>();

        SetupMenu();
    }

    private void SetupMenu()
    {
        Animator.SetAnim(Loading.Frames, Constants.AnimationDuration());
        Animator.AddComponent<TooltipTrigger>().NonLocalizedString = "This Is Your Animator";
    }
}