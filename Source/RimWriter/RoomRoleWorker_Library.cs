using Verse;

namespace RimWriter;

public class RoomRoleWorker_Library : RoomRoleWorker
{
    public override float GetScore(Room room)
    {
        var containedAndAdjacentThings = room.ContainedAndAdjacentThings;
        var num = containedAndAdjacentThings.Count(thing => thing is Building_Bookcase);

        return 13.5f * num;
    }
}