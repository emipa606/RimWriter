using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWriter
{
    public class Building_InternalStorage : Building, IThingHolder, IStoreSettingsParent
    {
        protected ThingOwner innerContainer;
        private StorageSettings storageSettings;
        private CompStorageGraphic compStorageGraphic = null;
        public CompStorageGraphic CompStorageGraphic
        {
            get
            {
                if (compStorageGraphic == null)
                {
                    compStorageGraphic = this.TryGetComp<CompStorageGraphic>();
                }
                return compStorageGraphic;
            }
        }
        
        public override Graphic Graphic
        {
            get
            {
                if (CompStorageGraphic?.CurStorageGraphic != null)
                {
                    return CompStorageGraphic.CurStorageGraphic;
                }
                return base.Graphic;
            }
        }


        public bool StorageTabVisible => true;
        public Building_InternalStorage()
        {
            innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
        }
        
        public bool TryAccept(Thing thing)
        {
            return true;
        }

        public bool Accepts(Thing thing)
        {
            if (!storageSettings.AllowedToAccept(thing))
            {
                return false;
            }
            if (innerContainer.Count + 1 > CompStorageGraphic.Props.countFullCapacity)
            {
                return false;
            }
            return true;
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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner>(ref innerContainer, "innerContainer", new object[]
            {
                            this
            });
            Scribe_Deep.Look<StorageSettings>(ref storageSettings, "storageSettings", new object[]
            {
                this
            });
        }



        public bool TryDropRandom(out Thing droppedThing, bool forbid = false)
        {
            droppedThing = null;
            if (innerContainer.Count > 0)
            {
                innerContainer.TryDrop(innerContainer.RandomElement(), ThingPlaceMode.Near, out Thing outThing);
                if (forbid)
                {
                    outThing.SetForbidden(true);
                }

                droppedThing = outThing as ThingBook;
                return true;
            }
            else
            {
                Log.Warning("Building_InternalStorage : TryDropRandom - failed to get a book.");
            }
            return false;
        }

        public bool TryDrop(Thing item, bool forbid = true)
        {
            if (innerContainer.Contains(item))
            {
                innerContainer.TryDrop(item, ThingPlaceMode.Near, out Thing outThing);
                if (forbid)
                {
                    outThing.SetForbidden(true);
                }

                return true;
            }
            return false;
        }


        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public StorageSettings GetParentStoreSettings()
        {
            return def.building.fixedStorageSettings;
        }

        public StorageSettings GetStoreSettings()
        {
            return storageSettings;
        }
    }
}
