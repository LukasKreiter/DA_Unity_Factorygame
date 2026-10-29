using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour, ITickable, IInputOwner, IOutputOwner
{
    private readonly Queue<ItemData> buffer = new Queue<ItemData>();
    [SerializeField][Min(1)] private int bufferSize = 2;
    public int BufferCount => buffer.Count;

    private OutputPort[] outputPorts;
    private int nextOutput;

    private void Awake()
    {
        outputPorts = GetComponentsInChildren<OutputPort>();
    }

    private void OnEnable()
    {
        TickManager.Instance.Register(this);
    }
    private void OnDisable()
    {
        if (TickManager.Instance != null)
            TickManager.Instance.Unregister(this);
    }

    public bool TryInsertInput(ItemData item, int portIndex)
    {
        if (buffer.Count >= bufferSize) {
            return false;
        }
        buffer.Enqueue(item);
        return true;
    }

    public bool TryPeekOutput(int portIndex, out ItemData item)
    {
        if (buffer.Count == 0 || portIndex != nextOutput) {
            item = null;
            return false;
        }
        item = buffer.Peek();
        return true;
    }

    public void ConfirmOutput(int portIndex)
    {
        buffer.Dequeue();
        nextOutput = (nextOutput + 1) % outputPorts.Length;
    }

    public void Tick()
    {
        if (outputPorts.Length == 0)
        {
            return;
        }
        if (buffer.Count == 0)
        {
            return;
        }

        for (int i = 0; i < outputPorts.Length; i++)
        {
            bool sent = false;

            foreach (OutputPort port in outputPorts) {
                if (port.PushOutput())
                {
                    sent = true; 
                    break;
                }
            }
            if (sent)
            {
                return;
            }
            nextOutput = (nextOutput + 1) % outputPorts.Length;
        }
    }
}
