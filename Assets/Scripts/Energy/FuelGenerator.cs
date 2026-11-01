using UnityEngine;

public class FuelGenerator : MonoBehaviour, ITickable, IInputOwner, IPowerProducer
{
    public enum State
    {
        NoRecipe,
        WaitingForFuel,
        Burning
    }

    public State CurrentState { get; private set; }

    [SerializeField] private BuildingData buildingData;
    [SerializeField] private Recipe recipe;

    private readonly Inventory inputBuffer = new Inventory();
    private float burnProgress;

    public float CurrentProduction => buildingData != null && CurrentState == State.Burning ? buildingData.PowerProduction : 0;

    public bool TryInsertInput(ItemData item, int portIndex)
    {
        if (recipe == null) return false;
        if (!recipe.Uses(item)) return false;
        if (inputBuffer.GetAmount(item) >= item.MaxStackSize) return false;

        inputBuffer.Add(item, 1);
        return true;
    }

    public void Tick()
    {
        if (recipe == null)
        {
            CurrentState = State.NoRecipe; 
            return;
        }
        if (CurrentState == State.Burning)
        {
            burnProgress += TickManager.Instance.TickDuration;
            if (burnProgress >= recipe.CraftTime)
            {
                CurrentState = State.WaitingForFuel;
            }
        }
        if (CurrentState != State.Burning)
        {
            TryStartBurning();
        }
    }

    private void TryStartBurning()
    {

        if (!inputBuffer.Has(recipe.Inputs))
        {
            CurrentState = State.WaitingForFuel;
            return;
        }

        inputBuffer.Remove(recipe.Inputs);
        CurrentState = State.Burning;
        burnProgress = 0;
    }

    private void OnEnable()
    {
        TickManager.Instance.Register(this);
        if (PowerGrid.Instance != null) PowerGrid.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (TickManager.Instance != null) TickManager.Instance.Unregister(this);
        if (PowerGrid.Instance != null) PowerGrid.Instance.Unregister(this);
    }
}
