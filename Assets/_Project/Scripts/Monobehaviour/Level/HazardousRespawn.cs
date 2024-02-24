using UnityEngine;

public class HazardousRespawn : MonoBehaviour
{
    private RespawnHandler _respawnHandler;
    private Vector3 _respawnPoint;
    
    private void Awake()
    {
        _respawnHandler = GetComponentInParent<RespawnHandler>();
        _respawnPoint = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity).point;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _respawnHandler.SetNewRespawnPoint(this);
    }
    
    public void RespawnPlayer(GameObject player)
    {
        player.transform.position = _respawnPoint;
    }
}
