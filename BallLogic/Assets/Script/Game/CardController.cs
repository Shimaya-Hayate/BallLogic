using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public GameObject[] card; //カードのオリジナル
    Vector3 position = new Vector3(0, 0, 0); //生成座標
    public GameObject mainPanel; //親指定（フォルダ指定）

    [System.NonSerialized]
    public GameObject[] cardClone = new GameObject[100]; //クローンを保存
    int[] type = new int[100]; //ブロックの種類を保存
    int i = 0;
    int j = 0;

    bool full = false; //フルかどうか

    //Run()
    int num;
    float wateTime = 0.6f;
    bool run = false;

    public GameObject player;

    PlayerController playerCon;

    void Start()
    {
        playerCon = player.GetComponent<PlayerController>();
    }

    //ブロック生成
    public void BlockCreate(int n)
    {
        position = new Vector3(1405, 630, 0); //生成場所

        //5列3行になるよう配置
        if (i < 5)
        {
            position.x += 100 * i;
        }
        else if (i < 10)
        {
            position.x += 100 * (i - 5);
            position.y -= 100;
        }
        else if (i < 15)
        {
            position.x += 100 * (i - 10);
            position.y -= 200;
        }
        else
        {
            full = true;
        }

        //フルでなければ生成
        if (full == false)
        {
            cardClone[i] = Instantiate(card[n], position, Quaternion.identity, mainPanel.transform);
            type[i] = n; //ブロックの種類を保存
            i++;
        }
    }

    //ブロック削除(新しいやつから)
    public void BlockDelete()
    {
        j = i - 1;

        if (0 <= j)
        {
            Destroy(cardClone[j].gameObject);
            i--;
            full = false;
        }
    }

    //Run()を呼ぶ
    public void RunCall()
    {
        Time.timeScale = 1;

        //実行中でなければ
        if (run == false)
        {
            run = true;

            //リセットされていたら
            if (num == 0)
            {
                Run();
            }
        }
    }

    //プログラム実行
    void Run()
    {
        if (num < i)
        {
            //動作の種類別にパターン分け
            switch (type[num])
            {
                case 0:
                    playerCon.vZ = 1;
                    break;
                case 1:
                    playerCon.vX = 1;
                    break;
                case 2:
                    playerCon.vX = -1;
                    break;
                case 3:
                    playerCon.vZ = -1;
                    break;
                case 4:
                    playerCon.vZ = 2;
                    break;
                case 5:
                    playerCon.vX = 2;
                    break;
                case 6:
                    playerCon.vX = -2;
                    break;
                case 7:
                    playerCon.vZ = -2;
                    break;
                case 8:
                    playerCon.vZ = 1;
                    playerCon.jamp = true;
                    break;
                case 9:
                    playerCon.vX = 1;
                    playerCon.jamp = true;
                    break;
                case 10:
                    playerCon.vX = -1;
                    playerCon.jamp = true;
                    break;
                case 11:
                    playerCon.vZ = -1;
                    playerCon.jamp = true;
                    break;
            }
            if (run)
            {
                Invoke("Run", wateTime); //wateTimeの間隔で動作
            }
        }
        //すべての動作が終了したら
        else
        {
            run = false;
        }
            num++;
    }

    public void Stop()
    {
        Time.timeScale = 0;
        run = false;
    }

    //リセット
    public void Reset()
    {
        //実行中でなければ
        if (run == false)
        {
            CancelInvoke(); //Invoke処理を取り消す
            num = 0; //実行順序リセット
            playerCon.PlayerReset(); //座標と速度リセット
        }

    }
}
