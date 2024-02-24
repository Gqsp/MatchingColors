using UnityEngine;

public class SecretTrigger : MonoBehaviour
{
    private SecretPassage _secretPassage;

    private void Awake()
    {
        _secretPassage = GetComponentInParent<SecretPassage>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _secretPassage.EnterSecret();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _secretPassage.ExitSecret();
    }
}
