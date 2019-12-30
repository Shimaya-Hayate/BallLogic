using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    CardController cardCon;

    void Start()
    {
        cardCon = GameObject.Find("CardController").GetComponent<CardController>();
    }

    //触れたものがPlayerならゲームオーバー
    void OnTriggerEnter(Collider other)
    {
        //ゲームオーバー処理
        if (other.gameObject.tag == "Player")//Playerなら
        {
            cardCon.Stop(); //止める
            Debug.Log("GameOver");
        }    
    }
}
