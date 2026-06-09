using UnityEngine;

public class TestMusicTime : MonoBehaviour
{
    void Update()
    {
        Debug.Log(GameManager.Instance.MusicTime);
    }
}