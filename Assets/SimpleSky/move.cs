using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkyManager : MonoBehaviour
{

    public Transform skyTran;
    public Transform croudParentTran;
    public GameObject star;
    public GameObject sun;
    public GameObject moon;
    public Material skyMaterial;

    private List<Transform> croudList;
    private float timeSpeed = 5f;   // 5秒で一周するように
    private float skyRotateSpeed = 0;
    private float cloudMoveSpeed = 1f;
    private float nowTime = 0;


    void Awake()
    {
        skyRotateSpeed = 360f / timeSpeed;
        croudList = new List<Transform>();
        foreach (Transform tran in croudParentTran)
        {
            croudList.Add(tran);
        }
    }

    void Update()
    {
        nowTime += Time.deltaTime;
        float nowValue = Mathf.Clamp(nowTime / timeSpeed, 0f, 1f);
        if (nowTime > timeSpeed) nowTime = 0;
        skyMaterial.SetTextureOffset("_MainTex", new Vector2(nowValue, 0));
        skyTran.Rotate(new Vector3(0, skyRotateSpeed * Time.deltaTime, 0));
        foreach (Transform tran in croudList)
        {
            tran.transform.position += new Vector3(cloudMoveSpeed * Time.deltaTime, 0, 0);
        }

        if (0.2 < nowValue && nowValue < 0.6f)
        {
            star.SetActive(true);
            moon.SetActive(true);
            sun.SetActive(false);
        }
        else
        {
            star.SetActive(false);
            moon.SetActive(false);
            sun.SetActive(true);
        }
    }
}