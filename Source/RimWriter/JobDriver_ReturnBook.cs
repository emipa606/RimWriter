using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimWriter;

public class JobDriver_ReturnBook : JobDriver
{
    private const TargetIndex CorpseIndex = TargetIndex.A;

    private const TargetIndex GraveIndex = TargetIndex.B;

    public JobDriver_ReturnBook()
    {
        rotateToFace = GraveIndex;
    }

    private ThingBook Book => (ThingBook)job.GetTarget(CorpseIndex).Thing;

    private Building_InternalStorage Storage => (Building_InternalStorage)job.GetTarget(GraveIndex).Thing;

    public override bool TryMakePreToilReservations(bool b)
    {
        return pawn.Reserve(Book, job) && pawn.Reserve(Book, job);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDestroyedNullOrForbidden(CorpseIndex);
        this.FailOnDestroyedNullOrForbidden(GraveIndex);
        this.FailOn(() => !Storage.Accepts(Book));
        yield return Toils_Goto.GotoThing(CorpseIndex, PathEndMode.ClosestTouch)
            .FailOnSomeonePhysicallyInteracting(CorpseIndex);
        yield return Toils_Haul.StartCarryThing(CorpseIndex);
        yield return Toils_Haul.CarryHauledThingToContainer();
        var prepare = Toils_General.Wait(250);
        prepare.WithProgressBarToilDelay(GraveIndex);
        yield return prepare;
        yield return new Toil
        {
            initAction = delegate
            {
                if (pawn.carryTracker.CarriedThing == null)
                {
                    Log.Error($"{pawn} tried to place hauled corpse in grave but is not hauling anything.");
                    return;
                }

                if (!Storage.Accepts(Book))
                {
                    return;
                }

                if (Book.holdingOwner != null)
                {
                    Book.holdingOwner.TryTransferToContainer(Book, Storage.TryGetInnerInteractableThingOwner(),
                        Book.stackCount);
                }
                else
                {
                    Storage.TryGetInnerInteractableThingOwner().TryAdd(Book);
                }

                Storage.CompStorageGraphic.UpdateGraphics();
                pawn.carryTracker.innerContainer.Remove(Book);
                pawn.records.Increment(RecordDefOf.CorpsesBuried);
            }
        };
    }
}