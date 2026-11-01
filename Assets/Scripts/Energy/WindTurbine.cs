using UnityEngine;

public class WindTurbine : MonoBehaviour, IPowerProducer
{
    [SerializeField] BuildingData buildingData;
    public float CurrentProduction => buildingData != null ? buildingData.PowerProduction : 0;

    private void OnEnable()
    {
        if (PowerGrid.Instance != null) PowerGrid.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (PowerGrid.Instance != null) PowerGrid.Instance.Unregister(this);
    }
}
