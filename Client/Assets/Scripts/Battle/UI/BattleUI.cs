using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour {

	private Button backButton;

    private Transform heroItemParent;

    private Dictionary<string, GameObject> heroItemDict;

	// Use this for initialization
	void Start () {
		backButton = transform.Find("BackButton").GetComponent<Button>();
        heroItemParent = transform.Find("HeroScrollView/Grid").transform;
        backButton.onClick.AddListener(OnBackEvent);

        heroItemDict = new Dictionary<string, GameObject>();

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
        foreach (var item in BattleSyncMgr.Instance.playerDic)
        {
            AddOneHeroItem(item.Key);
        }
    }

    void AddOneHeroItem(string username)
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/UI/HeroItem")) as GameObject;
        go.transform.SetParent(heroItemParent);
        heroItemDict.Add(username, go);

        if (BattleSyncMgr.Instance.playerDic.ContainsKey(username))
        {
            BattleSyncMgr.Instance.playerDic[username].heroData.onHpAction += OnListenerHp;

            go.transform.Find("NameLabel").GetComponent<Text>().text = BattleSyncMgr.Instance.playerDic[username].heroData.NickName;
            go.transform.Find("HpLabel").GetComponent<Text>().text = BattleSyncMgr.Instance.playerDic[username].heroData.Hp.ToString();
        }
    }

    void RemoveOnHeroItem(string username)
    {
        if (heroItemDict.ContainsKey(username))
        {
            if (BattleSyncMgr.Instance.playerDic.ContainsKey(username))
            {
                BattleSyncMgr.Instance.playerDic[username].heroData.onHpAction -= OnListenerHp;
            }
            Destroy(heroItemDict[username]);
            heroItemDict.Remove(username);
        }
    }

    void OnListenerHp(string username, int hp)
    {
        if(heroItemDict.ContainsKey(username))
        {
            heroItemDict[username].transform.Find("HpLabel").GetComponent<Text>().text = hp.ToString();
        }
    }
}
