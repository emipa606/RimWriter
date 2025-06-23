using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWriter;

public class ITab_Inventory : ITab
{
    private const float ThingIconSize = 28f;

    private const float ThingLeftX = 36f;

    private const float TopPadding = 20f;

    private static readonly Color highlightColor = new(0.5f, 0.5f, 0.5f, 1f);

    private static readonly Color thingLabelColor = new(0.9f, 0.9f, 0.9f, 1f);

    private static readonly List<Thing> workingInvList = [];

    private Vector2 scrollPosition = Vector2.zero;

    private float scrollViewHeight;

    public ITab_Inventory()
    {
        size = new Vector2(460f, 450f);
        labelKey = "RimWriter_Inventory";
    }

    public override bool IsVisible => SelStorage.TryGetInnerInteractableThingOwner().Count > 0;

    private bool CanControl => SelStorage.Spawned && SelStorage.Faction == Faction.OfPlayer;

    private Building_InternalStorage SelStorage
    {
        get
        {
            if (SelThing is Building_InternalStorage bld)
            {
                return bld;
            }

            return null;
        }
    }

    protected override void FillTab()
    {
        Text.Font = GameFont.Small;
        var rect = new Rect(0f, TopPadding, size.x, size.y - TopPadding);
        var rect2 = rect.ContractedBy(10f);
        var position = new Rect(rect2.x, rect2.y, rect2.width, rect2.height);
        GUI.BeginGroup(position);
        Text.Font = GameFont.Small;
        GUI.color = Color.white;
        var outRect = new Rect(0f, 0f, position.width, position.height);
        var viewRect = new Rect(0f, 0f, position.width - 16f, scrollViewHeight);
        Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
        var num = 0f;

        if (SelStorage.TryGetInnerInteractableThingOwner() is { } t)
        {
            Widgets.ListSeparator(ref num, viewRect.width, "Inventory".Translate());
            workingInvList.Clear();
            workingInvList.AddRange(t);
            foreach (var thing in workingInvList)
            {
                DrawThingRow(ref num, viewRect.width, thing);
            }
        }

        if (Event.current.type == EventType.Layout)
        {
            scrollViewHeight = num + 30f;
        }

        Widgets.EndScrollView();
        GUI.EndGroup();
        GUI.color = Color.white;
        Text.Anchor = TextAnchor.UpperLeft;
    }

    private void DrawThingRow(ref float y, float width, Thing thing)
    {
        var rect = new Rect(0f, y, width, ThingIconSize);
        Widgets.InfoCardButton(rect.width - 24f, y, thing);
        rect.width -= 24f;
        if (CanControl)
        {
            var rect2 = new Rect(rect.width - 24f, y, 24f, 24f);
            TooltipHandler.TipRegion(rect2, "DropThing".Translate());
            if (Widgets.ButtonImage(rect2, ITabButton.Drop))
            {
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
                interfaceDrop(thing);
            }

            rect.width -= 24f;
        }

        var rect4 = rect;
        rect4.xMin = rect4.xMax - 60f;

        CaravanThingsTabUtility.DrawMass(thing, rect4);
        rect.width -= 60f;
        if (Mouse.IsOver(rect))
        {
            GUI.color = highlightColor;
            GUI.DrawTexture(rect, TexUI.HighlightTex);
        }

        if (thing.def.DrawMatSingle != null && thing.def.DrawMatSingle.mainTexture != null)
        {
            Widgets.ThingIcon(new Rect(4f, y, ThingIconSize, ThingIconSize), thing);
        }

        Text.Anchor = TextAnchor.MiddleLeft;
        GUI.color = thingLabelColor;
        var rect5 = new Rect(ThingLeftX, y, rect.width - ThingLeftX, rect.height);
        var text = thing.LabelCap;
        Text.WordWrap = false;
        Widgets.Label(rect5, text.Truncate(rect5.width));
        Text.WordWrap = true;
        var text2 = thing.LabelCap;
        if (thing.def.useHitPoints)
        {
            var text3 = text2;
            text2 = $"{text3}\n{thing.HitPoints} / {thing.MaxHitPoints}";
        }

        TooltipHandler.TipRegion(rect, text2);
        y += ThingIconSize;
    }

    private void interfaceDrop(Thing t)
    {
        SelStorage.TryDrop(t);
    }
}