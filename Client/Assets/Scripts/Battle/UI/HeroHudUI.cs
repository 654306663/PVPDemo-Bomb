using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHudUI : MonoBehaviour {

    string username;
    Text nickNameText;
    Slider hpSlider;
    Image hpImage;

    bool isInit = false;
    void Awake()
    {
        Init();
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

    private void OnDestroy()
    {
        if (BattleSyncMgr.Instance.playerDic.ContainsKey(username))
        {
            BattleSyncMgr.Instance.playerDic[username].heroData.onHpAction -= OnListenerHp;
        }
    }

    public void SetData(string username, string nickName, int hp)
    {
        Init();
        this.username = username;
        nickNameText.text = nickName;
        hpSlider.value = hp / 100f;
        if (hpSlider.value > 0.8f) hpImage.color = Color.green;
        else if (hpSlider.value > 0.4f) hpImage.color = Color.yellow;
        else hpImage.color = Color.red;


        if (BattleSyncMgr.Instance.playerDic.ContainsKey(username))
        {
            BattleSyncMgr.Instance.playerDic[username].heroData.onHpAction += OnListenerHp;
        }
    }
    
    void Init()
    {
        if (isInit) return;
        isInit = true;
        nickNameText = transform.Find("NickNameText").GetComponent<Text>();
        hpSlider = transform.Find("HpSlider").GetComponent<Slider>();
        hpImage = transform.Find("HpSlider/Fill Area/Fill").GetComponent<Image>();
    }

    void OnListenerHp(string username, int hp)
    {
        hpSlider.value = hp / 100f;
        if (hpSlider.value > 0.8f) hpImage.color = Color.green;
        else if (hpSlider.value > 0.4f) hpImage.color = Color.yellow;
        else hpImage.color = Color.red;
    }
}
