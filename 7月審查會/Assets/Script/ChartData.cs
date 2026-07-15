using System;
using System.Collections.Generic;


[Serializable]
public class ChartData
{
    // 歌曲資訊
    public string songName;

    public float bpm;

    public float offset;


    // 所有音符
    public List<NoteData> notes = new();
}



[Serializable]
public class NoteData
{
    // 軌道
    public Lane lane;


    // 判定時間
    public float hitTime;


    // 音符種類
    public NoteType noteType = NoteType.Tap;


    // 判定形狀
    public JudgeShape judgeShape = JudgeShape.Circle;



    // =================
    // Hold資料
    // =================

    // 結束時間
    public float endTime;


    // 出生角度
    public float startAngle;


    // 判定角度
    public float endAngle;


    // 顏色
    public HoldColor color;
}