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

        UpdateHold();
    }

    void UpdateHold()
    {
        for (int i = NoteSpawner.Instance.activeHolds.Count - 1; i >= 0; i--)
        {
            HoldNote hold =
                NoteSpawner.Instance.activeHolds[i];

            if (hold == null)
            {
                NoteSpawner.Instance.activeHolds.RemoveAt(i);
                continue;
            }

            if (hold.finished)
            {
                NoteSpawner.Instance.activeHolds.RemoveAt(i);
                continue;
            }

            float now =
                SongManager.Instance.MusicTime;

            // 尚未開始
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

            bool pressedUp =
                hold.data.color == HoldColor.Red
                ? Input.GetMouseButtonUp(0)
                : Input.GetMouseButtonUp(1);

            // 第一次判定
            if (!hold.isHolding)
            {
                float error =
                    Mathf.Abs(now - hold.data.hitTime);

                if (now >
                    hold.data.hitTime +
                    GameManager.Instance.goodWindow)
                {
                    hold.Miss();

                    NoteSpawner.Instance.activeHolds.RemoveAt(i);

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

            // 是否在正確顏色
            bool onRed =
                redCircle.IsInsideTrack(
                    hold.judgeAngle);

            bool onBlue =
                blueCircle.IsInsideTrack(
                    hold.judgeAngle);

            if (hold.data.color == HoldColor.Red)
            {
                if (!onRed || onBlue)
                {
                    hold.Miss();

                    NoteSpawner.Instance.activeHolds.RemoveAt(i);

                    continue;
                }
            }
            else
            {
                if (!onBlue || onRed)
                {
                    hold.Miss();

                    NoteSpawner.Instance.activeHolds.RemoveAt(i);

                    continue;
                }
            }

            // 中途放開
            if (!pressed)
            {
                hold.Miss();

                NoteSpawner.Instance.activeHolds.RemoveAt(i);

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
                hold.finished = true;

                GameManager.Instance.Perfect();

                NoteSpawner.Instance.activeHolds.RemoveAt(i);

                Destroy(hold.gameObject);
            }
        }
    }
}