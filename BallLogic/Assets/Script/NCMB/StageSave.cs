using NCMB;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageSave : MonoBehaviour
{
    public string rankingClassName = "Stage";//NCMB側のランキングクラス名//
    public int count = 10;//いくつまでランキングデータを取得するか//
    private List<RankingData> topRankingDataList = new List<RankingData>();//取得した上位ランキングデータ//
    private List<RankingData> nearRankingDataList = new List<RankingData>();//取得したランキングデータ//

    public bool IsRankingDataValid { get; private set; }//ランキングデータの取得に成功しているか//

    public int PlayerCount { get; private set; }//いままで何人がスコアを登録したか//

    private string currentObjectid;//自分のスコアのidを一時保存する//

    public static StageSave Instance;//シングルトン//

    public int currentRank = 0;

    //Startより早く呼ばれる
    public void Awake()
    {
        FetchTopRanking();

        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            CheckNCMBValid();
        }
    }

    // 現プレイヤーのハイスコアを受けとってランクを取得 ---------------
    public void fetchRank(int currentScore)
    {
        // データスコアの検索
        NCMBQuery<NCMBObject> rankQuery = new NCMBQuery<NCMBObject>(rankingClassName);
        rankQuery.WhereGreaterThan("Score", currentScore);
        rankQuery.CountAsync((int count, NCMBException e) => {

            if (e != null)
            {
                //件数取得失敗
            }
            else
            {
                //件数取得成功
                currentRank = count + 1; // 自分よりスコアが上の人がn人いたら自分はn+1位
            }
            FetchNearRanking();
        });
    }

    public void FetchTopRanking(UnityAction callback = null)
    {
        if (CheckNCMBValid() == false)
        {
            if (callback != null) callback();
            return;
        }

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);

        //Scoreの値で降順にソート//
        query.OrderByDescending("Score");

        //取得数の設定//
        query.Limit = count;

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                //検索失敗時の処理
                IsRankingDataValid = false;
            }
            else
            {
                //上位ランキングのデータ
                int num = 1;

                topRankingDataList.Clear();

                foreach (NCMBObject obj in objList)
                {
                    topRankingDataList.Add(new RankingData(
                         num++,
                         name: obj["Name"] as string,
                         score: Convert.ToInt32(obj["Score"]),
                         objectid: obj.ObjectId

                        ));
                }
                IsRankingDataValid = true;
            }

            if (callback != null) callback();
        });
    }

    public void FetchNearRanking(UnityAction callback = null)
    {
        if (CheckNCMBValid() == false)
        {
            if (callback != null) callback();
            return;
        }

        int numSkip = currentRank - 4;
        if (numSkip < 0) numSkip = 0;

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);

        //Scoreの値で降順にソート//
        query.OrderByDescending("Score");

        //スキップする
        query.Skip = numSkip;

        //取得数の設定//
        query.Limit = count;

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                //検索失敗時の処理
                IsRankingDataValid = false;
            }
            else
            {
                //周辺のデータ
                int num = numSkip + 1;

                nearRankingDataList.Clear();

                foreach (NCMBObject obj in objList)
                {
                    nearRankingDataList.Add(new RankingData(
                         num++,
                         name: obj["Name"] as string,
                         score: Convert.ToInt32(obj["Score"]),
                         objectid: obj.ObjectId

                        ));
                }
                IsRankingDataValid = true;
            }

            if (callback != null) callback();
        });
    }

    public void SaveRanking(string name, int score, UnityAction callback = null)
    {
        //スコアがゼロなら登録しない//
        if (CheckNCMBValid() == false || score <= 0)
        {
            if (callback != null) callback();
            return;
        }

        //rankingClassNameに設定したオブジェクトを作る//
        NCMBObject ncmbObject = new NCMBObject(rankingClassName);

        //nameが空だったらNoNameと入れる//
        if (string.IsNullOrEmpty(name)) name = "No Name";

        // オブジェクトに値を設定
        ncmbObject["Name"] = name;
        ncmbObject["Score"] = score;

        // データストアへの登録
        ncmbObject.SaveAsync((NCMBException e) =>
        {
            if (e != null)
            {
                //接続失敗//
            }
            else
            {
                //接続成功//
                //保存したオブジェクトidを記録//
                currentObjectid = ncmbObject.ObjectId;
            }

            //ランキングの更新//
            if(callback != null)
            {
                FetchTopRanking(callback);
                FetchNearRanking(callback);
            }
            else
            {
                fetchRank(score);
                FetchTopRanking();
            }

        });
    }

    public List<RankingData> GetRanking()
    {
        //すでにStart()でフェッチ済みのデータを渡すだけ//
        return topRankingDataList;
    }

    public string GetRankingByText()
    {
        if (IsRankingDataValid)
        {
            string text = string.Empty;

            foreach (RankingData rankingData in topRankingDataList)
            {
                string rankNum = string.Format("{0, 2}", rankingData.rankNum);
                string name = string.Format("{0, -10}", rankingData.name);
                string score = string.Format("{0, -10}", rankingData.score.ToString());

                //さっき保存したスコアがあった場合は赤に着色する//
                if (rankingData.objectid == currentObjectid)
                {
                    text += "<color=red>" + rankNum + ": \t" + name + ": \t" + score + "</color> \n";
                }
                else
                {
                    text += rankNum + ": \t" + name + ": \t" + score + "\n";
                }
            }

            return text;
        }
        else
        {
            return "No Ranking Data";
        }
    }

    //周りのランキング----------------------------------------------------------------------
    public string GetNearRankingByText()
    {
        if (IsRankingDataValid)
        {
            string text = string.Empty;

            foreach (RankingData rankingData in nearRankingDataList)
            {
                string rankNum = string.Format("{0, 2}", rankingData.rankNum);
                string name = string.Format("{0, -10}", rankingData.name);
                string score = string.Format("{0, -10}", rankingData.score.ToString());

                //さっき保存したスコアがあった場合は赤に着色する//
                if (rankingData.objectid == currentObjectid)
                {
                    text += "<color=red>" + rankNum + ": \t" + name + ": \t" + score + "</color> \n";
                }
                else
                {
                    text += rankNum + ": \t" + name + ": \t" + score + "\n";
                }
            }

            return text;
        }
        else
        {
            return "No Ranking Data";
        }
    }

    public void FetchPlayerCount(UnityAction callback = null)
    {
        if (CheckNCMBValid() == false)
        {
            if (callback != null) callback();
            return;
        }

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(rankingClassName);
        query.CountAsync((int count, NCMBException e) => {
            if (e != null)
            {
                //接続失敗//
            }
            else
            {
                //接続成功//
                PlayerCount = count;
            }
            if (callback != null) callback();
        });
    }

    private bool CheckNCMBValid()
    {
#if UNITY_WEBGL
            Debug.LogWarning("NCMB SDK はWebGLに対応していません。");
            return false;
#else
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("ネットワーク接続がありません。");
            return false;
        }
        else
        {
            return true;
        }
#endif
    }
}

public class RankingData
{
    public readonly int rankNum;//順位（本クラス内でつける）//
    public readonly string name;//プレイヤー名//
    public readonly int score;//点数//
    public readonly string objectid;//NCMBのオブジェクトID//

    public RankingData(int rankNum, string name, int score, string objectid)
    {
        this.rankNum = rankNum;
        this.name = name;
        this.score = score;
        this.objectid = objectid;
    }
}