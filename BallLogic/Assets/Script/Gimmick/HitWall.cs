using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitWall : MonoBehaviour
{
    public GameObject player;

    PlayerController playerCon;

    void Start()
    {
         playerCon = player.GetComponent<PlayerController>();
    }

    //触れたものがPlayerならモーションをキャンセルさせる
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")//Playerなら
        {
            //キャンセル

            //初期値　を　壁に当たった座標　-　移動する距離　-　（壁とマスの誤差×ベクトル）に変更
            playerCon.posZ = playerCon.pos.z - playerCon.vZ - (0.1f * (playerCon.vZ));
            playerCon.posX = playerCon.pos.x - playerCon.vX - (0.1f * (playerCon.vX));
        }
    }
}
