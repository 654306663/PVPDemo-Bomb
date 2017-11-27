using UnityEngine;
using System.Collections;

public enum BombType
{
    Type1
}

public class BombData {

    public string username;
    public int id;
    public BombType type;
    public float durationTime;
    public float damageRange;
    public Vector3 startPos;
    public Vector3 endPos;

}
