using UnityEngine;
using static UnityEditor.Progress;

public class ProductionBuilding : MonoBehaviour, ITickable, IInputOwner, IOutputOwner
{
    public enum State
    {
        NoRecipe,
        WaitingForInputs,
        Working,
        OutputFull,
    }

    [SerializeField] private Recipe recipe;
    [SerializeField, Min(0f)] private float speedMultiplier = 1f;

    private OutputPort[] outputPorts;

    private readonly Inventory inputBuffer = new Inventory();
    private readonly Inventory outputBuffer = new Inventory();

    private bool isCrafting;
    private float craftProgress;
    private int craftedCount;

    public State CurrentState { get; private set; }
    public Inventory InputBuffer => inputBuffer;
    public Inventory OutputBuffer => outputBuffer;
    public Recipe Recipe => recipe;

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

    public bool TryInsertInput(ItemData item)
    {
        if (recipe == null)
        {
            return false;
        }
        if (!recipe.Uses(item))
        {
            return false;
        }
        if (inputBuffer.GetAmount(item) >= item.MaxStackSize)
        {
            return false;
        }
        inputBuffer.Add(item, 1);
        return true;
    }

    private void TryStartCrafting()
    {
        if (!InputBuffer.Has(recipe.Inputs))
        {
            CurrentState = State.WaitingForInputs;
            return;
        }
        if (!HasOutputSpace())
        {
            CurrentState = State.OutputFull;
            return;
        }

        InputBuffer.Remove(recipe.Inputs);
        isCrafting = true;
        craftProgress = 0f;
    }

    private bool HasOutputSpace()
    {
        foreach (ItemStack output in recipe.Outputs)
        {
            if (OutputBuffer.GetAmount(output.Item) + output.Amount > output.Item.MaxStackSize)
                return false;
        }
        return true;
    }

    public void SetRecipe(Recipe recipe)
    {
        this.recipe = recipe;
        InputBuffer.Clear();
        OutputBuffer.Clear();

        isCrafting = false;
        craftedCount = 0;
    }

    private void PushOutputs()
    {
        foreach (OutputPort outputPort in outputPorts)
        {
            outputPort.PushOutput();
        }
    }

    public void Tick()
    {
        if (recipe == null)
        {
            CurrentState = State.NoRecipe;
            return;
        }

        if (!isCrafting)
            TryStartCrafting();

        if (isCrafting)
        {
            CurrentState = State.Working;
            craftProgress += TickManager.Instance.TickDuration * speedMultiplier;
            if (craftProgress >= recipe.CraftTime)
            {
                OutputBuffer.Add(recipe.Outputs);
                isCrafting = false;
                craftedCount++;
                Debug.Log($"{name}: {recipe.DisplayName} fertig ({craftedCount})");
            }
        }

        PushOutputs();
    }

    public bool TryPeekOutput(int portIndex, out ItemData item)
    {
        if (recipe == null || portIndex >= recipe.Outputs.Count)
        {
            item = null;
            return false;
        }
        item = recipe.Outputs[portIndex].Item;
        return outputBuffer.GetAmount(item) > 0;
    }

    public void ConfirmOutput(int portIndex)
    {
        ItemData item = recipe.Outputs[portIndex].Item;
        outputBuffer.Remove(item, 1);
    }
}

