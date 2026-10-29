public interface IOutputOwner
{
    bool TryPeekOutput(int portIndex, out ItemData item);
    void ConfirmOutput(int portIndex);
}
