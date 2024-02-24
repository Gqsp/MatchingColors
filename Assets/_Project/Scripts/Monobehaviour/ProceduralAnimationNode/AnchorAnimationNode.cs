using UnityEngine;

public class AnchorAnimationNode : ProceduralAnimationNode
{
    [SerializeField] Transform anchor;
    float angle;

    public struct NodeData
    {
        //public Vector2 parentVelocity;
        public Vector2 updatedTargetPos;
    }

    private void Update()
    {
        transform.position = anchor.position;


        angle = rotationAnimation.Evaluate(Time.time % 1) * animationSpeed;
    }

    private NodeData CalculateNodeData(ProceduralAnimationNode node)
    {
        var defaultOffset = node.preferredParentOffset;
        NodeData data = new NodeData();
        data.updatedTargetPos = (Vector2)transform.position + Rotate(defaultOffset, angle);
        return data;
    }

    private void ApplyNodeData(NodeData data)
    {

    }

    public void UpdateRecursive(NodeData updatedData)
    {
        ApplyNodeData(updatedData);
        foreach(var node in childNodes)
        {
            //node.UpdateRecursive(CalculateNodeData(node));
        }
    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = cos * tx - sin * ty;
        v.y = sin * tx + cos * ty;
        return v;
    }
}
