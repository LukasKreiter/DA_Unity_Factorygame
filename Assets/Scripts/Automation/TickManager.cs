using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class TickManager : MonoBehaviour
{
    private List<ITickable> items = new List<ITickable>();

    private List<ITickable> toAdd = new List<ITickable>();
    private List<ITickable> toRemove = new List<ITickable>();

    [SerializeField] [Min(1)] private float ticksPerSecond = 20;
    public float TickDuration { get; private set; }
    private float accumulator = 0;
    [SerializeField] private int ticklimit = 5;

    public int CurrentTick { get; private set; } = 0;

    public static TickManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Zweiter TickManager vorhanden");
            Destroy(this);
            return;
        }
        TickDuration = 1f / ticksPerSecond;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Register(ITickable item)
    {
        toAdd.Add(item); 
    }

    public void Unregister(ITickable item)
    {
        toRemove.Add(item);
    }

    

    private void Update()
    {
        accumulator += Time.deltaTime;
        int ticksthisframe = 0;

        while (accumulator >= TickDuration && ticksthisframe < ticklimit)
        {
            RunTick();
            ticksthisframe++;
            accumulator -= TickDuration;
        }

        if (ticksthisframe >= ticklimit)
        {
            accumulator = 0;
        }

    }

    private void RunTick()
    {
        foreach (ITickable item2 in toAdd)
        {
            items.Add(item2);
        }
        toAdd.Clear();
        foreach (ITickable item2 in toRemove)
        {
            items.Remove(item2);
        }
        toRemove.Clear();
        CurrentTick++;
        foreach (ITickable item in items)
        {
            item.Tick();
            
        }
    }
}
