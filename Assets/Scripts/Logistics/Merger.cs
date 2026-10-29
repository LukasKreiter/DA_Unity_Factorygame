using UnityEngine;

public class Merger : MonoBehaviour, ITickable, IInputOwner, IOutputOwner
{
    private ItemData[] slots;
    private int nextInput;
    private int peekedSlot = -1;
    private OutputPort outputPort;

    private void Awake()
    {
        slots = new ItemData[GetComponentsInChildren<InputPort>().Length];
        outputPort = GetComponentInChildren<OutputPort>();
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
        if (portIndex >= slots.Length) return false;
        if (slots[portIndex] != null) return false;
        slots[portIndex] = item;
        return true;
    }
    public bool TryPeekOutput(int portIndex, out ItemData item)
    {
        for (int i = 0; i < slots.Length; i++) {
            int slot = (nextInput + i) % slots.Length;

            if (slots[slot] != null)
            {
                peekedSlot = slot;
                item = slots[slot];
                return true;
            }
        }
        item = null;
        return false;
    }

    public void ConfirmOutput(int portIndex)
    {
        slots[peekedSlot] = null;
        nextInput = (peekedSlot + 1) % slots.Length;
    }

    public void Tick()
    {
        if (outputPort == null) return;
        outputPort.PushOutput();
    }
}
