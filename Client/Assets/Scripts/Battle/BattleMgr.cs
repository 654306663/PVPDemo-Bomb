using UnityEngine;
using System.Collections;

public class BattleMgr : MonoBehaviour {

    public static BattleMgr Instance;

    public BombMgr bombMgr;

    public GameObject mainHero;
	// Use this for initialization
	void Awake ()
    {
        Instance = this;
        bombMgr = new BombMgr();
        InitHero();
    }
    
    void InitHero()
    {
        mainHero = Instantiate(Resources.Load("Prefabs/Heros/" + GlobleHeroData.heroModelName) as GameObject);
        PlayerController playerController = mainHero.GetComponent<PlayerController>();
        playerController.heroData.Username = GlobleHeroData.username;
        playerController.heroData.NickName = GlobleHeroData.nickName;
        playerController.heroData.Hp = 100;

        mainHero.transform.position = MapMgr.Instance.emptyItemList[Random.Range(0, MapMgr.Instance.emptyItemList.Count)];
    }
}
