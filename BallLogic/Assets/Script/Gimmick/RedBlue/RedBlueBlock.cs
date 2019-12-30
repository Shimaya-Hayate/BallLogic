using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlueBlock : MonoBehaviour
{
    GameObject redBlueBotton;

    RedBlueBotton rBB;

    public int redOrBlue = 0; //赤ブロック：0 青ブロック：1

    bool oldRedBlue = false;

    MeshRenderer meshrenderer;
    Color color = new Color(0, 0, 0, 0);

    Collider collider;

    void Start()
    {
        redBlueBotton = GameObject.Find("RedBotton");
        rBB = redBlueBotton.GetComponent<RedBlueBotton>();
        oldRedBlue = !rBB.redBlue;

        meshrenderer = this.GetComponent<MeshRenderer>();
        color = meshrenderer.material.color;

        collider = this.GetComponent<Collider>();
    }

    void Update()
    {
        if (!oldRedBlue)
        {
            //赤ボタンが押されたら
            if (rBB.redBlue)
            {
                //自分が赤ブロックなら
                if (redOrBlue == 0)
                {
                    //実体化
                    Colored();
                }
                //自分が青ブロックなら
                else
                {
                    //透明化
                    Cleared();
                }
                oldRedBlue = rBB.redBlue;
            }
        }

        if (oldRedBlue)
        {
            //青ボタンが押されたら
            if (!rBB.redBlue)
            {
                //自分が赤ブロックなら
                if (redOrBlue == 0)
                {
                    //透明化
                    Cleared();
                }
                //自分が青ブロックなら
                else
                {
                    //実体化
                    Colored();
                }
                oldRedBlue = rBB.redBlue;
            }
        }
    }

    void Cleared()
    {
        color.a = 0.1f;
        meshrenderer.material.color = color;
        collider.isTrigger = true;
    }

    void Colored()
    {
        color.a = 1.0f;
        meshrenderer.material.color = color;
        collider.isTrigger = false;
    }
}