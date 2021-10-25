using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class MyIgloo : UIScript
{
    public SkinInfoPopup infoPopup;
    public RectTransform useA;
    public RectTransform useB;
    [Header("SKIN")]
    public Dictionary<int,GameObject> skinA = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> skinB = new Dictionary<int, GameObject>();
    public Transform a_position;
    public Transform b_position;
    //public Image skinA;
    //public Image skinB;
    public MyIglooSkinItem myIglooSkinItemPrefab;
    public List<MyIglooSkinItem> myIglooSkinItems;

    public Transform skinContent;
    public int character = 0;

    public Dropdown sortingDropdown;
    public Text skinPowderText;
    public GameObject[] lightImage;
    //[Header("STYLE")]
    //[Header("BAG")]
    // Start is called before the first frame update
    private void Start()
    {
        using (var e = CSVManager.skinData.GetInfoEnumerator())
        {
            int i = 0;
            while (e.MoveNext())
            {
                skinA.Add(e.Current.Key, a_position.GetChild(i).gameObject);
                skinB.Add(e.Current.Key, b_position.GetChild(i).gameObject);

                var data = e.Current.Value;
                MyIglooSkinItem item = Instantiate(myIglooSkinItemPrefab);
                item.InitializeItem(data, this);
                myIglooSkinItems.Add(item);
                item.transform.SetParent(skinContent, false);
                i++;
            }

        }

        
        /*
        var changeSkinAStream = awsManager.userInfo.ObserveEveryValueChanged(x => x.skin_a)
            .Subscribe(skin_num => ChangeSkin(skin_num,awsManager.userInfo.skin_b)); 

        var changeSkinBStream = awsManager.userInfo.ObserveEveryValueChanged(x => x.skin_b)
            .Subscribe(skin_num => ChangeSkin(awsManager.userInfo.skin_a,skin_num));
        */

        var changeSkinAStream = xmlManager.database.userInfo.ObserveEveryValueChanged(x => x.skin_a)
          .Subscribe(skin_num => ChangeSkin(skin_num, xmlManager.database.userInfo.skin_b));

        var changeSkinBStream = xmlManager.database.userInfo.ObserveEveryValueChanged(x => x.skin_b)
            .Subscribe(skin_num => ChangeSkin(xmlManager.database.userInfo.skin_a, skin_num));
    }

    
    void ChangeSkin(int a_num , int b_num)
    {
        QuestManager.questDelegate(4, Data.QuestState.Clear);
        foreach(var a in skinA.Values)
        {
            a.SetActive(false);
        }
        foreach (var b in skinB.Values)
        {
            b.SetActive(false);
        }
        skinA[a_num].SetActive(true);//.sprite = Resources.Load<Sprite>("store/skin/" + csvManager.skins[a_num].path);
        skinB[b_num].SetActive(true);//.sprite = Resources.Load<Sprite>("store/skin/" + csvManager.skins[b_num].path);

        for (int i = 0; i < myIglooSkinItems.Count; i++)
        {
            if (myIglooSkinItems[i].skin.key == a_num)
            {

                myIglooSkinItems[i].inUse = true;
                useA.SetParent(myIglooSkinItems[i].transform,false);
                useA.localPosition = default;
                
            }
            else if(myIglooSkinItems[i].skin.key == b_num)
            {
                myIglooSkinItems[i].inUse = true;
                useB.SetParent(myIglooSkinItems[i].transform, false);
                useB.localPosition = default;
            }
            else
            {
                myIglooSkinItems[i].inUse = false;
            }
        }
    }


    public void GoLobby()
    {
        gameObject.SetActive(false);
    }

    public void SelectCharacter(int character_num)
    {
        character = character_num;


        if (character_num == 0)
        {
            lightImage[1].SetActive(false);
            lightImage[0].SetActive(true);
        }
            
        else
        {
            lightImage[0].SetActive(false);
            lightImage[1].SetActive(true);
        }
            
    }

    public void Filtering(bool all)
    {
        if(all)
        {
            foreach (MyIglooSkinItem item in myIglooSkinItems)
            {
                item.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (MyIglooSkinItem item in myIglooSkinItems)
            {
                item.gameObject.SetActive(item.skin.inPossession);
                
            }
        }
    }

    public void Sorting()
    {
        int dropDown = sortingDropdown.value;
        if (dropDown == 0)//최근 획득 순
        {
            myIglooSkinItems = myIglooSkinItems.OrderBy(x => x.skin.skin_get_time).ToList();
        }
        else if(dropDown == 1)//등급 순
        {
            myIglooSkinItems = myIglooSkinItems.OrderBy(x => x.skin.key).ToList();
        }

        foreach (MyIglooSkinItem item in myIglooSkinItems)
        {
            item.transform.SetAsFirstSibling();
        }
    }

    public void MoveUseButton()
    {
        foreach (MyIglooSkinItem item in myIglooSkinItems)
        {
            item.useButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //skinPowderText.text = awsManager.userInfo.skin_powder.ToString();
        
    }

}
