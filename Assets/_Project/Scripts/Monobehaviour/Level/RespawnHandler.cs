using UnityEngine;

public class RespawnHandler : MonoBehaviour
{
    [SerializeField] private HazardousRespawn _hazardousRespawn;
    private Vector3 _startPosition;
    
    private void Start()
    {
        // Find Object with tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _startPosition = player.transform.position;
        }
    }
    
    public void SetNewRespawnPoint(HazardousRespawn newRespawnPoint)
    {
        _hazardousRespawn = newRespawnPoint;
    }
    
    public void RespawnPlayer(GameObject player)
    {
        if (_hazardousRespawn == null)
        {
            player.transform.position = _startPosition;
            return;
        }
        _hazardousRespawn.RespawnPlayer(player);
    }
}
