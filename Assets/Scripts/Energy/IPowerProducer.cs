using UnityEngine;

public interface IPowerProducer
{
    Transform transform { get; }
    float CurrentProduction { get; }
}
