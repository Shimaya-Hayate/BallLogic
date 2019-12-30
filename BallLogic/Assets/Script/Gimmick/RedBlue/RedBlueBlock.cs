using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlueBlock : MonoBehaviour
{
    GameObject redBlueBotton;

    RedBlueBotton rBB;

    public int redOrBlue = 0; //赤ブロック：0 青ブロック：1

    bool oldRedBlue = false;

    void Start()
    {
        redBlueBotton = GameObject.Find("RedBotton");   
        rBB = redBlueBotton.GetComponent<RedBlueBotton>();
        oldRedBlue = !rBB.redBlue;
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
                    Debug.Log("赤実体化");
                }
                //自分が青ブロックなら
                else
                {
                    //透明化
                    Debug.Log("青透明化");
                }
                oldRedBlue = rBB.redBlue;
            }
        }
        
        if(oldRedBlue)
        {
            //青ボタンが押されたら
            if (!rBB.redBlue)
            {
                //自分が赤ブロックなら
                if (redOrBlue == 0)
                {
                    //透明化
                    Debug.Log("赤透明化");
                }
                //自分が青ブロックなら
                else
                {
                    //実体化
                    Debug.Log("青実体化");
                }
                oldRedBlue = rBB.redBlue;
            }
        }
    }
}
