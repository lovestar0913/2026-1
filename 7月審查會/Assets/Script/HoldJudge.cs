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
        for (int i = GameManager.Instance.activeHolds.Count - 1; i >= 0; i--)
        {
            HoldNote hold =
                GameManager.Instance.activeHolds[i];

            if (hold == null)
            {
                GameManager.Instance.activeHolds.RemoveAt(i);
                continue;
            }

            if (hold.finished)
            {
                GameManager.Instance.activeHolds.RemoveAt(i);
                continue;
            }

            float now =
                SongManager.Instance.MusicTime;

            //------------------------------------------------
            // 尚未開始
            //------------------------------------------------

            if (now < hold.data.hitTime)
                continue;

            //------------------------------------------------
            // 判斷按鍵
            //------------------------------------------------

            bool pressed =
                hold.data.color == HoldColor.Red
                ? Input.GetMouseButton(0)
                : Input.GetMouseButton(1);

            //------------------------------------------------
            // 第一次判定
            //------------------------------------------------

            if (!hold.isHolding)
            {
                if (now >
                    hold.data.hitTime +
                    GameManager.Instance.goodWindow)
                {
                    hold.Miss();

                    GameManager.Instance.activeHolds.RemoveAt(i);

                    continue;
                }

                if (pressed)
                {
                    hold.StartHold();

                    hold.nextTickTime =
                        now + tickInterval;
                }

                continue;
            }

            //------------------------------------------------
            // 是否仍在正確顏色
            //------------------------------------------------

            bool onRed =
                redCircle.IsInsideTrack(
                    hold.judgeAngle);

            bool onBlue =
                blueCircle.IsInsideTrack(
                    hold.judgeAngle);

            if (hold.data.color == HoldColor.Red)
            {
                // 離開紅圈或跑到藍圈
                if (!onRed || onBlue)
                {
                    hold.Miss();

                    GameManager.Instance.activeHolds.RemoveAt(i);

                    continue;
                }
            }
            else
            {
                // 離開藍圈或跑到紅圈
                if (!onBlue || onRed)
                {
                    hold.Miss();

                    GameManager.Instance.activeHolds.RemoveAt(i);

                    continue;
                }
            }

            //------------------------------------------------
            // 中途放開
            //------------------------------------------------

            if (!pressed)
            {
                hold.Miss();

                GameManager.Instance.activeHolds.RemoveAt(i);

                continue;
            }

            //------------------------------------------------
            // Tick 加分
            //------------------------------------------------

            if (now >= hold.nextTickTime &&
                now < hold.data.endTime)
            {
                GameManager.Instance.HoldTick(200);

                hold.nextTickTime += tickInterval;
            }

            //------------------------------------------------
            // Hold 完成
            //------------------------------------------------

            if (now >= hold.data.endTime)
            {
                hold.finished = true;

                GameManager.Instance.Perfect();

                GameManager.Instance.activeHolds.RemoveAt(i);

                Destroy(hold.gameObject);
            }
        }
    }
}