using UnityEngine;


public class ChartLoader : MonoBehaviour
{
    public static ChartLoader Instance;


    [Header("Chart File")]
    public TextAsset chartFile;


    private ChartData chartData;


    public bool IsLoaded { get; private set; }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        LoadChart();
    }



    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }



    //=========================
    // Load Chart
    //=========================

    public void LoadChart()
    {
        IsLoaded = false;


        if (chartFile == null)
        {
            Debug.LogError(
                "沒有指定 Chart File"
            );

            return;
        }



        chartData =
            JsonUtility.FromJson<ChartData>(
                chartFile.text
            );



        if (chartData == null)
        {
            Debug.LogError(
                "Chart 解析失敗"
            );

            return;
        }



        if (chartData.notes == null)
        {
            Debug.LogError(
                "Chart 沒有 notes 資料"
            );

            return;
        }



        IsLoaded = true;


    }



    //重新讀取歌曲
    public void ReloadChart()
    {
        LoadChart();
    }



    public ChartData GetChart()
    {
        if (!IsLoaded)
        {
            Debug.LogWarning(
                "Chart 尚未載入"
            );

            return null;
        }


        return chartData;
    }
}