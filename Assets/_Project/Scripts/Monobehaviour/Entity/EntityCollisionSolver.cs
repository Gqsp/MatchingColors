using UnityEngine;

public class EntityCollisionSolver : EntityComponent
{
    [SerializeField] float _maxWalkableAngle;
    [SerializeField] float _maxCeilingAngle;
    [SerializeField] float _maxWallAngle;

    float _cosMaxWalkableAngle;
    float _cosMaxCeilingAngle;
    float _cosMaxWallAngle;
    

    CollisionData _collisionData;
    public CollisionData CollisionData => _collisionData;

    private void Awake()
    {
        _cosMaxCeilingAngle = Mathf.Abs(Mathf.Cos(90 - _maxCeilingAngle * Mathf.Deg2Rad));
        _cosMaxWallAngle = Mathf.Abs(Mathf.Cos(90 - _maxWallAngle * Mathf.Deg2Rad));
        _cosMaxWalkableAngle = Mathf.Abs(Mathf.Cos(90 - _maxWalkableAngle * Mathf.Deg2Rad));
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        int layer = 1 << collision.gameObject.layer;
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;

            if (CheckCos(Vector2.up, normal, _cosMaxWalkableAngle))
            {
                _collisionData.downCollision |= layer;
                _collisionData.floorNormal = normal;
            }
            else if (CheckCos(Vector2.left, normal, _cosMaxWallAngle))
            {
                _collisionData.rightCollision |= layer;
                _collisionData.rWallNormal = normal;
            }
            else if (CheckCos(Vector2.right, normal, _cosMaxWallAngle))
            {
                _collisionData.leftCollision |= layer;
                _collisionData.lWallNormal = normal;
            }
            else if (CheckCos(Vector2.down, normal, _cosMaxCeilingAngle))
            {
                _collisionData.upCollision |= layer;
                _collisionData.ceilingNormal = normal;
            }
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     if (_collisionData.CheckCollisionUp())
    //     {
    //         Gizmos.color = Color.red;
    //         DrawCollisionLine(Vector2.up, _collisionData.ceilingNormal.Perpendicular1());
    //     }
    //     Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * .5f);
    //
    //     Gizmos.color = Color.green;
    //     if (_collisionData.CheckCollisionDown())
    //     {
    //         Gizmos.color = Color.red;
    //         DrawCollisionLine(Vector2.down, _collisionData.floorNormal.Perpendicular1());
    //     }
    //     Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * .5f);
    //
    //     Gizmos.color = Color.green;
    //     if (_collisionData.CheckCollisionLeft())
    //     {
    //         Gizmos.color = Color.red;
    //         DrawCollisionLine(Vector2.left, _collisionData.lWallNormal.Perpendicular1());
    //     }
    //     Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * .5f);
    //
    //     Gizmos.color = Color.green;
    //     if (_collisionData.CheckCollisionRight())
    //     {
    //         Gizmos.color = Color.red;
    //         DrawCollisionLine(Vector2.right, _collisionData.rWallNormal.Perpendicular1());
    //     }
    //     Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * .5f);
    // }

    // private void DrawCollisionLine(Vector2 dir, Vector2 tangeant)
    // {
    //     var center = (Vector2)transform.position + dir * .5f;
    //     var start = center + tangeant * .5f;
    //     var end = center - tangeant * .5f;
    //     Gizmos.DrawLine(start, end);
    // }

    private bool CheckCos(Vector2 dir1, Vector2 dir2, float maxCos)
    {
        return Vector2.Dot(dir1, dir2) >= 1 - maxCos;
    }

    private void FixedUpdate()
    {
        _collisionData.Clear();
    }
}

public struct CollisionData
{
    public LayerMask leftCollision;
    public LayerMask rightCollision;
    public LayerMask upCollision;
    public LayerMask downCollision;

    public Vector2 floorNormal;
    public Vector2 lWallNormal;
    public Vector2 rWallNormal;
    public Vector2 ceilingNormal;

    public bool CheckCollisionUp(int layer = ~0)
    {
        return (upCollision.value & layer) != 0;
    }

    public bool CheckCollisionDown(int layer = ~0)
    {
        return (downCollision & layer) != 0;
    }

    public bool CheckCollisionLeft(int layer = ~0)
    {
        return (leftCollision.value & layer) != 0;
    }

    public bool CheckCollisionRight(int layer = ~0)
    {
        return (rightCollision.value & layer) != 0;
    }

    public void Clear()
    {
        leftCollision = 0;
        rightCollision = 0;
        upCollision = 0;
        downCollision = 0;

        floorNormal = Vector2.up;
        lWallNormal = Vector2.right;
        rWallNormal = Vector2.left;
        ceilingNormal = Vector2.down;
    }
    
    public static int LayerMaskToBitmask(LayerMask layerMask)
    {
        int bitmask = 0;

        for (int i = 0; i < 32; i++)
        {
            int layer = 1 << i;

            if ((layerMask & layer) != 0)
            {
                bitmask |= 1 << i;
            }
        }

        return bitmask;
    }
}

