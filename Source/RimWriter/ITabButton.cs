using UnityEngine;
using Verse;

namespace RimWriter;

[StaticConstructorOnStartup]
internal static class ITabButton
{
    public static readonly Texture2D Drop = ContentFinder<Texture2D>.Get("UI/Buttons/Drop");
}