using System.Collections.Generic;

/// <summary>방 Id별 RoomRuntimeData 저장/조회 담당</summary>
public class RoomRuntimeRegistry
{
    private readonly Dictionary<int, RoomRuntimeData> data = new Dictionary<int, RoomRuntimeData>();

    public IEnumerable<RoomRuntimeData> AllData => data.Values;

    public void Register(int roomId, RoomRuntimeData roomRuntimeData)
    {
        data[roomId] = roomRuntimeData;
    }

    public RoomRuntimeData Get(int roomId)
    {
        return data[roomId];
    }

    public void Clear()
    {
        data.Clear();
    }
}
