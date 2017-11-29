using UnityEngine;
using System.Collections;

public class BattleMgr : MonoBehaviour {

    public static BattleMgr Instance;

    public BombMgr bombMgr;

    public GameObject mainHero;

    public PlayerController selfPlayerController;
    // Use this for initialization
    void Awake ()
    {
        Instance = this;
        bombMgr = new BombMgr();
        InitHero();
    }
    
    /// <summary>
    /// 初始化主英雄
    /// </summary>
    void InitHero()
    {
        mainHero = Instantiate(Resources.Load("Prefabs/Heros/" + GlobleHeroData.heroModelName) as GameObject);
        selfPlayerController = mainHero.GetComponent<PlayerController>();
        selfPlayerController.heroData.Username = GlobleHeroData.username;
        selfPlayerController.heroData.NickName = GlobleHeroData.nickName;
        selfPlayerController.heroData.Hp = 100;

        mainHero.transform.position = MapMgr.Instance.emptyItemList[Random.Range(0, MapMgr.Instance.emptyItemList.Count)];
    }
}
