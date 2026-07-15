using UnityEngine;


[System.Serializable]
public class SongData
{
    [Header("Song Info")]
    public string songName;


    [Header("Cover")]
    public Sprite cover;


    [Header("Audio")]
    public AudioClip music;


    [Header("Chart JSON")]
    public TextAsset chart;
}