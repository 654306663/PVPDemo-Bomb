using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tools;
using ProtoData;
using System.Linq;

public class BattleSyncMgr : MonoBehaviour
{

    public static BattleSyncMgr Instance;

    //存储所有实例化出来的Player
    public Dictionary<string, PlayerController> playerDic = new Dictionary<string, PlayerController>();

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        playerDic.Add(GlobleHeroData.username, BattleMgr.Instance.mainHero.GetComponent<PlayerController>());

        SyncPlayerRequest.Instance.SendSyncAddPlayerRequest();

        StartCoroutine(SyncPosition());

        //StartCoroutine(SyncTransformRemedyTimer());
    }

    //位置信息时时更新
    private Vector3 lastPosition = Vector3.zero;
    IEnumerator SyncPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.03f);

            //如果玩家的位置当前玩家的位置和上玩家上一个的位置距离大于0.1，就表示玩家移动了，就需要他位置的同步
            if (BattleMgr.Instance.mainHero.transform.position != lastPosition)
            {
                lastPosition = BattleMgr.Instance.mainHero.transform.position;
                Net.SyncTransformRequest.Instance.SendSyncTransformRequest(BattleMgr.Instance.mainHero.transform.position, BattleMgr.Instance.mainHero.transform.localEulerAngles.y);//调用位置信息同步的请求
            }
        }

    }

    //实例化其他客户端的角色
    public void OnSyncPlayerResponse(List<AddPlayerS2C.PlayerData> dataList)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            OnAddPlayerEvent(dataList[i].username, dataList[i].modelName, dataList[i].nickName, dataList[i].hp, dataList[i].killCount);
        }
    }

    public void OnAddPlayerEvent(string username, string heroModelName, string nickName, int hp, int killCount)
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Heros/" + heroModelName) as GameObject);
        go.GetComponent<FSMController>().system.playerType = PlayerType.Other;
        PlayerController playerController = go.GetComponent<PlayerController>();
        playerController.heroData.Username = username;
        playerController.heroData.NickName = nickName;
        playerController.heroData.Hp = hp;
        playerController.heroData.KillCount = killCount;

        playerDic.Add(username, playerController);//利用集合保存所有的其他客户端

        if(playerController.heroData.Hp <= 0)
        {
            CoroutineUtil.Instance.WaitTime(0, false, () =>{
                playerDic[username].fsmController.system.PerformTransition(FSMTransition.IdleToDead);
            });
        }

        MessageMediator.Dispatch(MessageMediatType.AddPlayer, username);
    }

    public void OnRemovePlayerEvent(string username)
    {
        if (playerDic.ContainsKey(username) && playerDic[username] != null)
        {
            Destroy(playerDic[username].gameObject);
            playerDic.Remove(username);
        }

        MessageMediator.Dispatch(MessageMediatType.RemovePlayer, username);
    }

    public void OnPlayerDeadEvent(string deadUsername, string killerUsername)
    {
        if (playerDic.ContainsKey(deadUsername))
        {
            FSMTransition transition = FSMTransition.Empty;
            switch (playerDic[deadUsername].fsmController.system.CurrentState.StateId)
            {
                case FSMStateId.Idle:
                    transition = FSMTransition.IdleToDead;
                    break;
                case FSMStateId.Run:
                    transition = FSMTransition.RunToDead;
                    break;
                case FSMStateId.Throw:
                    transition = FSMTransition.ThrowToDead;
                    break;
            }
            playerDic[deadUsername].fsmController.system.PerformTransition(transition);
        }
        if (playerDic.ContainsKey(killerUsername) && deadUsername != killerUsername)
        {
            playerDic[killerUsername].heroData.KillCount++;
        }
    }

    bool isLastRecorded = false;
    public void OnSyncTransformEvent(List<ProtoData.SyncTransformEvtS2C.TransformData> transformDataList)
    {
        foreach (ProtoData.SyncTransformEvtS2C.TransformData pd in transformDataList)//遍历所有的数据
        {
            PlayerController playerController = DictTool.GetValue<string, PlayerController>(playerDic, pd.username);//根据传递过来的Username去找到所对应的实例化出来的Player

            //如果查找到了相应的角色，就把相应的位置信息赋值给这个角色的position
            if (playerController != null && playerController.gameObject != BattleMgr.Instance.mainHero)
            {
                playerController.transform.position = new Vector3(pd.x, pd.y, pd.z);
                playerController.transform.localEulerAngles = new Vector3(0, pd.angleY, 0);
            }
        }

        //syncTransformTime = Time.time;

        //RecordTransfromData(transformDataList);
    }

    public void OnSyncTransitionEvent(string username, FSMTransition targetTransition, params object[] objs)
    {
        if (!playerDic.ContainsKey(username)) return;

        playerDic[username].fsmController.system.PerformTransition(targetTransition, objs);
    }

    /// <summary>
    /// 记录前两帧数据
    /// </summary>
    /// <param name="transformDataList"></param>
    Dictionary<float, List<ProtoData.SyncTransformEvtS2C.TransformData>> transformDatasDict = new Dictionary<float, List<SyncTransformEvtS2C.TransformData>>();
    void RecordTransfromData(List<ProtoData.SyncTransformEvtS2C.TransformData> transformDataList, bool isFromNet = false)
    {
        bool isReplace = false;
        if (isFromNet)
        {
            foreach (KeyValuePair<float, List<ProtoData.SyncTransformEvtS2C.TransformData>> item in transformDatasDict)
            {
                if (Time.time - item.Key < 0.02f)
                {
                    transformDatasDict[item.Key] = transformDataList;
                    isReplace = true;
                }
                break;
            }

        }
        if (!isReplace)
        {
            if (!transformDatasDict.ContainsKey(Time.time))
            {
                transformDatasDict.Add(Time.time, transformDataList);
            }
            else
            {
                transformDatasDict[Time.time] = transformDataList;
            }
        }
        if (transformDatasDict.Count > 5)
            transformDatasDict.Remove(transformDatasDict.First().Key);
    }

    /// <summary>
    /// 获取最近一次记录的时间
    /// </summary>
    /// <returns></returns>
    float GetLastRecordTime()
    {
        if (transformDatasDict.Count > 0)
        {
            return transformDatasDict.Last().Key;
        }
        return 0;
    }

    /// <summary>
    /// 计算补偿
    /// </summary>
    /// <returns></returns>
    List<ProtoData.SyncTransformEvtS2C.TransformData> CalculateSyncTransformRemedy()
    {
        if (transformDatasDict.Count > 1)
        {
            if(transformDatasDict.ElementAt(transformDatasDict.Count - 1).Key - transformDatasDict.ElementAt(transformDatasDict.Count - 2).Key > 0.05f)
            {
                return new List<SyncTransformEvtS2C.TransformData>();
            }
            List<ProtoData.SyncTransformEvtS2C.TransformData> dataList1 = transformDatasDict.ElementAt(transformDatasDict.Count - 2).Value;
            List<ProtoData.SyncTransformEvtS2C.TransformData> dataList2 = transformDatasDict.ElementAt(transformDatasDict.Count - 1).Value;
            List<ProtoData.SyncTransformEvtS2C.TransformData> newDataList = new List<SyncTransformEvtS2C.TransformData>();
            for (int i = 0; i < dataList2.Count; i++)
            {
                for (int j = 0; j < dataList1.Count; j++)
                {
                    if (dataList2[i].username == dataList1[j].username)
                    {
                        ProtoData.SyncTransformEvtS2C.TransformData newData = new SyncTransformEvtS2C.TransformData();
                        newData.username = dataList2[i].username;
                        Vector3 vec1 = new Vector3(dataList1[j].x, dataList1[j].y, dataList1[j].z);
                        Vector3 vec2 = new Vector3(dataList2[i].x, dataList2[i].y, dataList2[i].z);
                        float distance = Vector3.Distance(vec1, vec2);
                        Vector3 newVec = vec2 + (vec2 - vec1).normalized * (distance * 2);
                        newData.x = newVec.x;
                        newData.y = newVec.y;
                        newData.z = newVec.z;
                        newDataList.Add(newData);
                    }
                }
            }
            RecordTransfromData(newDataList);
            return newDataList;
        }
        else return new List<SyncTransformEvtS2C.TransformData>();
    }

    /// <summary>
    /// 同步补偿计时
    /// </summary>
    float lastSyncTransformTime = 0;
    float syncTransformTime = 0;
    IEnumerator SyncTransformRemedyTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.03f);

            if (syncTransformTime != 0 && Time.time - syncTransformTime > 0.03f)
            {
                SyncTransformRemedy();
            }
        }
    }

    /// <summary>
    /// 执行补偿
    /// </summary>
    void SyncTransformRemedy()
    {
        Debug.Log("执行位移补偿");
        //OnSyncTransformEvent(CalculateSyncTransformRemedy());
    }
}
