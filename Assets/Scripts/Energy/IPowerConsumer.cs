using UnityEngine;

public interface IPowerConsumer
{
    Transform transform { get; }
    float CurrentDemand { get; }
    void SetSatisfaction(float value);
}
