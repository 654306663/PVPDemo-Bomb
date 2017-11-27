using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegisterPanel : MonoBehaviour {

    public GameObject loginPanel;

    public InputField usernameInput;
    public InputField passwordInput;
    public Button registerButton;
    public Button returnButton;

    // Use this for initialization
    void Start () {
	
	}

    /// <summary>
    /// 注册按钮事件，外部挂载
    /// </summary>
    public void OnRegisterButtonEvent()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        Net.LoginRequest.Instance.SendRegisterRequest(username, password);
    }

    public void OnReturnButtonEvent()
    {
        loginPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
