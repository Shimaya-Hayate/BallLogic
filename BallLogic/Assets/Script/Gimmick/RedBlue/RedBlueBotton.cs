﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlueBotton : MonoBehaviour
{
    [System.NonSerialized]
    public bool redBlue = true; //赤true：青false

    public int redOrBlue = 0; //赤ボタン：0 青ボタン：1

    GameObject redBlueBotton;

    RedBlueBotton rBB;

    void Start()
    {
        redBlueBotton = GameObject.Find("RedBotton");
        rBB = redBlueBotton.GetComponent<RedBlueBotton>();
    }

    //ボタンの動作
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")//Playerなら
        {
            switch (redOrBlue)
            {
                //赤なら
                case 0:
                    redBlue = true;
                    break;

                //青なら
                case 1:
                    rBB.redBlue = false;
                    break;
            }
        }
    }
}
