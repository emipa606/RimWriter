using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimWriter;

public class GuideBook : ThingBook
{
    private const float learnRate = 0.085f;

    [CanBeNull] private string author;

    private Color? skillColor;

    private SkillDef skillDef = DefDatabase<SkillDef>.GetRandom();

    [CanBeNull]
    private string Author
    {
        get
        {
            if (author != null)
            {
                return author;
            }

            var authorNameInt = Traverse.Create(CompArt).Field("authorNameInt").GetValue<TaggedString>();
            if (!string.IsNullOrEmpty(authorNameInt) && !authorNameInt.RawText.StartsWith(" "))
            {
                // Log.Message(authorNameInt);
                author = authorNameInt;
            }
            else
            {
                author = Authors.Random;
                Traverse.Create(CompArt).Field("authorNameInt").SetValue((TaggedString)author);
            }

            return author;
        }
    }

    private CompArt CompArt => this.TryGetComp<CompArt>();

    private CompQuality CompQuality => this.TryGetComp<CompQuality>();

    public override string DescriptionFlavor =>
        "RimWriter_GuideSkillDesc".Translate(skillDef.label, QualityRate.ToStringPercent()) + "\n" +
        base.DescriptionFlavor;

    public override Graphic Graphic =>
        DefaultGraphic.GetColoredVersion(ShaderDatabase.CutoutComplex, SkillColor, SkillColor);

    public override string Label
    {
        get
        {
            if (CompArt != null)
            {
                return Author.NullOrEmpty()
                    ? "RimWriter_GuideTitle".Translate(skillDef.LabelCap) + " (" + CompQuality.Quality.GetLabel() +
                      ")"
                    : "RimWriter_GuideTitleWithAuthor".Translate(Author, skillDef.LabelCap) + " (" +
                      CompQuality.Quality.GetLabel() + ")";
            }

            return base.Label;
        }
    }

    private float LearnRate => learnRate * QualityRate;

    private float QualityRate
    {
        get
        {
            switch (CompQuality.Quality)
            {
                case QualityCategory.Awful:
                    return 0.25f;
                case QualityCategory.Poor:
                    break;
                case QualityCategory.Normal:
                    return 0.75f;
                case QualityCategory.Good:
                    return 1.0f;
                case QualityCategory.Excellent:
                    return 1.25f;
                case QualityCategory.Masterwork:
                    return 1.5f;
                case QualityCategory.Legendary:
                    return 1.75f;
            }

            return 0.5f;
        }
    }

    private Color SkillColor
    {
        get
        {
            if (skillColor != null)
            {
                return skillColor.Value;
            }

            var result = Color.white;
            if (skillDef == SkillDefOf.Animals)
            {
                result = new Color(0.14f, 0.19f, 0.42f); // Color.steelblue;
            }
            else if (skillDef == SkillDefOf.Artistic)
            {
                result = new Color(0.33f, 0.1f, 0.55f); // Color.purple
            }
            else if (skillDef == SkillDefOf.Construction)
            {
                result = new Color(0.8f, 0.4f, 0f); // Color.orange;
            }
            else if (skillDef == SkillDefOf.Cooking)
            {
                result = new Color(0.4f, 0f, 0f); // Color.dullred;
            }
            else if (skillDef == SkillDefOf.Crafting)
            {
                result = new Color(0.5f, 0.16f, 0.16f); // Color.brown;
            }
            else if (skillDef == SkillDefOf.Intellectual)
            {
                result = new Color(0f, 0f, 0.5f); // Color.navy;
            }
            else if (skillDef == SkillDefOf.Medicine)
            {
                result = Color.white;
            }
            else if (skillDef == SkillDefOf.Melee)
            {
                result = Color.red;
            }
            else if (skillDef == SkillDefOf.Mining)
            {
                result = Color.black;
            }
            else if (skillDef == SkillDefOf.Plants)
            {
                result = new Color(0.13f, 0.55f, 0.13f); // Color.forestgreen;
            }
            else if (skillDef == SkillDefOf.Shooting)
            {
                result = Color.gray;
            }
            else if (skillDef == SkillDefOf.Social)
            {
                result = new Color(0.93f, 0.8f, 0.68f);
            }

            skillColor = result;

            return skillColor.Value;
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref skillDef, "skillDef");
        Scribe_Values.Look(ref author, "author");
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        skillDef ??= DefDatabase<SkillDef>.GetRandom();
    }

    public void Teach(Pawn pawn, int delta)
    {
        pawn?.skills?.Learn(skillDef, LearnRate * delta);
    }
}