using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroItemUI : MonoBehaviour {

    public string username;

    Text nameText;
    Text hpText;
    Text killCountText;
    Text rankText;

    bool isInit = false;
	// Use this for initialization
	void Awake ()
    {
        Init();
    }

    private void OnDestroy()
    {
        if (BattleSyncMgr.Instance.playerDic.ContainsKey(username))
        {
            BattleSyncMgr.Instance.playerDic[username].heroData.onHpAction = null;
            BattleSyncMgr.Instance.playerDic[username].heroData.onKillCountAction = null;
        }
    }

    public void SetData(string username, string nickName, int hp, int killCount, int rank)
    {
        Init();
        
        this.username = username;
        nameText.text = nickName;
        hpText.text = hp.ToString();
        killCountText.text = killCount.ToString();
        rankText.text = rank.ToString();

        if(username == GlobleHeroData.username)
        {
            nameText.color = Color.green;
            hpText.color = Color.green;
            killCountText.color = Color.green;
            rankText.color = Color.green;
        }
        else
        {
            nameText.color = Color.white;
            hpText.color = Color.white;
            killCountText.color = Color.white;
            rankText.color = Color.white;
        }

        BattleSyncMgr.Instance.playerDic[username].heroData.onHpAction = OnListenerHp;
        BattleSyncMgr.Instance.playerDic[username].heroData.onKillCountAction = OnListenerKillCount;
    }

    void Init()
    {
        if (isInit) return;
        isInit = true;
        nameText = transform.Find("NameText").GetComponent<Text>();
        hpText = transform.Find("HpText").GetComponent<Text>();
        killCountText = transform.Find("KillCountText").GetComponent<Text>();
        rankText = transform.Find("RankText").GetComponent<Text>();
    }

    void OnListenerHp(string username, int hp)
    {
        hpText.text = hp.ToString();
    }

    void OnListenerKillCount(string username, int killCount)
    {
        killCountText.text = killCount.ToString();
        BattleUI.Instance.SetHeroItemData();
    }
}
