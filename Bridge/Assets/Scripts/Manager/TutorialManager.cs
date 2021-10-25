using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using Data;



public enum TutorialType
{
    ImageDialog,
    Dialog,
    Cartoon,
    Summary
}

public class TutorialManager : MonoBehaviour
{
    public Text helpText;

    Coroutine tutorialCoroutine;
    public TutorialType tutorialType;
    public GameObject tutorialCanvas;
    public GameObject progressButton;
    public ImageDialog imageDialog;
    public Dialog dialog;
    public SummaryViewUI SummaryView;
    public CartoonViewUI cartoonView;
    Dictionary<int, List<DialogData>> dialogDataDictionary = new Dictionary<int, List<DialogData>>();

    public static TutorialManager instance = null;

    bool trigger = false; // false is left turn
   
    public bool skipMode = false;
    public bool inScript = false;
    int editorTutorialNumber = 0;


    public Button skipButton;
    UiController uiController;

    public Transform moveImage;
    public Transform buttonClickImage;
    public GameObject fingerImage;


    #region Scenario


    string[] bp_tutorial_stage_0;
    string[] rp_tutorial_stage_0;

    string[] bp_tutorial_stage_1;
    string[] rp_tutorial_stage_1;

    string[] bp_tutorial_stage_2;
    string[] rp_tutorial_stage_2;

    string[] bp_tutorial_stage_3;
    string[] rp_tutorial_stage_3;

    string[] bp_tutorial_stage_4;
    string[] rp_tutorial_stage_4;

    string[] bp_tutorial_stage_5;
    string[] rp_tutorial_stage_5;



    #endregion

    
    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        

        bp_tutorial_stage_0 = new string[]
            {"@. 첫 번째 튜토리얼이야!",
    "어디보자…. ‘이동 조작’을 알아보는 튜토리얼이래!",
    "아니! 헤헤, 펭이 설명해 줄 거니까!",
    "으엑! 일은 나만 시키려구?",
    "펭? 내 말 들리지?? 펭!!"};

        rp_tutorial_stage_0 = new string[]
            {"무슨 말인지 이해는 했니, 핑.",
    "어휴. @",
    "우리를 움직이려면 화면을 스와이프 해봐.",
    "우리는 어딘가에 부딪힐 때까지 방향을 바꿀 수 없어."};



        bp_tutorial_stage_1 = new string[]
            {"펭! 여기야, 여기!",
    "아깐 내가 청소했으니까"+Environment.NewLine+"이번엔 펭한테 맡기고 쉴까 해서!",
    "으엑! 우리 못 만나는 거야?",
    "둘 다 청소해야 하는 거였어?"};

        rp_tutorial_stage_1 = new string[]
            {"핑. 왜 이렇게 호들갑이야.",
    "여기를 봐.",
    "그래. 각자의 구역은 알아서 청소하는 거야.",
    "그래. @! 우리를 움직이는 방법은 알겠지?",
    "다른 펭귄을 움직이고 싶으면, 교체 버튼을 눌러줘."};

        bp_tutorial_stage_2 = new string[]
            {"펭! 보고 싶었어, 아까는 못 만나는 줄 알았다니깐.",
    "아! 그랬지!",
    "응! @, 이번에도 잘 부탁해!",
    "우리는 서로를 장애물처럼 이용할 수 있어!",
    "부딪히면 바로 앞에서 멈추지!",
    "왜애애앵. 나는 펭이 좋은걸!"};

        rp_tutorial_stage_2 = new string[]
           {"진정해 핑. 어차피 일 끝나고 같이 가잖아.",
    "이 구역은 간단하네. 빨리 끝나겠어.",
    "너무 들러붙지는 말자는 거지.",
    "이러니까 말이야. 어서 일을 시작하자, @."};

        bp_tutorial_stage_3 = new string[]
            {"여긴 아까랑 똑같은 구역이네?",
    "엥? 뭐가? 완전 똑같은데.",
    "괜찮아! 우리가 힘을 합하면…."};

        rp_tutorial_stage_3 = new string[]
      {"바보야, 다르잖아.",
    "아까랑 다른 곳에서 시작하잖아.",
    "저기 가운데 튀어나온 곳이 까다롭겠는걸.",
    "맞아. @, 잘 부탁해.",
    "우리를 잘 부딪혀서 해결해줘.",
      "처음이니까, 우리가 도와줄게."};



        bp_tutorial_stage_4 = new string[]
            {"와, 펭! 이번엔 2층이야! 어떻게 올라가지?",
    "날아서 올라갈 수는 없으려나?",
    "흐에엥, 내 동심을 지켜주란 말이야, 펭!",
    "응! 고마워, 펭!"};

        rp_tutorial_stage_4 = new string[]
           {"저기에 경사가 있네. 저기로 미끄러져 올라가자.",
    "우리는 펭귄이야. 그것도 마쉬멜로우 함량 98%.",
    "단언컨데, 날 수 없지.",
    "2층에서 1층으로 미끌어지면 떨어져버려.",
    "조심하라고, 핑."};

        bp_tutorial_stage_5 = new string[]
            {"펭! 어디있어? 안 보여!",
    "거기서 뭐해? 어서 내려와!",
    "정말! 어떻게 하지?",
    "업고 움직일 수도 있구나!",
    "대단해, 펭!",
    "실수해도 괜찮아, @!",
    "일시정지를 하고 처음부터 다시 시작할 수 있어!"};


        rp_tutorial_stage_5 = new string[]
            {"여기, 2층에 있어.",
    "바보야. 반대편에 치워야 하는 2층 언덕이 있잖아.",
    "….",
    "나를 업고 가야겠어, 핑.",
    "….",
    "잘 부탁해 @.",
    "떨어지면 다시 올라갈 수 없으니까."};

        

    }

    private void Start()
    {
        SetDialog();

        var nextStream = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Where(_ => inScript)
            .Subscribe(_ => NextProgressClicked());

    }

    void SetDialog()
    {
        List<DialogData> Stage1 = new List<DialogData>();
        Stage1.Add( new DialogData("ping", bp_tutorial_stage_0[0]));
        Stage1.Add( new DialogData("ping", bp_tutorial_stage_0[1]));
        Stage1.Add( new DialogData("peng", rp_tutorial_stage_0[0]));
        Stage1.Add( new DialogData("ping", bp_tutorial_stage_0[2]));
        Stage1.Add( new DialogData("peng", rp_tutorial_stage_0[1]));
        Stage1.Add( new DialogData("peng", rp_tutorial_stage_0[2]));
        Stage1.Add( new DialogData("ping", bp_tutorial_stage_0[3]));
        Stage1.Add( new DialogData("peng", rp_tutorial_stage_0[3]));
        Stage1.Add( new DialogData("ping", bp_tutorial_stage_0[4]));
        dialogDataDictionary.Add(1, Stage1);

        List<DialogData> Stage2 = new List<DialogData>();
        Stage2.Add( new DialogData("ping", bp_tutorial_stage_1[0]));
        Stage2.Add( new DialogData("peng", rp_tutorial_stage_1[0]));
        Stage2.Add( new DialogData("ping", bp_tutorial_stage_1[1]));
        Stage2.Add( new DialogData("peng", rp_tutorial_stage_1[1]));
        Stage2.Add( new DialogData("ping", bp_tutorial_stage_1[2]));
        Stage2.Add( new DialogData("peng", rp_tutorial_stage_1[2]));
        Stage2.Add( new DialogData("ping", bp_tutorial_stage_1[3]));
        Stage2.Add( new DialogData("peng", rp_tutorial_stage_1[3]));
        Stage2.Add( new DialogData("peng", rp_tutorial_stage_1[4]));
        dialogDataDictionary.Add(2, Stage2);

        List<DialogData> Stage3 = new List<DialogData>();
        Stage3.Add( new DialogData("ping", bp_tutorial_stage_2[0]));
        Stage3.Add( new DialogData("peng", rp_tutorial_stage_2[0]));
        Stage3.Add( new DialogData("ping", bp_tutorial_stage_2[1]));
        Stage3.Add( new DialogData("peng", rp_tutorial_stage_2[1]));
        Stage3.Add( new DialogData("ping", bp_tutorial_stage_2[2]));
        Stage3.Add( new DialogData("ping", bp_tutorial_stage_2[3]));
        Stage3.Add( new DialogData("ping", bp_tutorial_stage_2[4]));
        Stage3.Add( new DialogData("peng", rp_tutorial_stage_2[2]));
        Stage3.Add( new DialogData("ping", bp_tutorial_stage_2[5]));
        Stage3.Add( new DialogData("peng", rp_tutorial_stage_2[3]));
        dialogDataDictionary.Add(3, Stage3);

        List<DialogData> Stage4 = new List<DialogData>();
        Stage4.Add( new DialogData("ping", bp_tutorial_stage_3[0]));
        Stage4.Add( new DialogData("peng", rp_tutorial_stage_3[0]));
        Stage4.Add( new DialogData("ping", bp_tutorial_stage_3[1]));
        Stage4.Add( new DialogData("peng", rp_tutorial_stage_3[1]));
        Stage4.Add( new DialogData("peng", rp_tutorial_stage_3[2]));
        Stage4.Add( new DialogData("ping", bp_tutorial_stage_3[2]));
        Stage4.Add( new DialogData("peng", rp_tutorial_stage_3[3]));
        Stage4.Add( new DialogData("peng", rp_tutorial_stage_3[4]));
        Stage4.Add( new DialogData("peng", rp_tutorial_stage_3[5]));
        dialogDataDictionary.Add(4, Stage4);

        List<DialogData> Stage5 = new List<DialogData>();
        Stage5.Add( new DialogData("ping", bp_tutorial_stage_4[0]));
        Stage5.Add( new DialogData("peng", rp_tutorial_stage_4[0]));
        Stage5.Add( new DialogData("ping", bp_tutorial_stage_4[1]));
        Stage5.Add( new DialogData("peng", rp_tutorial_stage_4[1]));
        Stage5.Add( new DialogData("peng", rp_tutorial_stage_4[2]));
        Stage5.Add( new DialogData("ping", bp_tutorial_stage_4[2]));
        Stage5.Add( new DialogData("peng", rp_tutorial_stage_4[3]));
        Stage5.Add( new DialogData("peng", rp_tutorial_stage_4[4]));
        Stage5.Add( new DialogData("ping", bp_tutorial_stage_4[3]));
        dialogDataDictionary.Add(5, Stage5);

        List<DialogData> stage6 = new List<DialogData>();
        stage6.Add( new DialogData("ping", bp_tutorial_stage_5[0]));
        stage6.Add( new DialogData("peng", rp_tutorial_stage_5[0]));
        stage6.Add( new DialogData("ping", bp_tutorial_stage_5[1]));
        stage6.Add( new DialogData("peng", rp_tutorial_stage_5[1]));
        stage6.Add( new DialogData("ping", bp_tutorial_stage_5[2]));
        stage6.Add( new DialogData("peng", rp_tutorial_stage_5[2]));
        stage6.Add( new DialogData("peng", rp_tutorial_stage_5[3]));
        stage6.Add( new DialogData("ping", bp_tutorial_stage_5[3]));
        stage6.Add( new DialogData("ping", bp_tutorial_stage_5[4]));
        stage6.Add( new DialogData("peng", rp_tutorial_stage_5[4]));
        stage6.Add( new DialogData("peng", rp_tutorial_stage_5[5]));
        stage6.Add( new DialogData("peng", rp_tutorial_stage_5[6]));
        stage6.Add( new DialogData("ping", bp_tutorial_stage_5[5]));
        stage6.Add( new DialogData("ping", bp_tutorial_stage_5[6]));
        dialogDataDictionary.Add(6, stage6);


    }

    public void SkipButtonClicked()
    {
        switch (tutorialType)
        {
            case TutorialType.ImageDialog:
                
                break;
            case TutorialType.Dialog:
                
                break;
            case TutorialType.Cartoon:
                break;
            case TutorialType.Summary:
                
                break;
        }
        skipButton.interactable = false;

    }

    public void NextProgressClicked()
    {
        switch(tutorialType)
        {
            case TutorialType.ImageDialog:
                if(imageDialog.GetTexingIsEnd())
                {
                    imageDialog.NextDialog();
                }
                else
                {
                    imageDialog.SkipTexting();
                }
                break;
            case TutorialType.Dialog:
                if(dialog.GetTexingIsEnd())
                {
                    dialog.NextDialog();
                }
                else
                {
                    dialog.SkipTexting();
                }
                break;
            case TutorialType.Cartoon:
                cartoonView.SetEndCartoonView(true);
                break;
            case TutorialType.Summary:
                SummaryView.SetEndSummaryView(true);
                break;
        }
        
    }


    public IEnumerator StartImageDialog(List<ImageDialogData.Info> image)
    {
        tutorialType = TutorialType.ImageDialog;
        imageDialog.gameObject.SetActive(true);
        progressButton.SetActive(true);

        yield return StartCoroutine(imageDialog.ImageDialogIEnumerator(image));
        yield break;
    }

    public IEnumerator StartDialog(List<DialogData> dialogDatas)
    {
        tutorialType = TutorialType.Dialog;
        dialog.gameObject.SetActive(true);
        progressButton.SetActive(true);

        yield return StartCoroutine(dialog.DialogIEnumerator(dialogDatas));
        yield break;
    }

    public IEnumerator StartSummaryView(List<Sprite> summarySprites)
    {
        tutorialType = TutorialType.Summary;
        SummaryView.gameObject.SetActive(true);
        progressButton.SetActive(true);

        yield return StartCoroutine(SummaryView.SummaryViewIEnumerator(summarySprites));
        yield break;
    }

    public IEnumerator StartCartoonView(Sprite cartoon)
    {
        tutorialType = TutorialType.Cartoon;
        cartoonView.gameObject.SetActive(true);
        progressButton.SetActive(true);

        yield return StartCoroutine(cartoonView.CartoonViewIEnumerator(cartoon));
        yield break;
    }

    public IEnumerator StartTutorial(int number)
    {
        tutorialCanvas.SetActive(true);
        uiController = GameObject.Find("UiController").GetComponent<UiController>();
        GameController.Playing = true;
        inScript = false;


        ///동작 튜토리얼

        if (GameManager.instance.stageDataOnPlay.GetStageNumber() == 1)
        {
            fingerImage.SetActive(true);
            yield return StartCoroutine(MoveTutorial(1));
        }
        if (GameManager.instance.stageDataOnPlay.GetStageNumber() == 2)
        {
            yield return StartCoroutine(ChangeCharacter(GameController.instance.player2));
        }

        if (GameManager.instance.stageDataOnPlay.GetStageNumber() == 4)
        {
            yield return StartCoroutine(MoveTutorial(0));
            yield return StartCoroutine(ChangeCharacter(GameController.instance.player2));
            yield return StartCoroutine(MoveTutorial(1));
            yield return StartCoroutine(MoveTutorial(2));
            yield return StartCoroutine(MoveTutorial(3));
            yield return StartCoroutine(MoveTutorial(0));
            yield return StartCoroutine(MoveTutorial(1));
        }
        if (GameManager.instance.stageDataOnPlay.GetStageNumber() == 5)
        {
            yield return StartCoroutine(MoveTutorial(2));
            yield return StartCoroutine(MoveTutorial(1));
            yield return StartCoroutine(ChangeCharacter(GameController.instance.player2));
            yield return StartCoroutine(MoveTutorial(1));
            yield return StartCoroutine(ChangeCharacter(GameController.instance.player1));
            yield return StartCoroutine(MoveTutorial(0));
            yield return StartCoroutine(ChangeCharacter(GameController.instance.player2));
            yield return StartCoroutine(MoveTutorial(3));
            yield return StartCoroutine(MoveTutorial(2));
            yield return StartCoroutine(MoveTutorial(1));
            yield return StartCoroutine(ChangeCharacter(GameController.instance.player1));
            yield return StartCoroutine(MoveTutorial(3));
            yield return StartCoroutine(MoveTutorial(0));

        }
        if (GameManager.instance.stageDataOnPlay.GetStageNumber() == 6)
        {
            yield return StartCoroutine(ChangeCharacter(GameController.instance.player2));
            yield return StartCoroutine(MoveTutorial(2));
            yield return StartCoroutine(ChangeCharacter(GameController.instance.player1));
            yield return StartCoroutine(MoveTutorial(1));
            yield return StartCoroutine(MoveTutorial(0));
        }
        tutorialCanvas.SetActive(false);
        dialog.gameObject.SetActive(false);
        yield break;




    }

    


    //Tutorial Action

    IEnumerator ChangeCharacter(Player activate)
    {
        if(activate == GameController.instance.player1)
        {
            buttonClickImage.transform.position = uiController.player1.transform.position + Vector3.up * 50;
        }
        else
        {
            buttonClickImage.transform.position = uiController.player2.transform.position + Vector3.up * 50;
        }
        GameController.Playing = false;
        buttonClickImage.gameObject.SetActive(true);

        yield return new WaitUntil(() => GameController.instance.nowPlayer == activate);

        buttonClickImage.gameObject.SetActive(false);
        GameController.Playing = true;


        yield break;
    }
    IEnumerator MoveTutorial(int move)//입력받아야할 방향 0123 위 오른쪽 아래 왼쪽
    {
        bool temp_player1 = uiController.player1.interactable;
        bool temp_player2 = uiController.player2.interactable;

        uiController.player1.interactable = false;
        uiController.player2.interactable = false;


        GameController.Playing = false;
        //방향키 유아이
        moveImage.rotation = Quaternion.Euler(new Vector3(0, 0, -90 * move));
        moveImage.gameObject.SetActive(true);
        Debug.Log(move);
        yield return new WaitUntil(() => GameController.instance.unirx_dir == move);
        Debug.Log(GameController.instance.unirx_dir);
        uiController.player1.interactable = temp_player1;
        uiController.player2.interactable = temp_player2;

        fingerImage.SetActive(false);
        moveImage.gameObject.SetActive(false);
        GameController.Playing = true;
        GameController.instance.MakeMoveCommand();

        yield return new WaitUntil(() => !GameController.instance.nowPlayer.Moving() && !GameController.instance.nowPlayer.other.Moving());
        yield break;
    }

   
}
