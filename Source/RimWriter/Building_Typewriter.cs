// ----------------------------------------------------------------------
// These are basic usings. Always let them be here.
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimWriter;

/// <summary>
///     This is the main class for the Gramophone.
///     Major coding credits go to mrofa and Haplo.
///     I am but an amateur working on the shoulders of
///     giants.
/// </summary>
/// <author>Jecrell</author>
/// <permission>Free to use by all.</permission>
public class Building_Typewriter : Building_WorkTable
{
    private const float SanityRestoreRate = 0.000025f;

    // ===================== Variables =====================

    // Work variable
    // private int counter = 0;                  // 60Ticks = 1s // 20000Ticks = 1 Day

    // Destroyed flag. Most of the time not really needed, but sometimes...
    private bool destroyedFlag;

    /// <summary>
    ///     Debug button play, for now.
    /// </summary>
    /// <returns></returns>
    [DebuggerHidden]
    public IEnumerable<Command> CompGetGizmosExtra()
    {
        if (Prefs.DevMode)
        {
            yield return new Command_Action
            {
                defaultLabel = "Debug: Set fuel to 0.1",
                action = delegate
                {
                    // this.<> f__this.fuel = 0.1f;
                }
            };
        }
    }

    // ===================== Destroy =====================

    /// <summary>
    ///     Clean up when this is destroyed
    /// </summary>
    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        // block further ticker work
        destroyedFlag = true;

        base.Destroy(mode);
    }

    /// <summary>
    ///     All the menu options for the Gramophone.
    /// </summary>
    /// <param name="myPawn"></param>
    /// <returns></returns>
    public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
    {
        var list = base.GetFloatMenuOptions(myPawn).ToList();

        Thing building = null;
        for (var i = 0; i < 4; i++)
        {
            var c = Position + GenAdj.CardinalDirections[i];
            if (c.IsForbidden(myPawn))
            {
                continue;
            }

            var edifice = c.GetEdifice(Map);
            if (edifice == null || !edifice.def.building.isSittable || !myPawn.CanReserve(edifice))
            {
                continue;
            }

            building = edifice;
            break;
        }

        if (building == null)
        {
        }

        if (!myPawn.CanReserve(this))
        {
            list.Add(new FloatMenuOption("CannotUseReserved".Translate(), null));
            return list;
        }

        if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some))
        {
            list.Add(new FloatMenuOption("CannotUseNoPath".Translate(), null));
            return list;
        }

        if (building == null)
        {
            list.Add(new FloatMenuOption("Seat required", null));
            return list;
        }

        if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
        {
            list.Add(new FloatMenuOption(
                "CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Manipulation.label)),
                null));
            return list;
        }

        list.Add(new FloatMenuOption("Practice writing", action0));

        return list;

        void action0()
        {
            var job = new Job(DefDatabase<JobDef>.GetNamed("RimWriter_FreeWrite"), this, building);

            if (myPawn.jobs.TryTakeOrderedJob(job))
            {
                // Lala
            }
        }
    }

    // ===================== Inspections =====================

    /// <summary>
    ///     This string will be shown when the object is selected (focus)
    /// </summary>
    /// <returns></returns>
    public override string GetInspectString()
    {
        var stringBuilder = new StringBuilder();

        // Add the inspections string from the base
        stringBuilder.Append(base.GetInspectString());

        // return the complete string
        return stringBuilder.ToString();
    }

    /// <summary>
    ///     This is used, when the Ticker in the XML is set to 'Normal'
    ///     This Tick is done often (60 times per second)
    /// </summary>
    protected override void Tick()
    {
        if (destroyedFlag)
        {
            // Do nothing further, when destroyed (just a safety)
            return;
        }

        base.Tick();

        // Call work function
        doTickerWork();
    }

    // ===================== Ticker =====================

    /// <summary>
    ///     This is used, when the Ticker in the XML is set to 'Rare'
    ///     This is a tick that's done once every 250 normal Ticks
    /// </summary>
    public override void TickRare()
    {
        if (destroyedFlag)
        {
            // Do nothing further, when destroyed (just a safety)
            return;
        }

        // Don't forget the base work
        base.TickRare();

        // Call work function
        doTickerWork();
    }

    public override void UsedThisTick()
    {
        base.UsedThisTick();
        if (!Spawned || !InteractionCell.IsValid || InteractionCell.GetFirstPawn(MapHeld) is not { } pawn)
        {
            return;
        }

        if (!RimWriterUtility.IsCosmicHorrorsLoaded() && !RimWriterUtility.IsCultsLoaded())
        {
            return;
        }

        try
        {
            if (!RimWriterUtility.HasSanityLoss(pawn))
            {
                return;
            }

            RimWriterUtility.ReduceSanityLoss(pawn, SanityRestoreRate);
            if (Find.TickManager.TicksGame % 1500 == 0)
            {
                Messages.Message($"{pawn} has restored some sanity using the {def.label}.",
                    new TargetInfo(pawn.Position, pawn.Map), MessageTypeDefOf.NeutralEvent); // .Standard);
            }
        }
        catch
        {
            // Log.Message("Error loading Sanity Hediff.");
        }
    }

    // ===================== Main Work Function =====================

    /// <summary>
    ///     This will be called from one of the Ticker-Functions.
    /// </summary>
    private static void doTickerWork()
    {
    }
}