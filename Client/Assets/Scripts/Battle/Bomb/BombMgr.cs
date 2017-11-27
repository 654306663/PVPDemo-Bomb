using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombMgr{

    private Dictionary<int, Bomb> activeBombDict = new Dictionary<int, Bomb>();
    private Stack<Bomb> unactiveBombStack = new Stack<Bomb>();

    public void AddBomb(BombData data)
    {
        if(unactiveBombStack.Count > 0)
        {
            Bomb bomb = unactiveBombStack.Pop();
            bomb.transform.position = data.startPos;
            bomb.Show(data);
            activeBombDict.Add(data.id, bomb);
            bomb.gameObject.SetActive(true);
        }
        else
        {
            Bomb bomb = GameObject.Instantiate(Resources.Load("Prefabs/Bomb") as GameObject).GetComponent<Bomb>();
            bomb.transform.SetParent(GetBombParent(), true);
            bomb.transform.position = data.startPos;
            bomb.Show(data);
            activeBombDict.Add(data.id, bomb);
            bomb.gameObject.SetActive(true);
        }
    }

    public void OpenBomb(ProtoData.OpemBombS2CEvt openBombS2CEvt)
    {
        if (activeBombDict.ContainsKey(openBombS2CEvt.bombId) && activeBombDict[openBombS2CEvt.bombId] != null)
        {
            activeBombDict[openBombS2CEvt.bombId].Open();
        }
        CalculateDamage(openBombS2CEvt.beHitData);
    }

    void CalculateDamage(List<ProtoData.OpemBombS2CEvt.BeHitData> beHitDataList)
    {
        for (int i = 0; i < beHitDataList.Count; i++)
        {
            if(BattleSyncMgr.Instance.playerDic.ContainsKey(beHitDataList[i].username))
            {
                BattleSyncMgr.Instance.playerDic[beHitDataList[i].username].heroData.Hp -= beHitDataList[i].lossHp;
            }
        }
    }

    public void RemoveBomb(int id)
    {
        if(activeBombDict.ContainsKey(id) && activeBombDict[id] != null)
        {
            activeBombDict[id].gameObject.SetActive(false);
            activeBombDict[id].Reset();

            if(unactiveBombStack.Count < 10)
            {
                unactiveBombStack.Push(activeBombDict[id]);
            }
            activeBombDict.Remove(id);
        }
    }

    Transform bombParent = null;
    Transform GetBombParent()
    {
        if (bombParent == null)
        {
            bombParent = new GameObject("BombParent").transform;
            bombParent.position = Vector3.zero;
            bombParent.eulerAngles = Vector3.zero;
            bombParent.localScale = Vector3.one;
        }
        return bombParent;
    }
}
