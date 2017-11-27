using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {

    public GameObject registerPanel;

    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    
    /// <summary>
    /// 登录按钮事件，外部挂载
    /// </summary>
    public void OnLoginButtonEvent()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        Net.LoginRequest.Instance.SendLoginRequest(username, password);
    }

    public void OnLoginRegisterButtonEvent()
    {
        gameObject.SetActive(false);
        registerPanel.SetActive(true);
    }
}
