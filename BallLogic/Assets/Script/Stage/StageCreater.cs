using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreater : MonoBehaviour
{
    /*  テキスト「2　3　0　1　2　3　4　5」の場合
     *  
     *   3 4 5
     *   0 1 2
     * 
    */

   
    public GameObject[] cube;

    private string[] Data; //1文字区切りにした文字を代入
    private int[] data = new int[100];//1文字区切りにした数字を代入

    int y = 4;

    void Start()
    {
        StageCreate();
    }

    public void StageCreate()
    {
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("Stage1", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる

        //Splitでスペースで区切られた値を代入した1次配列を作成
        Data = TextLines.Split(' ');

        //文字列を数値に変換
        for (int n = 0; n < Data.Length; n++)
        {
            data[n] = int.Parse(Data[n]);
        }

        //一次元を二次元に変換
        int[,] stage = new int[data[0], data[1]];
        int x = 2;
        for (int i = 0; i < data[0]; i++)
        {
            for (int j = 0; j < data[1]; j++)
            {
                stage[i, j] = data[x];
                x++;
            }
        }

        //ステージを作成
        for (int i = 0; i < data[0]; i++)
        {
            for (int j = 0; j < data[1]; j++)
            {
                switch (stage[i, j])
                {
                    case 0:
                        Instantiate(cube[0], new Vector3(0 + j, y, 0 + i), Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(cube[0], new Vector3(0 + j, y, 0 + i), Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(cube[0], new Vector3(0 + j, y, 0 + i), Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(cube[0], new Vector3(0 + j, y, 0 + i), Quaternion.identity);
                        break;
                }
                
            }
        }
    }
}
