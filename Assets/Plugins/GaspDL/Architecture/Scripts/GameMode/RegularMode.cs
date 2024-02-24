using System.Collections;

public class RegularMode : IGameMode
{
    private GameModeState _state = GameModeState.Ended;
    private int _saveId; 

    public IEnumerator OnEnd()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator OnStart()
    {
        if (_state != GameModeState.Ended) yield break;
        _state = GameModeState.Starting;

        App.SaveManager.LoadData(_saveId);
    }

    public IEnumerator OnEditorStart()
    {
        throw new System.NotImplementedException();
    }
}
