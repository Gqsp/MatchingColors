using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnimationNode : MonoBehaviour
{
    
    [SerializeField] protected List<ProceduralAnimationNode> childNodes;
    ProceduralAnimationNode parentNode;

    [SerializeField] protected AnimationCurve rotationAnimation;
    [SerializeField] protected float animationSpeed;
    
    [SerializeField] float baseAngle;
    [SerializeField] public Vector2 preferredParentOffset;

    [SerializeField] float maxDistanceToCenter;
    [SerializeField] float childMaxDistance;
    [SerializeField] float dampMovementFactor;

    Vector3 desiredPos;

    private void Start()
    {
        foreach(var child in childNodes)
        {
            child.Parent(this);
        }

        desiredPos = transform.localPosition;
    }

   

    public void Parent(ProceduralAnimationNode newParent)
    {
        parentNode = newParent;
    }
    public ProceduralAnimationNode GetRootNode()
    {
        if (parentNode != null) return parentNode.GetRootNode();
        return this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(desiredPos, maxDistanceToCenter);
    }
}
