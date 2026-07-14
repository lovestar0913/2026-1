using UnityEngine;

public class TestMusicTime : MonoBehaviour
{
    void Update()
    {
        Debug.Log(SongManager.Instance.MusicTime);
    }
}