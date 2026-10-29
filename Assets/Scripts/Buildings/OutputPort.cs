using UnityEngine;

public class OutputPort : MonoBehaviour
{
    [SerializeField] private int index;

    [SerializeField] private InputPort testConnection;

    private ProductionBuilding building;

    private IItemReceiver target;

    private void Awake()
    {
        building = GetComponentInParent<ProductionBuilding>();
    }

    public void Connect(IItemReceiver receiver)
    {
        target = receiver;
    }

    private void Start()
    {
        if (testConnection != null)
        {
            Connect(testConnection);
        }
    }

    public void PushOutput()
    {
        if (target == null || building.Recipe == null || building.Recipe.Outputs.Count < index+1) {
            return;
        }
        ItemData item = building.Recipe.Outputs[index].Item;

        if (building.OutputBuffer.GetAmount(item) <= 0)
        {
            return;
        }
        if (target.TryInsert(item))
        {
            building.OutputBuffer.Remove(item, 1);
        }
    }

}
