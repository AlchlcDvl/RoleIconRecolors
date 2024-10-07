namespace FancyUI.Assets.SilhouetteSwapper;

public class SilhouetteAnimation(string name)
{
    public string Name { get; } = name;

    public List<Sprite> Frames { get; set; } = [];

    public static bool operator ==(SilhouetteAnimation a, SilhouetteAnimation b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Name == b.Name && a.GetHashCode() == b.GetHashCode() && a.Frames.All(b.Frames.Contains);
    }

    public static bool operator !=(SilhouetteAnimation a, SilhouetteAnimation b) => !(a == b);

    public override int GetHashCode() => HashCode.Combine(Name, Frames.Count);

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        var type = obj.GetType();
        var plType = typeof(SilhouetteAnimation);

        if (type != plType && !plType.IsAssignableFrom(type))
            return false;

        return Equals((SilhouetteAnimation)obj);
    }
}