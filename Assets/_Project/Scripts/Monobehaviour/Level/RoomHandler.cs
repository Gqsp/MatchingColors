using System.Collections;
using Cinemachine;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] RoomExit[] _exits;
    [SerializeField] CinemachineVirtualCamera _camera;
    
    bool _isActive;

    private void Start()
    {
        if (!_isActive)
        {
            OnRoomPass(false);
        }
    }

    private void OnValidate()
    {
        _exits = GetComponentsInChildren<RoomExit>();
    }

    public void OnRoomPass(bool enter)
    {
        _camera.Priority = 10 + (enter ? 1 : 0);
        foreach (var exit in _exits)
        {
            exit.gameObject.SetActive(enter);
        }
        
        if (enter) StartCoroutine(StopTimeDuringTransition(0.5f));
        _isActive = enter;
    }

    IEnumerator StopTimeDuringTransition(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }
}
