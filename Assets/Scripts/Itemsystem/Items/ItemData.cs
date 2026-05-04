public class ItemData
{
    int itemID;
    ItemType itemType;
    
    public ItemData(ItemType itemType)
    {
        this.itemType = itemType;
    }
    
    public ItemType getItemType()
    {
        return this.itemType;
    }

    public void setItemType(ItemType type)
    {
        this.itemType = type;
    }
}