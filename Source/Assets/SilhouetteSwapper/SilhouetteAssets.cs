// namespace FancyUI.Assets.SilhouetteSwapper;
//
// public class SilhouetteAssets(string name)
// {
//     private string Name { get; } = name;
//     public int Count { get; private set; }
//
//     public Dictionary<string, SilhouetteAnimation> Animations { get; } = [];
//
//     public void Debug()
//     {
//         Fancy.Instance.Message($"Debugging {Name}");
//         Count = 0;
//
//         foreach (var (name1, animation) in Animations)
//         {
//             if (!animation.IsValid())
//                 continue;
//
//             Fancy.Instance.Message($"{name1} has an animation!");
//             Count++;
//         }
//
//         Fancy.Instance.Message($"{Animations.Count} assets loaded!");
//     }
// }