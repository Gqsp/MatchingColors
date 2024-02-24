using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode
{
    public IEnumerator OnEnd();
    public IEnumerator OnStart();
    public IEnumerator OnEditorStart();
}
