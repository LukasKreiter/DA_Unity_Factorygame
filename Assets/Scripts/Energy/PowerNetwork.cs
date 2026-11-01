using System.Collections.Generic;
using UnityEngine;

public class PowerNetwork 
{
    private List<PowerPole> poles = new List<PowerPole>();
    
    private List<IPowerProducer> producers = new List<IPowerProducer>();
    private List<IPowerConsumer> consumers = new List<IPowerConsumer>();

    public float Production { get; private set; }
    public float Demand { get; private set; }
    public float Satisfaction { get; private set; }

    public void Add(PowerPole pole)
    {
        poles.Add(pole);
    }
    public void Add(IPowerProducer producer) {
        producers.Add(producer);
    }
    public void Add(IPowerConsumer consumer)
    {
        consumers.Add(consumer);
    }

    public void Tick()
    {
        Production = 0;
        foreach (IPowerProducer producer in producers)
        {
            Production += producer.CurrentProduction;
        }

        Demand = 0;
        foreach (IPowerConsumer consumer in consumers)
        {
            Demand += consumer.CurrentDemand;
        }

        if (Demand <= 0)
        {
            Satisfaction = Production > 0 ? 1 : 0;
        }
        else
        {
            Satisfaction = Mathf.Min(1, Production / Demand);
        }

        foreach (IPowerConsumer consumer in consumers)
        {
            consumer.SetSatisfaction(Satisfaction);
        }
    }
}
