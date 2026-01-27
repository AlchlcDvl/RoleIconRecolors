namespace FancyUI.UI;

public abstract class UISingleton<T> : UIController where T : UIController
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            _instance ??= FindObjectOfType<T>(true);
            return _instance;
        }
    }

    public virtual void OnDestroy() => _instance = null;
}