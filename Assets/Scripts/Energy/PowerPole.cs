using System.Collections.Generic;
using UnityEngine;

public class PowerPole : MonoBehaviour
{
    [SerializeField][Min(0.1f)] private float supplyRadius = 8f;
    [SerializeField][Min(0.1f)] private float connectionRange = 20f;
    public PowerNetwork Network { get; set; }

    private List<PowerPole> neighbors = new List<PowerPole>();

    public float SupplyRadius => supplyRadius;
    public float ConnectionRange => connectionRange;

    public IReadOnlyList<PowerPole> Neighbors => neighbors;

    private void OnEnable()
    {
        if (PowerGrid.Instance != null) PowerGrid.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (PowerGrid.Instance != null) PowerGrid.Instance.Unregister(this);
    }

    public void AddNeighbor(PowerPole neighbor)
    {
        neighbors.Add(neighbor);
    }

    public void ClearNeighbors()
    {
        neighbors.Clear();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SupplyRadius);

        Gizmos.color = Color.cyan;
        for (int i = 0; i < neighbors.Count; i++) {
            Gizmos.DrawLine(transform.position, neighbors[i].transform.position);
        }
    }
}
