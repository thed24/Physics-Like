public interface IHoldItems
{
    void UseHeldItem(int hand);
    void DropItem(int hand);
    void EquipItem(IHoldable item, int hand);
    bool IsHoldingItemInHand(int hand);
}