using UnityEngine;

public class TapJudge : MonoBehaviour
{
    private void Update()
    {
        if (SongManager.Instance == null)
            return;

        if (!SongManager.Instance.Started)
            return;


        if (Input.GetKeyDown(KeyCode.Q))
            Judge(Lane.Q);

        if (Input.GetKeyDown(KeyCode.W))
            Judge(Lane.W);

        if (Input.GetKeyDown(KeyCode.E))
            Judge(Lane.E);

        if (Input.GetKeyDown(KeyCode.D))
            Judge(Lane.D);

        if (Input.GetKeyDown(KeyCode.S))
            Judge(Lane.S);

        if (Input.GetKeyDown(KeyCode.A))
            Judge(Lane.A);


        CheckMiss();
    }



    //=========================
    // Tap 判定
    //=========================

    void Judge(Lane lane)
    {
        Note target = null;

        float smallestError = Mathf.Infinity;



        foreach (Note note in NoteSpawner.Instance.activeNotes)
        {
            if (note == null)
                continue;


            if (note.judged)
                continue;


            if (note.lane != lane)
                continue;



            float error =
                Mathf.Abs(
                    SongManager.Instance.MusicTime
                    -
                    note.hitTime
                );


            if (error < smallestError)
            {
                smallestError = error;
                target = note;
            }
        }



        if (target == null)
            return;



        if (smallestError <= GameManager.Instance.perfectWindow)
        {
            GameManager.Instance.Perfect();
        }
        else if (smallestError <= GameManager.Instance.greatWindow)
        {
            GameManager.Instance.Great();
        }
        else if (smallestError <= GameManager.Instance.goodWindow)
        {
            GameManager.Instance.Good();
        }
        else
        {
            return;
        }



        target.judged = true;


        NoteSpawner.Instance.activeNotes.Remove(target);


        Destroy(target.gameObject);
    }



    //=========================
    // Miss 判定
    //=========================

    void CheckMiss()
    {
        float now =
            SongManager.Instance.MusicTime;



        for (int i = NoteSpawner.Instance.activeNotes.Count - 1;
            i >= 0;
            i--)
        {
            Note note =
                NoteSpawner.Instance.activeNotes[i];



            if (note == null)
            {
                NoteSpawner.Instance.activeNotes.RemoveAt(i);
                continue;
            }



            if (note.judged)
                continue;



            /*
             * 防止剛生成就 Miss
             *
             * 音符真正判定時間:
             * hitTime + goodWindow
             */
            if (now >
               note.hitTime +
               GameManager.Instance.goodWindow)
            {

                Debug.Log(
                    "MISS => "
                    +
                    note.name
                    +
                    " Now:"
                    +
                    now
                    +
                    " Hit:"
                    +
                    note.hitTime
                );


                note.judged = true;


                GameManager.Instance.Miss();


                NoteSpawner.Instance.activeNotes.RemoveAt(i);


                Destroy(note.gameObject);
            }
        }
    }
}