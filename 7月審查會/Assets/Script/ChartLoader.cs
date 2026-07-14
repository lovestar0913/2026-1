using UnityEngine;

public class ChartLoader : MonoBehaviour
{
    public static ChartLoader Instance;

    [Header("目前譜面")]
    public ChartData chartData;

    private void Awake()
    {
        Instance = this;
    }

    public ChartData LoadChart()
    {
        return chartData;
        //Resources.Load<ChartData>("Tutorial");
    }
}