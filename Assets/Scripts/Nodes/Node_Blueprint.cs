using UnityEngine;

public abstract class Node_Blueprint : ScriptableObject
{
    public string nodeName = "Node";

    public virtual void Init() { }

    public abstract float Evaluate(float x, float y);
}