using System;
using System.Collections.Generic;
using Verse;

namespace RimWriter
{
    public class RoomRoleWorker_Library : RoomRoleWorker
    {
        
        public override float GetScore(Room room)
        {
            var num = 0;
            List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
            for (var i = 0; i < containedAndAdjacentThings.Count; i++)
            {
                if (containedAndAdjacentThings[i] is Building_Bookcase)
                {
                    num++;
                }
            }
            return 13.5f * (float)num;
        }
    }
}
