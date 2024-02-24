using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] ExitDirection _exitDirection;
    
    private LevelHandler _levelManager;
    private enum ExitDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelHandler>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var position = other.transform.position;
        Debug.Log("Entered ! " + GetRelativeSide(position) + " " + _exitDirection);
        if (GetRelativeSide(position) == _exitDirection)
        {
            _levelManager.OnLevelEnd();
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
        Debug.Log(position.y + " " + transform.position.y);
        var relativeY = position.y - transform.position.y;
        return relativeY > 0 ? ExitDirection.Up : ExitDirection.Down;
    }
}
