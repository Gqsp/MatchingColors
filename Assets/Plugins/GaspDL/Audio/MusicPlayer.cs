using GaspDL.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] string musicName;
    [SerializeField] string sfxName;

    private void Start()
    {
        AudioManager.Instance.Play(musicName);

        Destroy(gameObject);
    }
}
