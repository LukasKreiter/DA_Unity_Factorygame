using UnityEngine;

public abstract class UnaryNode : Node_Blueprint
{
    public Node_Blueprint input;

    protected UnaryNode(Node_Blueprint input)
    {
        this.input = input;
    }

    public override void Init()
    {
        input?.Init();
    }
}