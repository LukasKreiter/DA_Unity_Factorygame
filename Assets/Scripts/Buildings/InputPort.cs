using UnityEngine;

public class InputPort : MonoBehaviour, IItemReceiver
{
    private IInputOwner owner;
    [SerializeField] private int index;

    public bool TryInsert(ItemData item)
    {
        return owner.TryInsertInput(item, index);
    }
    private void Awake()
    {
        owner = GetComponentInParent<IInputOwner>();
    }
}
