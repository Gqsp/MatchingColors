using UnityEngine;

[CreateAssetMenu(menuName = "Collections/StateResolver/Grounded")]
public class SO_StateResolver_Grounded : EntityStateResolver
{
    [SerializeField] LayerMask groundLayer;

    public override bool ResolveState(EntityMovementCore entity)
    {
        return entity.FrameCollisionData.CheckCollisionDown();
    }
}
