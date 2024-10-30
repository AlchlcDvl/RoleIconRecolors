namespace FancyUI.UI;

public class SilhouetteSetTestingUI : UIController
{
    public static SilhouetteSetTestingUI Instance { get; private set; }

    private CustomAnimator Animator;

    private GameObject Back;

    public void Awake()
    {
        Instance = this;

        Animator = transform.Find("Animator").AddComponent<CustomAnimator>();
        Back = transform.Find("Back").gameObject;

        SetupMenu();
    }

    private void SetupMenu()
    {
        Back.GetComponent<Button>().onClick.AddListener(GoBack);
        Back.AddComponent<TooltipTrigger>().NonLocalizedString = "Close Testing Menu";

        Animator.SetAnim(Loading.Frames, Constants.AnimationDuration());
        Animator.AddComponent<TooltipTrigger>().NonLocalizedString = "This Is Your Animator";
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        SilhouetteSwapperUI.Instance.gameObject.SetActive(true);
    }
}