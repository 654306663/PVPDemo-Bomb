using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class BattleUI : MonoBehaviour {

    public static BattleUI Instance;

	private Button backButton;

    private Transform heroItemParent;

    private List<HeroItemUI> heroItemList;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
		backButton = transform.Find("BackButton").GetComponent<Button>();
        heroItemParent = transform.Find("HeroScrollView/Grid").transform;
        backButton.onClick.AddListener(OnBackEvent);

        heroItemList = new List<HeroItemUI>();

        AddHeroItem();

        MessageMediator.AddListener<string>(MessageMediatType.AddPlayer, AddOneHeroItem);
        MessageMediator.AddListener<string>(MessageMediatType.RemovePlayer, RemoveOnHeroItem);
    }

    private void OnDestroy()
    {
        MessageMediator.RemoveListener<string>(MessageMediatType.AddPlayer, AddOneHeroItem);
        MessageMediator.RemoveListener<string>(MessageMediatType.RemovePlayer, RemoveOnHeroItem);
    }

    void OnBackEvent()
    {
        SyncPlayerRequest.Instance.SendSyncRemovePlayerRequest();
        SceneMgr.LoadScene(SceneType.ChooseScene);
    }

    void AddHeroItem()
    {
        SortHeroItem();
        foreach (var item in BattleSyncMgr.Instance.playerDic)
        {
            AddOneHeroItem(item.Key);
        }
    }

    int initRankIndex = 1;
    void AddOneHeroItem(string username)
    {
        if (!BattleSyncMgr.Instance.playerDic.ContainsKey(username)) return;

        if (heroItemList.Count < 10)
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/UI/HeroItem")) as GameObject;
            go.transform.SetParent(heroItemParent);
            HeroItemUI heroItemUI = go.GetComponent<HeroItemUI>();
            heroItemList.Add(heroItemUI);
        }

        SetHeroItemData();
    }

    void RemoveOnHeroItem(string username)
    {
        if(heroItemList.Count > BattleSyncMgr.Instance.playerDic.Count)
        {
            Destroy(heroItemList[0].gameObject);
            heroItemList.RemoveAt(0);
        }
        SetHeroItemData();
    }

    void SortHeroItem()
    {
        BattleSyncMgr.Instance.playerDic = BattleSyncMgr.Instance.playerDic.OrderByDescending(i => i.Value.heroData.KillCount).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void SetHeroItemData()
    {
        SortHeroItem();
        int index = 0;
        int initRankIndex = 1;
        foreach (var item in BattleSyncMgr.Instance.playerDic)
        {
            if (heroItemList.Count > index)
            {
                heroItemList[index++].SetData(item.Key, item.Value.heroData.NickName, item.Value.heroData.Hp
            , item.Value.heroData.KillCount, initRankIndex++);
            }
        }
    }

    public void ShowFlutterText(string deadUsername, string killerUsername)
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/UI/FlutterText")) as GameObject;
        go.transform.SetParent(GameObject.Find("Canvas").transform);
        if (deadUsername == killerUsername)
        {
            go.GetComponent<Text>().text = "<color=#ff0000>" + deadUsername + "</color>" + "<color=#ffffff>自杀了</color>";
        }
        else
        {
            go.GetComponent<Text>().text = "<color=#00ff00>" + killerUsername + "</color><color=#ffffff>杀死了</color><color=#ff0000>" + deadUsername + "</color>";
        }
    }
}
