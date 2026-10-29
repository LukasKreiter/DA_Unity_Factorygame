using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour, IItemReceiver, ITickable
{
    [SerializeField][Min(0.1f)] private float length = 5f;
    [SerializeField][Min(0.1f)] private float speed = 1f;
    [SerializeField][Min(0.1f)] private float itemSpacing = 0.5f;
    [SerializeField] private OutputPort from;
    [SerializeField] private InputPort to;

    private List<BeltItem> items = new List<BeltItem>();
    private IItemReceiver target;

    private void OnEnable()
    {
        TickManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (TickManager.Instance != null)
            TickManager.Instance.Unregister(this);
    }

    private void Start()
    {
        if (from != null)
        {
            from.Connect(this);
        }
        if (to != null)
        {
            Connect(to);
        }
    }

    public void Connect(IItemReceiver receiver)
    {
        target = receiver;
    }

    public bool TryInsert(ItemData item)
    {
        if (items.Count != 0 && items[items.Count-1].position < itemSpacing)
        {
            return false;
        }
        items.Add(new BeltItem(item));
        return true;
    }
    public void Tick()
    {
        float step = speed * TickManager.Instance.TickDuration;
        float maxPosition;

        for (int i = 0; i < items.Count; i++) {
            if (i == 0)
            {
                maxPosition = length;
            }
            else {
                maxPosition = items[i - 1].position - itemSpacing;
            }
            items[i].position = Mathf.Min(items[i].position + step, maxPosition);
        }
        if (items.Count == 0 || target == null) {
            return;
        }
        if (items[0].position >= length)
        {
            if (target.TryInsert(items[0].item))
            {
                items.RemoveAt(0);
            }
        }

    }
}
