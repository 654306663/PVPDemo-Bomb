using UnityEngine;
using System.Collections;
using System;

public class HeroData {

    #region  事件监听
    public Action<string, int> onHpAction;
    #endregion

    #region 属性
    private int id;
    private string username;
    private string nickName;
    private int hp;
    #endregion



    #region 方法
    public int Hp { get { return hp; } set { hp = value; if (onHpAction != null) {onHpAction(Username, hp); } } }

    public string Username { get { return username; } set { username = value; } }

    public string NickName { get { return nickName; } set { nickName = value; } }
    #endregion
}
