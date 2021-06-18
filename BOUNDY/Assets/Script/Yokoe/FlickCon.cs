﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickCon : MonoBehaviour
{
    /// <summary>
    /// 現在の座標を取得する間隔
    /// </summary>
    int count = 0;

    const int proccesing = 1;

    /// <summary>
    /// 座標を保存できる数
    /// </summary>
    const int AccuracyLebel = 5;

    /// <summary>
    /// 座標の配列
    /// </summary>
    private Vector3[] pos = new Vector3[AccuracyLebel];

    /// <summary>
    /// 座標をとった回数
    /// </summary>
    private int poscount = 0;

    /// <summary>
    /// 平均速度
    /// </summary>
    private float averagespeed = 0;

    void PosClear()
    {
        for (int i = 0; i < AccuracyLebel; ++i)
        {
            pos[i] = Vector3.zero;
        }
        poscount = 0;
        averagespeed = 0;
    }

    void CheckFlickSpeed()
    {
        float sum = 0;
        int max = AccuracyLebel > poscount ? poscount : AccuracyLebel;
        for (int i = 0; i < max - 1; ++i)
        {
            sum += (pos[i] - pos[i + 1]).sqrMagnitude;
        }
        averagespeed = sum / max * 10;
    }

    void SavePos()
    {
        if (++count % proccesing != 0) return;

        pos[poscount % AccuracyLebel] = Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position);
        ++poscount;
    }

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public int Flisk_task()
    {
        if (Input.GetMouseButton(0))
        {
            SavePos();
            CheckFlickSpeed();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (averagespeed > 2f)
            {
                Vector3 Direction;
                int nowpos = (poscount - 1) % AccuracyLebel;
                if (nowpos == 0)
                {
                    //現在保存している座標の配列が0番の時
                    Direction = pos[nowpos] - pos[AccuracyLebel - 1];
                }
                else
                {
                    Direction = pos[nowpos] - pos[nowpos - 1];
                }

                if (Direction.x > 0) 
                {
                    Debug.Log("a");
                    return 1;   
                }
                else
                {
                    Debug.Log("b");
                    return -1;
                }
               
            }

            PosClear();
        }
       
        if (count == int.MaxValue) count = 0;
        return 0;
    }
}
