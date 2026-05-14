using UnityEngine;

public abstract class BinaryNode : Node_Blueprint
{
    public Node_Blueprint inputA;
    public Node_Blueprint inputB;

    protected BinaryNode(Node_Blueprint a, Node_Blueprint b)
    {
        inputA = a;
        inputB = b;
    }

    public override void Init()
    {
        inputA?.Init();
        inputB?.Init();
    }
}