using System.Collections.Generic;

public class Recipe
{
    public string Id;

    public Dictionary<ItemType, int> Inputs;
    public Dictionary<ItemType, int> Outputs;

    public float CraftTime;

    public Recipe(ItemType input1, int amount1, ItemType output1, int amountO)
    {
        Inputs = new()
        {
            {input1, amount1}
        };

        Outputs = new()
        {
            {output1, amountO}
        };


    }
    
}