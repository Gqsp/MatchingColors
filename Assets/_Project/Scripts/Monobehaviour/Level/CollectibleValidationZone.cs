using UnityEngine;

public class CollectibleValidationZone : MonoBehaviour
{
    Collectible _collectible;

    private void Start()
    {
        _collectible = GetComponentInParent<Collectible>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _collectible.OnPlayerEnterValidationZone();
    }
}
