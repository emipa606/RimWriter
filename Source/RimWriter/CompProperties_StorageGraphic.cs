using Verse;

namespace RimWriter;

public class CompProperties_StorageGraphic : CompProperties
{
    public readonly int countFullCapacity = 30;

    public readonly int countSparseThreshhold = 5;

    public readonly GraphicData graphicEmpty = null;

    public readonly GraphicData graphicFull = null;

    public readonly GraphicData graphicSparse = null;

    public CompProperties_StorageGraphic()
    {
        compClass = typeof(CompStorageGraphic);
    }
}