using UnityEngine;

public class OutputPort : MonoBehaviour
{
    [SerializeField] private int index;

    [SerializeField] private InputPort testConnection;

    private IOutputOwner owner;

    private IItemReceiver target;

    private void Awake()
    {
        owner = GetComponentInParent<IOutputOwner>();
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
        if (target == null) {
            return;
        }

        if (!owner.TryPeekOutput(index, out ItemData item))
        {
            return;
        }
        if (target.TryInsert(item))
        {
            owner.ConfirmOutput(index);
        }
    }

}
