using UnityEngine;

[CreateAssetMenu(menuName = "Collections/StateResolver/WallCling")]
public class SO_StateResolver_WallCling : EntityStateResolver
{
    public Direction wallClingDetection;
    public LayerMask wallLayer;
    public override bool ResolveState(EntityMovementCore entity)
    {
        switch (wallClingDetection)
        {
            case Direction.Left:
                return entity.FrameCollisionData.CheckCollisionLeft(wallLayer.value) && !entity.FrameCollisionData.CheckCollisionDown();
            case Direction.Right:
                return entity.FrameCollisionData.CheckCollisionRight(wallLayer.value) && !entity.FrameCollisionData.CheckCollisionDown();
            default:
                return (entity.FrameCollisionData.CheckCollisionLeft(wallLayer.value) || entity.FrameCollisionData.CheckCollisionRight(wallLayer.value)) && !entity.FrameCollisionData.CheckCollisionDown();
        }
    }
}

public enum Direction
{
    Left,
    Right,
    Both
}

