using UnityEngine;
public class ItemData : ScriptableObject
{
    int itemID;
    string name;
    string description;
    
    public ItemData(int id, string name, string description)
    {
        this.name = name;
        this.description = description;
    }
    
    public string getName()
    {
        return this.name;
    }

    public string getDescription()
    {
        return this.description;
    }

    public int getID()
    {
        return this.itemID;
    }


}