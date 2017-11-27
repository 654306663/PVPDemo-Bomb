using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChooseHeroPanel : MonoBehaviour {

    public string[] heroNames;

    public Transform heroParent;

    Button randomButton;
    Button battleButton;

    InputField nickNameInput;

    string selectHeroName;

	// Use this for initialization
	void Start ()
    {
        randomButton = transform.Find("RandomButton").GetComponent<Button>();
        battleButton = transform.Find("BattleButton").GetComponent<Button>();
        nickNameInput = transform.Find("NickNameInput").GetComponent<InputField>();

        randomButton.onClick.AddListener(OnRandomEvent);
        battleButton.onClick.AddListener(OnBattleEvent);

        OnRandomEvent();
    }
	
    void OnRandomEvent()
    {
        for (int i = 0; i < heroParent.childCount; i++)
        {
            Destroy(heroParent.GetChild(i).gameObject);
        }

        selectHeroName = heroNames[Random.Range(0, heroNames.Length)];
        GameObject go = Instantiate(Resources.Load("Prefabs/Heros/" + selectHeroName) as GameObject);
        go.transform.SetParent(heroParent, false);
    }

    void OnBattleEvent()
    {
        GlobleHeroData.heroModelName = selectHeroName;
        GlobleHeroData.nickName = nickNameInput.text;
        SceneMgr.LoadScene(SceneType.GameScene);
    }

}
