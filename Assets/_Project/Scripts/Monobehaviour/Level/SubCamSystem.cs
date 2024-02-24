using Cinemachine;
using UnityEngine;

public class SubCamSystem : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _camera;
    private void OnTriggerEnter2D(Collider2D other)
    {
        _camera.enabled = true;
        _camera.Priority = 11;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _camera.Priority = 10;
        _camera.enabled = false;
    }
}
