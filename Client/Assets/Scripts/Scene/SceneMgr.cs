using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LoginScene,
    ChooseScene,
    GameScene,
}

public class SceneMgr {

    static SceneType currentScene = SceneType.LoginScene;
    
    public static void LoadScene(SceneType targetSceneType)
    {
        SceneManager.LoadScene((int)targetSceneType);
    }

}
