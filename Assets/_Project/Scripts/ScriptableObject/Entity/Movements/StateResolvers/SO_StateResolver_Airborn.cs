using UnityEngine;

[CreateAssetMenu(menuName = "Collections/StateResolver/Airborn")]
public class SO_StateResolver_Airborn : EntityStateResolver
{
    public LayerMask greenLayer;
    public override bool ResolveState(EntityMovementCore entity)
    {
        return !entity.FrameCollisionData.CheckCollisionDown() && !entity.FrameCollisionData.CheckCollisionLeft(greenLayer.value) && !entity.FrameCollisionData.CheckCollisionRight(greenLayer.value);
    }
}
