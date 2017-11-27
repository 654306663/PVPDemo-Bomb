using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Bomb : MonoBehaviour {

    private Renderer renderer;

    public BombData bombData;

    Tweener tweener;
    public void Show(BombData data)
    {
        bombData = data;

        if (renderer == null) renderer = GetComponent<Renderer>();
        tweener = renderer.material.DOColor(Color.red, 3);

        Init();
    }

    public void Reset()
    {
        if (tweener != null) tweener.Pause();

        if (renderer == null) renderer = GetComponent<Renderer>();
        renderer.material.color = Color.black;
        time = 0;
    }

    public void Open()
    {
        BattleMgr.Instance.bombMgr.RemoveBomb(bombData.id);
        GameObject go = Instantiate(Resources.Load("Prefabs/Effect/Detonator-Wide")) as GameObject;
        go.transform.position = transform.position + new Vector3(0, 1, 0);
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;
    }

    public const float g = 8;

    public float speed = 10;
    private float verticalSpeed;
    void Init()
    {
        float tmepDistance = Vector3.Distance(transform.position, bombData.endPos);
        float tempTime = tmepDistance / speed;
        float riseTime, downTime;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;

        transform.LookAt(bombData.endPos);
    }
    private float time;
    void FixedUpdate()
    {
        if (transform.position.y < 0)
        {
            return;
        }
        time += Time.deltaTime;
        float test = verticalSpeed - g * time;
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        transform.Translate(transform.up * test * Time.deltaTime, Space.World);
    }
}
