using GaspDL;
using GaspDL.SaveSystem;

[System.Serializable]
public class GameProgression : ISaveable
{
    public PersistentID ID => throw new System.NotImplementedException();
    public DataType DataType { get; set; }
    public Optional<int> RememberDistance { get; set; }
    public Optional<string> SceneName { get; set; }

    public object CaptureState()
    {
        throw new System.NotImplementedException();
    }

    public void RestoreState(object state)
    {
        throw new System.NotImplementedException();
    }
}
