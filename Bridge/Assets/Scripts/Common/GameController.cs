using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.EventSystems;
using System.Linq;
using Data;

public class GameController : MonoBehaviour
{
    

    public static GameController instance;
    XMLManager xmlManager = XMLManager.ins;
    AWSManager awsManager = AWSManager.instance;
    CSVManager csvManager = CSVManager.instance;
    [Header("Controller Field")]
    public CameraController cameraController;
    public UiController ui;

    [Header("Game Component Field")]
    public MapLoader mapLoader;
    public Player player1;
    public Player player2;
    public Player nowPlayer;

    StageData stageData;
    public StageData GetStageData() { return stageData; }
    Map map;
    public Map GetMap() { return map; }
    public int snow_total, snow_remain;
    public int moveCount;
    public bool isSuccess;
  
    [Header ("For Achievement Datas")]
    int dropCount; // 몇번 떨어졌는지
    int crashCount; // 몇번 부딪혔는지 (캐릭터)
    int carryCount; // 몇번 업었는지
    int resetCount; // 몇번 되돌렸는지
    int crackCount; // 몇번 크래커를 부셨는지
    int cloudCount; // 몇번 솜사탕을 탔는지

    [SerializeField]
    int parfaitOrder;
    public static int ParfaitOrder
    {
        get => instance.parfaitOrder;
        set => instance.parfaitOrder = value;
    }

    public bool customMode;
    public bool editorMode;
    public int unirx_dir;



    private bool isPlaying;
    public static bool Playing
    {
        get => instance.isPlaying;
        set => instance.isPlaying = value;
    }

    public float startTime, endTime;



    SoundManager soundManager;
    GameManager gameManager = GameManager.instance;
    JsonAdapter jsonAdapter = JsonAdapter.instance;
    

    [SerializeField]
    Vector2 down;
    [SerializeField]
    Vector2 up;
    [SerializeField]
    bool click;

    InputHandler handler;
    public MoveCommand moveCommand;
    public TutorialManager tutorialManager;

    int star = 3;

    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        handler = new InputHandler();
        

    }

    void Start()
    {
        xmlManager = XMLManager.ins;
        gameManager = GameManager.instance;
        soundManager = SoundManager.instance;
        awsManager = AWSManager.instance;
        jsonAdapter = JsonAdapter.instance;
        csvManager = CSVManager.instance;

        unirx_dir = -1;
        

        SwipeStream();
        GameSetting();

        MoPubManager.OnInterstitialLoadedEvent += InterstitialLoadCallback;
        MoPubManager.OnInterstitialFailedEvent += InterstitialFailedCallback;

        this.ObserveEveryValueChanged(x => parfaitOrder)
            .Subscribe(x => ui.ParfaitDone(parfaitOrder));

        
    }

    void InterstitialFailedCallback(string adUnitId, string error)
    {
        Debug.LogFormat("Ad failed {0} , {1}", adUnitId, error);
    }
    
    void SwipeStream()
    {



#if UNITY_ANDROID || UNITY_IOS

        var touchDownStream = this.UpdateAsObservable()
          
            .Where(_ => !click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => !EventSystem.current.IsPointerOverGameObject(0))
            .Where(_ => Input.GetTouch(0).phase == TouchPhase.Began)
            .Select(_ => Input.GetTouch(0))
            .Subscribe(_ => { down = _.position; click = true; } );

        var touchUpStream = this.UpdateAsObservable()
         
            .Where(_ => click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => Input.GetTouch(0).phase == TouchPhase.Ended)
            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { up = _; MakeDirectionBySlide(); MakeMoveCommand(); click = false; });

#else
        var mouseDownStream = this.UpdateAsObservable()
                .Where(_ => !EventSystem.current.IsPointerOverGameObject()
                    && !click
                    && Input.GetMouseButtonDown(0))

                .Select(_ => Input.mousePosition)
                .Subscribe(_ => { down = _; click = true; });

        var mouseUpStream = this.UpdateAsObservable()
            .Where(_ => click && Input.GetMouseButtonUp(0))

            .Select(_ => Input.mousePosition)
            .Subscribe(_ => {
                up = _;
                MakeDirectionBySlide();
                Debug.Log(GameController.instance.unirx_dir);
                MakeMoveCommand();
                click = false;
            });

        var KeyStream = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.LeftArrow))
            .Subscribe(_ => {
                MakeDirectionByKeyBoard();
                MakeMoveCommand();
            });

#endif

    }

    void MakeDirectionByKeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            unirx_dir = 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            unirx_dir = 0;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            unirx_dir = 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            unirx_dir = 3;
        }
        else
        {
            unirx_dir = -1;
        }
    }

    void MakeDirectionBySlide()
    {
        if(Vector2.Distance(up, down) <= 30)//민감도
        {
            unirx_dir = -1;
            return;
        }
        Vector2 normalized = (up - down).normalized;


        if (normalized.x < -0.5)
        {
            unirx_dir = 3;
            //isMove = PlayerControl(3); //left
        }
        else if (normalized.x > 0.5)
        {
            unirx_dir = 1;
            //isMove = PlayerControl(1); //right
        }
        else
        {
            if (normalized.y > 0)
            {
                unirx_dir = 0;
                //isMove = PlayerControl(0); //up

            }
            else
            {
                unirx_dir = 2;
                //isMove = PlayerControl(2); // down
            }

        }

        Debug.Log(unirx_dir);
    }

   
    public void MakeMoveCommand()
    {
       
        if (!player1.Moving() && !player2.Moving() && isPlaying && unirx_dir != -1)
        {
            moveCommand = new MoveCommand(nowPlayer, map, unirx_dir);
            List<Tuple<Vector3, int>> targetPositions = nowPlayer.GetTargetPositions(map, unirx_dir);
            //캐릭터가 1칸도 이동할 수 없고, 캐릭터의 상태가 Master -> Idle로 변경 되어야 함.
            if(targetPositions.Count == 0 && nowPlayer.stateChange)
            {
                nowPlayer.stateChange = false;
                moveCommand = new MoveCommand(nowPlayer.other, map, unirx_dir);
                List<Tuple<Vector3, int>> otherTargetPositions = nowPlayer.other.GetTargetPositions(map, unirx_dir);
                //다른 캐락터도 1칸도 이동할 수 없다면, 종료
                if(otherTargetPositions.Count == 0)
                {
                    return;
                }
                // 캐릭터는 이동할 수 없고, 다른 캐릭터는 이동할 수 있다면, 다른 캐릭터만 이동
                else
                {
                    handler.ExecuteCommand(moveCommand);
                    moveCount++;
                }
            }
            //캐릭터가 한칸이라도 이동할 수 없고, 상태가 변경되지도 않는다면,
            else if(targetPositions.Count == 0 && !nowPlayer.stateChange)
            {
                return;
            }
            //캐릭터가 한칸이라도 이동할 수 있다면,
            else
            {
                handler.ExecuteCommand(moveCommand);
                moveCount++;
            }

            //moveCommand = new MoveCommand(nowPlayer, map, unirx_dir);

            //commands.add(moveCommand) and nowPlayer.Move(dir)
            
            if(moveCount < map.GetStepLimitsByIndex(0))
            {
                star = 3;
            }
            else if(moveCount < map.GetStepLimitsByIndex(1))
            {
                star = 2;
            }
            else if(moveCount < map.GetStepLimitsByIndex(2))
            {
                star = 1;
            }
            else
            {
                star = 0;
            }

            ui.SetMoveCountText(moveCount, map.GetStepLimitsByIndex(2));
            ui.revertButton.interactable = true;
        }

    }

    public void UndoCommand()
    {
        if (moveCount < map.GetStepLimitsByIndex(0))
        {
            star = 3;
        }
        else if (moveCount < map.GetStepLimitsByIndex(1))
        {
            star = 2;
        }
        else if (moveCount < map.GetStepLimitsByIndex(2))
        {
            star = 1;
        }
        else
        {
            star = 0;
        }
        ui.revertButton.interactable = false;
        ui.SetMoveCountText(moveCount, map.GetStepLimitsByIndex(2));
    }

   
    void GameSetting()
    {
        
        if (customMode)
        {
            map = mapLoader.CustomPlayMap();
        }
        else if (editorMode)
        {
            map = mapLoader.EditorStage();
        }
        else
            map = mapLoader.GenerateMap(gameManager.nowLevel);//map 생성



        //데이터 초기화 (Remain / Total / MoveCount)
        snow_total = RemainCheck();
        moveCount = 0;
        // player.FindPlayer 가 실행되면 자동으로 2개가 사라짐 이 전까지는 remain == total
        // 실행위치는 GameStart CameraController에 의해서 실행됨.


        //character 위치에 맵 데이터가 노멀블럭으로 되어있는데 캐릭터 데이터로 전환 ? **데이터가 캐릭터면 바꿀필요 없음
        //체크 true로 변경
        //snow_remain 변경?
        int AposX = (int)map.characterAPosition.x;
        int AposZ = (int)map.characterAPosition.z;

        int BposX = (int)map.characterBPosition.x;
        int BposZ = (int)map.characterBPosition.z;

        map.UpdateCheckArray(width: AposX, height: AposZ, true);//character position check true
        map.UpdateCheckArray(width: BposX, height: BposZ, true);//character position check true

        snow_remain = RemainCheck();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            Player player = players[i].GetComponent<Player>();
            player.enabled = true;

            if (player1 == null)
            {
                player1 = player;
                nowPlayer = player;
            }
            else
            {
                player2 = player;
                player1.other = player;
                player2.other = player1;
            }
        }

        //카메라 활성화
        cameraController.gameObject.SetActive(true);
        

        
    }


    public int RemainCheck()//남은 눈 체크 --> 이동
    {
        int remain = 0;

        for (int i = 0; i < map.mapSizeHeight; i++)
        {
            for (int j = 0; j < map.mapSizeWidth; j++)
            {
                if (!map.check[i, j])
                {
                    remain++;
                }
            }
        }

        snow_remain = remain;
        ui.SetRemainText(snow_remain, snow_total);

        if (remain == 0)
        {
            GameEnd(true);
        }
        else if (remain == 5)
        {
            ui.ActivateWarningUI();
        }
        else
        {
            if(moveCount == map.GetStepLimitsByIndex(2)) GameEnd(false);
        }
        

        return remain;
    }





    public void SetPlaying(bool play)
    {
        isPlaying = play;
    }

    public void GameStart()//called by cameracontroller.cs after mapscanning...
    {

        
        //if(!simulating)
        ui.SetRemainText(remain: snow_remain, total: snow_total);
        ui.SetSlider(map.GetStepLimits(), map.GetStepLimitsByIndex(2));
        ui.SetMoveCountText(moveCount, map.GetStepLimitsByIndex(2));
        
        nowPlayer = player1;
        nowPlayer.isActive = true;


        isPlaying = true;
        startTime = Time.time;

        if (map.parfait)
        {
            ui.mission_parfait.SetActive(true);
        }
        
        ui.inGame.SetActive(true);

        
        if (gameManager.stageDataOnPlay.GetIslandNumber() == 1)
        {
            StartCoroutine(TutorialManager.instance.StartTutorial(gameManager.stageDataOnPlay.GetStageNumber()));            
        }
        
        

        
        
            
    }

#region JSON
    /*
    public void UserUpdate(int cash, int stage)//cash : +cash , stage : nowStage
    {
        UserData user = new UserData(gameManager.user.id, cash, change_heart: 0, stage);
        var json = JsonUtility.ToJson(user);
        StartCoroutine(jsonAdapter.API_POST("account/update", json, callback => {

            gameManager.user.cash += cash;
            gameManager.user.stage = stage;

        }));

    }

    public void StageClear(int stage_num, int step)//max update
    {
        StageData stage = new StageData(gameManager.user.id, stage_num, step);

        var json = JsonUtility.ToJson(stage);
        StartCoroutine(jsonAdapter.API_POST("stage/insert", json, callback => { }));
    }

    public void StageClear(int step)//update step
    {
        StageData stage = new StageData(gameManager.user.id, gameManager.nowLevel, step);

        var json = JsonUtility.ToJson(stage);
        StartCoroutine(jsonAdapter.API_POST("stage/update", json, callback => { }));
    }
    */
#endregion

    void InterstitialLoadCallback(string id)
    {
        Debug.Log("Interstitial AD Callback");
        MoPub.RequestInterstitialAd(id);
        MoPub.ShowInterstitialAd(id);
    }

    public void GameEnd(bool success)
    {
        isSuccess = success;
        soundManager.Mute();

        isPlaying = false;
        endTime = Time.time;
        Debug.Log("Game End... PlayTime : " + (endTime - startTime));

        UserInfo userInfo = xmlManager.database.userInfo;
        UserHistory userHistory = xmlManager.database.userHistory;


        if(customMode)
        {

        }
        else if(editorMode)
        {

        }
        else
        {
            if (isSuccess)
            {
                int interstitialID = UnityEngine.Random.Range(0, Constants.FULL_IOS.Length);
               
                if (GameManager.instance.FullADCount % 5 ==0)
                {
#if UNITY_ANDROID
                    MoPub.RequestInterstitialAd(Constants.FULL_ANDROID[interstitialID]);
                    MoPub.ShowInterstitialAd(Constants.FULL_ANDROID[interstitialID]);
#elif UNITY_EDITOR
#else
                    
                    MoPub.RequestInterstitialAd(Constants.FULL_IOS[interstitialID]);
                    MoPub.ShowInterstitialAd(Constants.FULL_IOS[interstitialID]);
#endif

                }
                GameManager.instance.FullADCount++;


                userHistory.stage_clear++;

                if (!gameManager.stageDataOnPlay.GetIsClear())//최초 클리어
                {
                    if (gameManager.stageDataOnPlay.GetIslandNumber() == 1 && gameManager.stageDataOnPlay.GetStageNumber() == 6)
                    {
                        QuestManager.questDelegate(6, QuestState.Clear);//튜토리얼 섬 클리어
                    }
                    else if (gameManager.stageDataOnPlay.GetIslandNumber() == 2 && gameManager.stageDataOnPlay.GetStageNumber() == 30)
                    {
                        QuestManager.questDelegate(7, QuestState.Clear);//6 클리어
                    }
                    else if (gameManager.stageDataOnPlay.GetIslandNumber() == 3 && gameManager.stageDataOnPlay.GetStageNumber() == 30)
                    {
                        QuestManager.questDelegate(8, QuestState.Clear);//6 클리어
                    }
                    else if (gameManager.stageDataOnPlay.GetIslandNumber() == 4 && gameManager.stageDataOnPlay.GetStageNumber() == 30)
                    {
                        QuestManager.questDelegate(9, QuestState.Clear);//6 클리어
                    }
                    else if (gameManager.stageDataOnPlay.GetIslandNumber() == 5 && gameManager.stageDataOnPlay.GetStageNumber() == 30)
                    {
                        QuestManager.questDelegate(10, QuestState.Clear);//6 클리어

                    }

                    if (gameManager.stageDataOnPlay.GetStageNumber() % 5 == 0 && gameManager.stageDataOnPlay.GetIslandNumber() != 1)
                    {
                        int devide = (gameManager.stageDataOnPlay.GetStageNumber() / 5)- 1;
                        if(devide == 0)
                        {
                            xmlManager.database.userRooms[0].isDirty = false;
                            xmlManager.SaveItems();
                        }
                       
                    }

                    UserStage newStageClear = new UserStage(userInfo.nickname,gameManager.stageDataOnPlay.GetIslandNumber(), gameManager.stageDataOnPlay.GetStageNumber(), gameManager.stageDataOnPlay.GetStageName(), star, moveCount);
                    xmlManager.database.userStage.Add(newStageClear);

                    userInfo.boong += 200 + gameManager.stageDataOnPlay.GetIslandNumber() * 50;
                    userHistory.boong_get = 200 + gameManager.stageDataOnPlay.GetIslandNumber() * 50;

                    ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);
                }
                else//이미 클리어 된 스테이지
                {
                    UserStage clearStage = xmlManager.database.userStage.Find(x => x.stage_name == gameManager.stageDataOnPlay.GetStageName());
                    clearStage.UpdateClearStage(star, moveCount);
                    ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);

                }


            }
            else//fail stage
            {
                userHistory.stage_fail++;
                ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);
            }
        }

        xmlManager.database.SyncWithCSV();
    }
    /*
    public void GameEnd(bool success)
    {
        isSuccess = success;
        soundManager.Mute();

        //copyHistory = awsManager.userHistory.DeepCopy();
        //copyInfo = awsManager.userInfo.DeepCopy();
        copyHistory = xmlManager.database.userHistory;
        copyInfo = xmlManager.database.userInfo;

        isPlaying = false;
        endTime = Time.time;
        Debug.Log("Game End... PlayTime : " + (endTime - startTime));

        
       
        
        if (customMode)
        {
            
            copyHistory.editor_clear++;
            
            if (gameManager.playEditorStage.move > moveCount)
            {
                gameManager.playEditorStage.move = moveCount;
            }
            //이미 클리어한 맵이라면 업데이트, 아니면 인서트
            UserStage newStageClear = new UserStage(copyInfo.nickname, gameManager.playEditorStage.map_id, star, moveCount);
            EditorClearRequest editorClearRequest = new EditorClearRequest(copyInfo, copyHistory, newStageClear, gameManager.playEditorStage);

            if (!awsManager.userStage.ContainsKey(gameManager.playEditorStage.map_id))
            {
                
                //최초 클리어 시
                //붕 별사탕 보상 획득
                copyInfo.boong += gameManager.playEditorStage.level * 100;
                copyInfo.candy += gameManager.playEditorStage.level;
                copyHistory.boong_get += gameManager.playEditorStage.level * 100;

                //플레이 카운트 +1
                gameManager.playEditorStage.play_count++;
                

                var request= jsonAdapter.POST_DATA(editorClearRequest,"newEditorStage/update", (isConnect)=> {
                    if(isConnect)
                    {
                        CustomMapItem map = awsManager.editorMap.Find(x => x.itemdata.map_id == GameManager.instance.playEditorStage.map_id);
                        map.itemdata = GameManager.instance.playEditorStage;
                        map.play.text = map.itemdata.play_count.ToString();

                        ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);
                        gameManager.retry = true;
                    }
                    else
                    {

                    }
                });

                jsonAdapter.ReadyRequest(request);
            }
            else
            {
                //이미 클리어
                var request = jsonAdapter.POST_DATA(editorClearRequest,"updateEditorStage/update", (isConnect) => {
                    if (isConnect)
                    {
                        CustomMapItem map = awsManager.editorMap.Find(x => x.itemdata.map_id == GameManager.instance.playEditorStage.map_id);
                        map.itemdata = GameManager.instance.playEditorStage;
                        map.play.text = map.itemdata.play_count.ToString();

                        ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);
                        gameManager.retry = true;
                    }
                    else
                    {

                    }
                });

                jsonAdapter.ReadyRequest(request);


            }
            
           
            

        }
        else if (editorMode)//mapLoader.editorMap 생성하기
        {
            awsManager.userHistory.editor_make++;
            ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);
        }
        else//stage mode
        {
            if (isSuccess)
            {
                copyHistory.stage_clear++;
                
                
                if (!gameManager.stageDataOnPlay.GetIsClear())//최초 클리어
                {
                    if (gameManager.islandDataOnPlay.GetIslandNumber() == 1 && gameManager.stageDataOnPlay.GetStageNumber() == 6)
                    {
                        QuestManager.questDelegate(3, 2);//3 시작
                        QuestManager.questDelegate(6, 4);//6 클리어
                        QuestManager.questDelegate(7, 2);//7시작
                    }
                    else if(gameManager.islandDataOnPlay.GetIslandNumber() == 2 && gameManager.stageDataOnPlay.GetStageNumber() == 4)
                    {
                        QuestManager.questDelegate(7, 4);//6 클리어
                        QuestManager.questDelegate(8, 2);//7시작
                    }
                    else if (gameManager.islandDataOnPlay.GetIslandNumber() == 3 && gameManager.stageDataOnPlay.GetStageNumber() == 4)
                    {
                        QuestManager.questDelegate(8, 4);//6 클리어
                        QuestManager.questDelegate(9, 2);//7시작
                    }
                    else if (gameManager.islandDataOnPlay.GetIslandNumber() == 4 && gameManager.stageDataOnPlay.GetStageNumber() == 4)
                    {
                        QuestManager.questDelegate(9, 4);//6 클리어
                        QuestManager.questDelegate(10, 2);//7시작
                    }
                    else if (gameManager.islandDataOnPlay.GetIslandNumber() == 5 && gameManager.stageDataOnPlay.GetStageNumber() == 4)
                    {
                        QuestManager.questDelegate(10, 4);//6 클리어
                       
                    }

                    Debug.Log("high level");
                    UserStage newStageClear = new UserStage(copyInfo.nickname, gameManager.playStageName, star, moveCount);
                    copyInfo.boong += 200 + gameManager.islandDataOnPlay.GetIslandNumber() * 50;
                    copyHistory.boong_get = 200 + gameManager.islandDataOnPlay.GetIslandNumber() * 50;
                    StageClearRequest stageClearRequest = new StageClearRequest(copyInfo, copyHistory, newStageClear);
                    var request = jsonAdapter.POST_DATA(stageClearRequest,"newStage/update", (isConnect)=> {
                        if(isConnect)
                        {
                            ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);
                        }
                        else
                        {

                        }
                    });

                    jsonAdapter.ReadyRequest(request);
                }
                else//이미 클리어 된 스테이지
                {

                    UserStage clearStage = awsManager.userStage[gameManager.playStageName];
                    clearStage.UpdateClearStage(star, moveCount);
                    if (clearStage != null)
                    {
                        StageClearRequest stageClearRequest = new StageClearRequest(copyInfo, copyHistory, clearStage);
                        var request = jsonAdapter.POST_DATA(stageClearRequest, "updateStage/update", (isConnect) => {
                            if (isConnect)
                            {
                                ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);
                            }
                            else
                            {

                            }
                        });

                        jsonAdapter.ReadyRequest(request);

                    }

                }

                
            }
            else//fail stage
            {
                copyHistory.stage_fail++;
                ui.GameEnd(isSuccess, star, snow_remain, moveCount, customMode, editorMode);
            }

            


            
        }


    }
    
   */

    private void OnDestroy()
    {
        MoPubManager.OnInterstitialLoadedEvent -= InterstitialLoadCallback;
    }

}
