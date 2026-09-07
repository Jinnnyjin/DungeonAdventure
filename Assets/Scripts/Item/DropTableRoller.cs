using System;
using System.Collections.Generic;

public class DropTableRoller
{
    public ItemData RollDrop(List<DropEntry> dropTable)
    {
        float totalWeight = 0;
        foreach (DropEntry entry in dropTable)
        {
            totalWeight += entry.DropWeight;
        }

        // float는 range(min,max)에서 min, max값 모두 포함 // int는 max 미포함
        float roll = UnityEngine.Random.Range(0f, totalWeight);

        float standard = 0;
        foreach (DropEntry entry in dropTable)
        {
            standard += entry.DropWeight;

            if(roll < standard)
            {
                return entry.Item;
            }
        }

        throw new InvalidOperationException("드랍 테이블에서 아이템을 선택 실패");
    }
}
