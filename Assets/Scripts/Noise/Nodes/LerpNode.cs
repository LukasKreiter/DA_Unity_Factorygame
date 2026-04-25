using UnityEngine;

public class LerpNode : Node_Blueprint
{
    public Node_Blueprint a;
    public Node_Blueprint b;
    public Node_Blueprint mask;

    public LerpNode(
        Node_Blueprint a,
        Node_Blueprint b,
        Node_Blueprint mask)
    {
        this.a = a;
        this.b = b;
        this.mask = mask;
    }

    public override void Init()
    {
        a.Init();
        b.Init();
        mask.Init();
    }

    public override float Evaluate(float x, float y)
    {
        float av = a.Evaluate(x,y);
        float bv = b.Evaluate(x,y);

        float m = mask.Evaluate(x,y);
        m = (m + 1f) * 0.5f;

        return Mathf.Lerp(av, bv, m);
    }
}