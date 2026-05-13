using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase
{
    List<ItemData> itemDataBase = new List<ItemData>();


    public void addItem(string name, string description)
    {
        int itemID = itemDataBase.Count+1;
        
        itemDataBase.Add(new ItemData(itemID, name, description));
    }
}
