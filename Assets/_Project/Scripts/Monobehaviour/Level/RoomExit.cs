using UnityEngine;

public class RoomExit : MonoBehaviour
{
    RoomHandler _roomHandler;
    [SerializeField] RoomHandler _connectedRoom;
    [SerializeField] ExitDirection _exitDirection;
    private enum ExitDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    
    private void Awake()
    {
        _roomHandler = GetComponentInParent<RoomHandler>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var position = other.transform.position;
        
        if (GetRelativeSide(position) == _exitDirection)
        {
            _roomHandler.OnRoomPass(false);
            _connectedRoom.OnRoomPass(true);
        }
    }

    private ExitDirection GetRelativeSide(Vector3 position)
    {
        return _exitDirection is ExitDirection.Up or ExitDirection.Down ? GetRelativeSideVertical(position) : GetRelativeSideHorizontal(position);
    }
    
    private ExitDirection GetRelativeSideHorizontal(Vector3 position)
    {
        var relativeX = position.x - transform.position.x;
        return relativeX > 0 ? ExitDirection.Right : ExitDirection.Left;
    }

    private ExitDirection GetRelativeSideVertical(Vector3 position)
    {
        var relativeY = position.y - transform.position.y;
        return relativeY > 0 ? ExitDirection.Up : ExitDirection.Down;
    }
}
