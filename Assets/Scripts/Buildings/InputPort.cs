using UnityEngine;

public class InputPort : MonoBehaviour, IItemReceiver
{
    private IInputOwner owner;

    public bool TryInsert(ItemData item)
    {
        return owner.TryInsertInput(item);
    }
    private void Awake()
    {
        owner = GetComponentInParent<IInputOwner>();
    }
}
