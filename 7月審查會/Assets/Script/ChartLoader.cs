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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        LoadChart();
    }



    private void LoadChart()
    {
        if (chartFile == null)
        {
            Debug.LogError("沒有指定 .chart 檔案");
            return;
        }



        chartData =
            JsonUtility.FromJson<ChartData>(
                chartFile.text
            );



        if (chartData == null)
        {
            Debug.LogError("Chart 解析失敗");
            return;
        }



        IsLoaded = true;



        Debug.Log(
            "Chart Loaded : "
            + chartData.songName
        );
    }



    public ChartData GetChart()
    {
        return chartData;
    }
}