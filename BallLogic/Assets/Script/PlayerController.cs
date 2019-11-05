﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float v = 0.1f; //速度
    float vX, vZ; //方向
    float posX, posZ; //Z座標
    bool stop = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform myTransform = this.transform;
        Vector3 pos = myTransform.position;

        //静止中なら
        if(stop)
        {
            if (Input.GetKeyDown("up"))
                vZ = 1;

            if (Input.GetKeyDown("down"))
                vZ = -1;

            if(vZ != 0)
            {
                vX = 0;
                stop = false;
                posZ = pos.z; //座標を保存
            }

            if (Input.GetKeyDown("right"))
                vX = 1;

            if (Input.GetKeyDown("left"))
                vX = -1;

            if (vX != 0)
            {
                vZ = 0;
                stop = false;
                posX = pos.x; //座標を保存
            }
        }
        //移動中なら
        else
        {
            if (vZ != 0)
            {
                pos.z += vZ * v;

                if ((posZ + vZ) * vZ <= pos.z * vZ)
                {
                    pos.z = posZ + vZ;
                    stop = true;
                    vZ = 0;
                }
            }

            if (vX != 0)
            {
                pos.x += vX * v;

                if ((posX + vX) * vX <= pos.x * vX)
                {
                    pos.x = posX + vX;
                    stop = true;
                    vX = 0;
                }
            }
        }

        myTransform.position = pos; //座標の更新
    }
}