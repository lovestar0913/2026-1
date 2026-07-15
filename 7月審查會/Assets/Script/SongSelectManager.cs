using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class SongSelectManager : MonoBehaviour
{
    public static SongData selectedSong;


    [Header("Songs")]
    public List<SongData> songs;


    [Header("Screen")]
    public SpriteRenderer leftScreen;
    public SpriteRenderer centerScreen;
    public SpriteRenderer rightScreen;
    public SpriteRenderer backScreen;


    [Header("Song Name")]
    public Text songName;

    [Header("Hard Button")]
    public GameObject hardButton;


    [Header("Preview")]
    public AudioSource previewAudio;
    public float previewDelay = 0.5f;


    [Header("Rotate")]
    public float rotateTime = 0.4f;
    public float arcHeight = 2f;


    private int index = 0;

    private bool moving = false;


    // =====================
    // 四個槽位
    // =====================


    Vector3 leftPos =
        new Vector3(-6, 0, -7);

    Vector3 centerPos =
        new Vector3(0, 0, 0);

    Vector3 rightPos =
        new Vector3(6, 0, -7);

    Vector3 backPos =
        new Vector3(0, 0, -15);



    Quaternion leftRot =
        Quaternion.Euler(0, -50, 0);

    Quaternion centerRot =
        Quaternion.identity;

    Quaternion rightRot =
        Quaternion.Euler(0, 50, 0);

    Quaternion backRot =
        Quaternion.Euler(0, 180, 0);


    Vector3 leftScale =
        Vector3.one * 0.75f;

    Vector3 centerScale =
        Vector3.one * 1.2f;

    Vector3 rightScale =
        Vector3.one * 0.75f;

    Vector3 backScale =
        Vector3.one * 0.5f;

    void Start()
    {
        if (songs.Count == 0)
        {
            Debug.LogError("沒有歌曲");
            return;
        }


        UpdateScreen();
    }

    void Update()
    {
        if (moving)
            return;


        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(RotateNext());
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(RotatePrevious());
        }
    }

    // =====================
    // 下一首
    // =====================

    IEnumerator RotateNext()
    {
        moving = true;


        StopPreview();

        HideUI();



        yield return StartCoroutine(
            Rotate(1)
        );



        index++;

        if (index >= songs.Count)
            index = 0;



        UpdateScreen();



        yield return
            new WaitForSeconds(previewDelay);



        PlayPreview();



        ShowUI();


        moving = false;
    }

    // =====================
    // 上一首
    // =====================

    IEnumerator RotatePrevious()
    {
        moving = true;


        StopPreview();

        HideUI();



        yield return StartCoroutine(
            Rotate(-1)
        );



        index--;

        if (index < 0)
            index = songs.Count - 1;



        UpdateScreen();



        yield return
            new WaitForSeconds(previewDelay);



        PlayPreview();



        ShowUI();


        moving = false;
    }

    // =====================
    // 旋轉
    // dir 1 = D
    // dir -1 = A
    // =====================

    IEnumerator Rotate(int dir)
    {
        float time = 0;



        Vector3 lp =
            leftScreen.transform.position;

        Vector3 cp =
            centerScreen.transform.position;

        Vector3 rp =
            rightScreen.transform.position;

        Vector3 bp =
            backScreen.transform.position;



        Quaternion lr =
            leftScreen.transform.rotation;

        Quaternion cr =
            centerScreen.transform.rotation;

        Quaternion rr =
            rightScreen.transform.rotation;

        Quaternion br =
            backScreen.transform.rotation;



        Vector3 ls =
            leftScreen.transform.localScale;

        Vector3 cs =
            centerScreen.transform.localScale;

        Vector3 rs =
            rightScreen.transform.localScale;

        Vector3 bs =
            backScreen.transform.localScale;



        while (time < rotateTime)
        {
            time += Time.deltaTime;


            float t =
                time / rotateTime;



            if (dir == 1)
            {
                // Left -> Back

                Move(
                    leftScreen,
                    lp,
                    backPos,
                    lr,
                    backRot,
                    ls,
                    backScale,
                    t
                );


                // Center -> Left

                Move(
                    centerScreen,
                    cp,
                    leftPos,
                    cr,
                    leftRot,
                    cs,
                    leftScale,
                    t
                );


                // Right -> Center

                Move(
                    rightScreen,
                    rp,
                    centerPos,
                    rr,
                    centerRot,
                    rs,
                    centerScale,
                    t
                );


                // Back -> Right

                Move(
                    backScreen,
                    bp,
                    rightPos,
                    br,
                    rightRot,
                    bs,
                    rightScale,
                    t
                );
            }
            else
            {
                // Right -> Back

                Move(
                    rightScreen,
                    rp,
                    backPos,
                    rr,
                    backRot,
                    rs,
                    backScale,
                    t
                );


                // Center -> Right

                Move(
                    centerScreen,
                    cp,
                    rightPos,
                    cr,
                    rightRot,
                    cs,
                    rightScale,
                    t
                );


                // Left -> Center

                Move(
                    leftScreen,
                    lp,
                    centerPos,
                    lr,
                    centerRot,
                    ls,
                    centerScale,
                    t
                );


                // Back -> Left

                Move(
                    backScreen,
                    bp,
                    leftPos,
                    br,
                    leftRot,
                    bs,
                    leftScale,
                    t
                );
            }


            yield return null;
        }


        // 非常重要
        // 交換槽位

        if (dir == 1)
            SwapNext();
        else
            SwapPrevious();
    }

    void Move(
        SpriteRenderer obj,
        Vector3 startPos,
        Vector3 endPos,
        Quaternion startRot,
        Quaternion endRot,
        Vector3 startScale,
        Vector3 endScale,
        float t)
    {
        obj.transform.position =
            ArcMove(
                startPos,
                endPos,
                arcHeight,
                t
            );


        obj.transform.rotation =
            Quaternion.Lerp(
                startRot,
                endRot,
                t
            );


        obj.transform.localScale =
            Vector3.Lerp(
                startScale,
                endScale,
                t
            );
    }

    Vector3 ArcMove(
        Vector3 start,
        Vector3 end,
        float height,
        float t)
    {
        Vector3 pos =
            Vector3.Lerp(
                start,
                end,
                t
            );


        pos.y +=
            Mathf.Sin(
                t * Mathf.PI
            ) * height;


        return pos;
    }

    // =====================
    // 交換螢幕引用
    // =====================


    void SwapNext()
    {
        SpriteRenderer temp =
            backScreen;


        backScreen =
            leftScreen;


        leftScreen =
            centerScreen;


        centerScreen =
            rightScreen;


        rightScreen =
            temp;
    }

    void SwapPrevious()
    {
        SpriteRenderer temp =
            leftScreen;


        leftScreen =
            backScreen;


        backScreen =
            rightScreen;


        rightScreen =
            centerScreen;


        centerScreen =
            temp;
    }

    void UpdateScreen()
    {
        int left =
            index - 1;


        if (left < 0)
            left = songs.Count - 1;



        int right =
            index + 1;


        if (right >= songs.Count)
            right = 0;



        centerScreen.sprite =
            songs[index].cover;


        leftScreen.sprite =
            songs[left].cover;


        rightScreen.sprite =
            songs[right].cover;
    }

    void PlayPreview()
    {
        if (previewAudio == null)
            return;


        previewAudio.clip =
            songs[index].music;


        previewAudio.Play();
    }

    void StopPreview()
    {
        if (previewAudio)
            previewAudio.Stop();
    }

    void HideUI()
    {
        songName.gameObject.SetActive(false);


        if (hardButton)
            hardButton.SetActive(false);
    }

    void ShowUI()
    {
        songName.gameObject.SetActive(true);


        if (hardButton)
            hardButton.SetActive(true);


        songName.text =
            songs[index].songName;
    }

    public void StartHard()
    {
        selectedSong =
            songs[index];


        SceneManager.LoadScene("Play");
    }
}