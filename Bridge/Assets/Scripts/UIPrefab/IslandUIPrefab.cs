using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

/*@IslandUIPrefab
 * brief : 메인 로비의 섬 유아이 프리팹이다
 */
public class IslandUIPrefab : UIPrefab
{
    [Header("UI")]
    private Image islandBackgroundImage;
    private Image islandBackgroundLowerImage;
    private Button islandButton;//섬 이미지
    private Image islandTextImage;//섬 텍스트 이미지
    private Image goldMedal;//섬 클리어 시 골드 메달
    private Text islandStarText;//별 보유 갯수
    private Button islandStarButton;


    private void Awake()
    {
        islandBackgroundImage = transform.GetChild(0).GetComponent<Image>();
        islandBackgroundLowerImage = transform.GetChild(1).GetComponent<Image>();
        islandButton = transform.GetChild(2).GetComponent<Button>();
        islandTextImage = transform.GetChild(3).GetComponent<Image>();
        goldMedal = transform.GetChild(4).GetComponent<Image>();

        islandStarButton = transform.GetChild(5).GetComponent<Button>();
        islandStarText = islandStarButton.transform.GetChild(0).GetComponent<Text>();

        goldMedal.gameObject.SetActive(false);
        islandStarButton.onClick.AddListener(()=>ActiveStarPopup());
        islandButton.onClick.AddListener(() => ActiveStageSelectTab());

        Sprite islandTextBackgroundSprite = Resources.Load<Sprite>("MainScene/Other/islandTextBackgroundImage");
        if(islandStarButton != null)
        {
            islandStarButton.GetComponent<Image>().sprite = islandTextBackgroundSprite;
        }
        else
        {
            Debug.LogWarning("Image is not exist");
        }

        Sprite goldMedalImage = Resources.Load<Sprite>("MainScene/Other/goldMedal");
        if(goldMedalImage != null)
        {
            goldMedal.sprite = goldMedalImage;
        }
        else
        {
            Debug.LogWarning("Image is not exist");
        }

    }
    public override void SetUIPrefab(BaseData.BaseDataInfo data)
    {
        base.SetUIPrefab(data);
        IslandData.Info islandData = (IslandData.Info)data;
        string path = islandData.GetIslandName();
        string isClear = islandData.IsIslandClear() ? "_clear" : "_noClear";
        SetIslandImage(path,isClear);
        SetIslandStarText(islandData.GetMyStar(),islandData.GetMaxStar());

        bool activeMedal = islandData.GetMyStar() == islandData.GetMaxStar() ? true : false;
        ActiveGoldMedal(activeMedal);


    }
    public void SetIslandImage(string path,string clear)
    {
        islandBackgroundImage.sprite = Resources.Load<Sprite>("MainScene/IslandBackground/" + path);
        islandBackgroundLowerImage.sprite = Resources.Load<Sprite>("MainScene/IslandBackground/" + path+"_lower");
        islandButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("MainScene/IslandImage/"+ path+clear);
        islandTextImage.sprite = Resources.Load<Sprite>("MainScene/IslandText/" + path+"_text");
    }
    public void SetIslandStarText(int userStar , int maxStar)
    {
        islandStarText.text = userStar + "/" + maxStar;
    }
    public void ActiveGoldMedal(bool active)
    {
        goldMedal.gameObject.SetActive(active);
    }

    void ActiveStarPopup()
    {
        IslandData.Info islandData = (IslandData.Info)data;
        GameObject.Find("Main Canvas").GetComponent<MainSceneUIScript>().ActiveStarGuideBookPopup(islandData.GetIslandNumber());

    }

    public void ActiveStageSelectTab()
    {
        
        IslandData.Info islandData = (IslandData.Info)data;
        GameObject.Find("Main Canvas").GetComponent<MainSceneUIScript>().ActiveStarGuideBookPopup(islandData.GetIslandNumber());
    }
}
