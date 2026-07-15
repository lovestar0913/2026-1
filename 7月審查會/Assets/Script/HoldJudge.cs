using UnityEngine;

public class HoldJudge : MonoBehaviour
{
    [Header("Hold Tick")]
    public float tickInterval = 0.5f;


    [Header("Circle")]
    public CircleTrack redCircle;
    public CircleTrack blueCircle;


    private void Update()
    {
        if (SongManager.Instance == null)
            return;

        if (!SongManager.Instance.Started)
            return;

        UpdateHold();
    }


    // 安全移除 Hold
    void RemoveHold(int index)
    {
        if (index >= 0 &&
            index < NoteSpawner.Instance.activeHolds.Count)
        {
            NoteSpawner.Instance.activeHolds.RemoveAt(index);
        }
    }


    void UpdateHold()
    {
        // 倒序刪除，避免 List 位移問題
        for (int i = NoteSpawner.Instance.activeHolds.Count - 1;
             i >= 0;
             i--)
        {
            HoldNote hold =
                NoteSpawner.Instance.activeHolds[i];


            // 防止空物件
            if (hold == null)
            {
                RemoveHold(i);
                continue;
            }

            // 已完成
            if (hold.finished)
            {
                RemoveHold(i);
                continue;
            }

            float now =
                SongManager.Instance.MusicTime;


            // 尚未到判定時間
            if (now < hold.data.hitTime)
                continue;


            // 按鍵狀態
            bool pressed =
                hold.data.color == HoldColor.Red
                ? Input.GetMouseButton(0)
                : Input.GetMouseButton(1);

            bool pressedDown =
                hold.data.color == HoldColor.Red
                ? Input.GetMouseButtonDown(0)
                : Input.GetMouseButtonDown(1);


            // 第一次按下判定
            if (!hold.isHolding)
            {
                float error =
                    Mathf.Abs(now - hold.data.hitTime);

                // 超過 Good 判定時間
                if (now >
                    hold.data.hitTime +
                    GameManager.Instance.goodWindow)
                {
                    hold.Miss();

                    RemoveHold(i);

                    continue;
                }

                if (pressedDown)
                {
                    if (error <= GameManager.Instance.perfectWindow)
                    {
                        GameManager.Instance.Perfect();
                    }
                    else if (error <= GameManager.Instance.greatWindow)
                    {
                        GameManager.Instance.Great();
                    }
                    else if (error <= GameManager.Instance.goodWindow)
                    {
                        GameManager.Instance.Good();
                    }
                    else
                    {
                        continue;
                    }

                    hold.StartHold();

                    hold.nextTickTime =
                        now + tickInterval;
                }

                continue;
            }

            // Hold 中判定
            bool onRed =
                redCircle.IsInsideTrack(
                    hold.judgeAngle);

            bool onBlue =
                blueCircle.IsInsideTrack(
                    hold.judgeAngle);

            // 顏色判定
            if (hold.data.color == HoldColor.Red)
            {
                if (!onRed || onBlue)
                {
                    hold.Miss();

                    RemoveHold(i);

                    continue;
                }
            }
            else
            {
                if (!onBlue || onRed)
                {
                    hold.Miss();

                    RemoveHold(i);

                    continue;
                }
            }

            // 放開滑鼠
            if (!pressed)
            {
                hold.Miss();

                RemoveHold(i);

                continue;
            }

            // Tick 加分
            if (now >= hold.nextTickTime &&
                now < hold.data.endTime)
            {
                GameManager.Instance.HoldTick(200);


                hold.nextTickTime += tickInterval;
            }

            // Hold 完成
            if (now >= hold.data.endTime)
            {
                GameManager.Instance.Perfect();

                hold.finished = true;

                Destroy(hold.gameObject);

                RemoveHold(i);

                continue;
            }
        }
    }
}