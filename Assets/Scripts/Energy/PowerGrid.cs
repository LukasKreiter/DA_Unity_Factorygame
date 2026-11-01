using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-90)]
public class PowerGrid : MonoBehaviour, ITickable
{
    public static PowerGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogError("Doppeltes PowerGrid, wird geloescht");
            Destroy(this);
        }
    }

    private void Start()
    {
        TickManager.Instance.Register(this);
    }

    private void OnDestroy()
    {
        if (TickManager.Instance != null) TickManager.Instance.Unregister(this);
        if (Instance == this) Instance = null;
    }

    private List<PowerPole> poles = new List<PowerPole>();

    private List<IPowerProducer> producers = new List<IPowerProducer>();
    private List<IPowerConsumer> consumers = new List<IPowerConsumer>();

    private List<PowerNetwork> networks = new List<PowerNetwork>();

    private List<IPowerConsumer> unpoweredConsumers = new List<IPowerConsumer>();

    bool dirty;

    public IReadOnlyList<PowerNetwork> Networks => networks;

    public void Register(PowerPole pole)
    {
        poles.Add(pole);
        dirty = true;
    }

    public void Unregister(PowerPole pole)
    {
        poles.Remove(pole);
        dirty = true;
    }

    public void Register(IPowerProducer producer)
    {
        producers.Add(producer);
        dirty = true;
    }

    public void Unregister(IPowerProducer producer)
    {
        producers.Remove(producer);
        dirty = true;
    }

    public void Register(IPowerConsumer consumer)
    {
        consumers.Add(consumer);
        dirty = true;
    }

    public void Unregister(IPowerConsumer consumer)
    {
        consumers.Remove(consumer);
        dirty = true;
    }


    public void Tick()
    {
        if(dirty)
        {
            RebuildNetworks();
            dirty = false;
        }
        foreach (PowerNetwork network in networks) {
            network.Tick();
        }
        foreach (IPowerConsumer consumer in unpoweredConsumers)
        {
            consumer.SetSatisfaction(0);
        }
    }

    private void RebuildNetworks()
    {
        Queue<PowerPole> queue = new Queue<PowerPole>();

        networks.Clear();
        unpoweredConsumers.Clear();
        foreach (PowerPole pole in poles)
        {
            pole.Network = null;
            pole.ClearNeighbors();
        }

        for (int i = 0; i < poles.Count; i++)
        {
            for (int j = i+1; j < poles.Count; j++)
            {
                PowerPole a = poles[i];
                PowerPole b = poles[j];

                if (Vector3.Distance(a.transform.position, b.transform.position) <= Mathf.Min(a.ConnectionRange, b.ConnectionRange))
                {
                    a.AddNeighbor(b);
                    b.AddNeighbor(a);
                }
            }
        }

        foreach (PowerPole start in poles)
        {
            if (start.Network == null)
            {
                PowerNetwork network = new PowerNetwork();
                start.Network = network;
                networks.Add(network);
                queue.Enqueue(start);

                while (queue.Count > 0)
                {
                    PowerPole current = queue.Dequeue();
                    network.Add(current);
                    foreach (PowerPole neighbor in current.Neighbors)
                    {
                        if (neighbor.Network == null)
                        {
                            neighbor.Network = network;
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
        }

        foreach (IPowerProducer producer in producers)
        {
            PowerPole pole = FindNearestPole(producer.transform.position);
            if (pole != null) pole.Network.Add(producer);
        }
        foreach (IPowerConsumer consumer in consumers)
        {
            PowerPole pole = FindNearestPole(consumer.transform.position);
            if (pole != null ) pole.Network.Add(consumer);
            else unpoweredConsumers.Add(consumer);
        }

        Debug.Log($"Stromnetze: {networks.Count}");
    }

    private PowerPole FindNearestPole(Vector3 position)
    {
        PowerPole best = null;
        float bestDistance = float.MaxValue;

        foreach (PowerPole pole in poles)
        {
            float d = Vector3.Distance(position, pole.transform.position);

            if (d <= pole.SupplyRadius && d < bestDistance)
            {
                best = pole;
                bestDistance = d;
            }
        }
        return best;
    }
}
