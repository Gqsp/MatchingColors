using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] RoomHandler _startingRoom;
    [SerializeField] GameObject _player;
    [SerializeField] Timer _timer;
    
    [SerializeField] GameObject _tilemap;
    [SerializeField] LayerMask _orangeLayer;
    [SerializeField] LayerMask _blueLayer;
    
    public LayerMask TilemapLayer => 1 << _tilemap.layer;
    
    private void Awake()
    {
        _startingRoom.OnRoomPass(true);
        
        GameData.timer = 0;
        GameData.deaths = 0;
        GameData.collectables = 0;

        _tilemap.layer = GameData.startMode switch
        {
            StartMode.Bouncy => Mathf.RoundToInt(Mathf.Log(_orangeLayer.value, 2)),
            StartMode.Slippery => Mathf.RoundToInt(Mathf.Log(_blueLayer.value, 2)),
            _ => _tilemap.layer
        };
    }
    
    public void OnLevelEnd()
    {
        Debug.Log("Level end !");
        _timer.PauseTimer(true);
        GameData.timer = _timer.ElapsedTime;
        
        _player.SetActive(false);
        BackdropHandler.Instance.Require(1f, OnFadeOutComplete);
    }

    private void OnFadeOutComplete()
    {
        GameData.menuLoadMode = (MenuLoadMode)((int)GameData.startMode + 1);
        SceneManager.LoadScene("MainMenu");
    }
}
