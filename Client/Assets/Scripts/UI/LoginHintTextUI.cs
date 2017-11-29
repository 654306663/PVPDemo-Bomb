using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginHintTextUI : MonoBehaviour {

    public static LoginHintTextUI Instance;

    Text hintText;

    private void Awake()
    {
        Instance = this;
        hintText = GetComponent<Text>();
    }
    
    public void SetText(string text, Color color = default(Color))
    {
        hintText.text = text;
        hintText.color = color;
    }
}
