using UnityEngine;

public class InputPort : MonoBehaviour, IItemReceiver
{
    private ProductionBuilding building;

    public bool TryInsert(ItemData item)
    {
        return building.TryInsertInput(item);
    }
    private void Awake()
    {
        building = GetComponentInParent<ProductionBuilding>();
    }
}
