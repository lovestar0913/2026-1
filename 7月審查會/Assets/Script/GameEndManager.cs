using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    private bool ending = false;


    private void Update()
    {
        if (ending)
            return;

        if (SongManager.Instance == null)
            return;

        if (NoteSpawner.Instance == null)
            return;

        if (!SongManager.Instance.Started)
            return;

        if (NoteSpawner.Instance.AllNotesFinished)
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        ending = true;


        //停止音樂
        if (SongManager.Instance != null)
        {
            SongManager.Instance.Stop();
        }



        TapJudge tapJudge =
            FindFirstObjectByType<TapJudge>();

        if (tapJudge != null)
            tapJudge.enabled = false;



        HoldJudge holdJudge =
            FindFirstObjectByType<HoldJudge>();

        if (holdJudge != null)
            holdJudge.enabled = false;



        JudgeRingManager ringManager =
            FindFirstObjectByType<JudgeRingManager>();

        if (ringManager != null)
            ringManager.enabled = false;



        //清除音符
        foreach (Note note in
            FindObjectsByType<Note>(
            FindObjectsSortMode.None))
        {
            Destroy(note.gameObject);
        }



        foreach (HoldNote hold in
            FindObjectsByType<HoldNote>(
            FindObjectsSortMode.None))
        {
            Destroy(hold.gameObject);
        }



        yield return new WaitForSeconds(2f);

        if (SongManager.Instance != null)
        {
            Destroy(
                SongManager.Instance.gameObject
            );
        }

        SceneManager.LoadScene("GameOver");
    }
}