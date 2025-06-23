using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RimWriter;

public class Building_InternalStorage : Building, IThingHolder, IStoreSettingsParent
{
    private CompStorageGraphic compStorageGraphic;
    protected ThingOwner InnerContainer;

    private StorageSettings storageSettings;

    protected Building_InternalStorage()
    {
        InnerContainer = new ThingOwner<Thing>(this, false);
    }

    public CompStorageGraphic CompStorageGraphic
    {
        get
        {
            compStorageGraphic ??= this.TryGetComp<CompStorageGraphic>();

            return compStorageGraphic;
        }
    }

    public override Graphic Graphic => CompStorageGraphic?.CurStorageGraphic ?? base.Graphic;

    public void Notify_SettingsChanged()
    {
    }

    public bool StorageTabVisible => true;

    public StorageSettings GetParentStoreSettings()
    {
        return def.building.fixedStorageSettings;
    }

    public StorageSettings GetStoreSettings()
    {
        return storageSettings;
    }

    public void GetChildHolders(List<IThingHolder> outChildren)
    {
        ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
    }

    public ThingOwner GetDirectlyHeldThings()
    {
        return InnerContainer;
    }

    public bool Accepts(Thing thing)
    {
        if (!storageSettings.AllowedToAccept(thing))
        {
            return false;
        }

        return InnerContainer.Count + 1 <= CompStorageGraphic.Props.countFullCapacity;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Deep.Look(ref InnerContainer, "innerContainer", this);
        Scribe_Deep.Look(ref storageSettings, "storageSettings", this);
    }

    public override void PostMake()
    {
        base.PostMake();
        storageSettings = new StorageSettings(this);
        if (def.building.defaultStorageSettings != null)
        {
            storageSettings.CopyFrom(def.building.defaultStorageSettings);
        }
    }

    public void TryDrop(Thing item, bool forbid = true)
    {
        if (!InnerContainer.Contains(item))
        {
            return;
        }

        InnerContainer.TryDrop(item, ThingPlaceMode.Near, out var outThing);
        if (forbid)
        {
            outThing.SetForbidden(true);
        }
    }

    public bool TryDropRandom(out Thing droppedThing, bool forbid = false)
    {
        droppedThing = null;
        if (InnerContainer.Count > 0)
        {
            InnerContainer.TryDrop(InnerContainer.RandomElement(), ThingPlaceMode.Near, out var outThing);
            if (forbid)
            {
                outThing.SetForbidden(true);
            }

            droppedThing = outThing as ThingBook;
            return true;
        }

        Log.Warning("Building_InternalStorage : TryDropRandom - failed to get a book.");
        return false;
    }
}