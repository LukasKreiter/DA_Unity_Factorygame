using UnityEngine;

public interface IItemReceiver
{
    bool TryInsert(ItemData item);
}
