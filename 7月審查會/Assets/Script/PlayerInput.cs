using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public JudgeManager judgeManager;

    public Note testNote;

    void Update()
    {
        int sector = GetKeySector();

        if (sector != -1)
        {
            judgeManager.Judge(testNote, sector);
        }
    }

    int GetKeySector()
    {
        if (Input.GetKeyDown(KeyCode.D)) return 0;
        if (Input.GetKeyDown(KeyCode.E)) return 1;
        if (Input.GetKeyDown(KeyCode.W)) return 2;
        if (Input.GetKeyDown(KeyCode.Q)) return 3;
        if (Input.GetKeyDown(KeyCode.A)) return 4;
        if (Input.GetKeyDown(KeyCode.S)) return 5;

        return -1;
    }
}