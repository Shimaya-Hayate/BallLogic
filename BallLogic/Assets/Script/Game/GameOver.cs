using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    BlockController blockCon;

    void Start()
    {
        blockCon = GameObject.Find("BlockController").GetComponent<BlockController>();
    }

    //触れたものがPlayerならゲームオーバー
    void OnTriggerEnter(Collider other)
    {
        //ゲームオーバー処理
        if (other.gameObject.tag == "Player")//Playerなら
        {
            blockCon.Stop(); //止める
        }    
    }
}
