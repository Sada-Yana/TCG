using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {
    public TCG tcg_data;
    public List<TCG.Param> mines;
    public List<TCG.Param> yours;

    public Queue<TCG.Param> tcg_ = new Queue<TCG.Param>();

    int i = 0;


   [SerializeField] int deck_count;
    public Image[] ima;

    public Text mine_num;
    public Text your_num;
    public Text deck_num;
    public Text card;

    public Text mine_point;
    public Text your_point;

    public Text result;

    public int mp = 0;
    public int yp = 0;

    [SerializeField] bool player_play = true;
	// Use this for initialization
	void Start () {


        Temple();

        

	}
	
	// Update is called once per frame
	void Update () {
        Text();
        Turn();

    }

    /// <summary>
    /// ターン受け渡し処理
    /// </summary>
    void Turn()
    {
        if (tcg_.Count > 0 && !(tcg_.Count == 1 && (mines.Count == 1 || yours.Count == 1)))
        {
            if (!player_play)
                AI();
            else
                Use_Card();
        }

        if(tcg_.Count <= 1)
        {
            Result();
        }
    }


    /// <summary>
    /// リザルト
    /// </summary>
    void Result()
    {
        if(mp > yp)
        {
            result.text = "貴方の勝ち";
        }
         else if(yp > mp)
        {
            result.text = "貴方の負け";
        }
        else
        {
            result.text = "引き分け";
        }
        if(Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(0);
    }

    /// 
    /// <summary>
    /// 初期化
    /// </summary>
    void Temple()
    {
        tcg_data.param.Shuffle();
        deck_count = tcg_data.param.Count;
        for (i = 0; i < tcg_data.param.Count; i++)
        {
            tcg_.Enqueue((TCG.Param)tcg_data.param[i].Clone());
        }
        for (i = 0; i < 4; i++)
        {
            mines.Add(tcg_.Dequeue());
            deck_count--;
            yours.Add(tcg_.Dequeue());
            deck_count--;
        }
    }

    /// <summary>
    /// カード使用
    /// </summary>
    void  Use_Card()
    {
        if (deck_count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Card_Effected(mines[0]);
                mines.RemoveAt(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Card_Effected(mines[1]);
                mines.RemoveAt(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Card_Effected(mines[2]);
                mines.RemoveAt(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Card_Effected(mines[3]);
                mines.RemoveAt(3);
            }


            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Card_Effected(mines[4]);
                mines.RemoveAt(4);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Card_Effected(mines[5]);
                mines.RemoveAt(5);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Card_Effected(mines[6]);
                mines.RemoveAt(6);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Card_Effected(mines[7]);
                mines.RemoveAt(7);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Card_Effected(mines[8]);
                mines.RemoveAt(8);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Card_Effected(mines[9]);
                mines.RemoveAt(9);
            }
        }
    }

    /// <summary>
    /// テキスト表示
    /// </summary>
    void Text()
    {
        mine_num.text = "手札の枚数" + mines.Count.ToString();
        your_num.text = "相手の手札の枚数" + yours.Count.ToString();
        deck_num.text = "デッキの枚数" + deck_count.ToString();

        mine_point.text = "あなたの点" + mp;
        your_point.text = "相手の点" + yp;

        card.text =  "0: " + Substitution(mines[0].effect) + " " + mines[0].point + "\n";
        for(i = 1; i <= mines.Count - 1;i++)
        {
            card.text += i + ": " + Substitution(mines[i].effect) + " " + mines[i].point + "\n";
        }

    }


    /// <summary>
    /// 置換
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    string Substitution(int effect)
    {
        string str = "";
        if(effect == 1)
        {
            str = "ドロー";
        }
        else if(effect == 2)
        {
            str = "アド";
        }
        else if(effect == 3)
        {
            str = "ペナルティ";
        }
        else if(effect == 4)
        {
            str = "アドアド";
        }
        else if(effect == 5)
        {
            str = "ペナペナ";
        }
        else if(effect == 6)
        {
            str = "最強";
        }


        return str;
    }

    /// <summary>
    /// 効果の判定
    /// ターン終了の判定もここ
    /// </summary>
    /// <param name="card"></param>
    void Card_Effected(TCG.Param card)
    {
            switch (card.effect)
            {
                case 1: Card_Draw(card.point); break;
                case 2: Player_Add(card); break;
                case 3: Enemy_Penalty(card); break;
                case 4: Special(card); break;
                default: Special(card); break;
            }
            Card_Draw(1);
            player_play = !player_play;
        
    }

    /// <summary>
    /// ドロー
    /// 終了判定もここ
    /// </summary>
    /// <param name="card"></param>
    void Card_Draw(int card)
    {
        if (deck_count > card)
        {
            if (player_play)
                for (var i = 0; i < card; i++)
                {
                    mines.Add(tcg_.Dequeue());
                    deck_count--;
                }
            else
                for (var i = 0; i < card; i++)
                {
                    yours.Add(tcg_.Dequeue());
                    deck_count--;
                }
        }
    }

    /// <summary>
    /// アド
    /// </summary>
    /// <param name="card"></param>
    void Player_Add(TCG.Param card)
    {

            if (player_play && mines.Count - 1 > card.point)
            {
                for (i = 0; i < card.point; i++)
                {
                    mp++;
                    mines.RemoveAt(Random.Range(0, mines.Count - 1));
                }
            }
            else if(!player_play && yours.Count - 1 > card.point)
            {
                for (i = 0; i < card.point; i++)
                {
                    yp++;
                    yours.RemoveAt(Random.Range(0, mines.Count - 1));
                }
            }
        
    }
    /// <summary>
    /// ペナ
    /// </summary>
    void Enemy_Penalty(TCG.Param card)
    {
            if (player_play && mines.Count - 1 > card.point)
            {
                for (i = 0; i < card.point; i++)
                {
                    yp--;
                    mines.RemoveAt(Random.Range(0, mines.Count - 1));
                }
            }
            else if(!player_play && yours.Count - 1 > card.point)
            {
                for (i = 0; i < card.point; i++)
                {
                    mp--;
                    yours.RemoveAt(Random.Range(0, mines.Count - 1));
                }
            }
        
    }

    /// <summary>
    /// スペシャル関連
    /// </summary>
    /// <param name="card"></param>
    void Special(TCG.Param card)
    {
        if (player_play)
        {
            if (card.effect == 4)
            {
                for (i = 0; i < card.point; i++)
                {
                    mp++;
                }
            }

            if (card.effect == 5)
            {
                for (i = 0; i < card.point; i++)
                {
                    yp--;
                }
            }

            if (card.effect == 6)
            {
                for (i = 0; i < card.point; i++)
                {
                    mp++;
                    yp--;
                }
            }
        }

        else
        {
            if (card.effect == 4)
            {
                for (i = 0; i < card.point; i++)
                {
                    yp++;
                }
            }

            if (card.effect == 5)
            {
                for (i = 0; i < card.point; i++)
                {
                    mp--;
                }
            }

            if (card.effect == 6)
            {
                for (i = 0; i < card.point; i++)
                {
                    yp++;
                    mp--;
                }
            }
        }
    }



    void AI()
    {

        int  tmp_add = -1;
        int tmp_draw = -1;
        int tmp_pen = -1;
        int tmp_spe = -1;

        int eff = 0;
        int i = 0;

        for( i = 0; i < yours.Count;i++)
        {
            if (yours[i].effect == 2)
                tmp_add = i;
            if (yours[i].effect == 3)
                tmp_pen = i;
            if (yours[i].effect == 1)
                tmp_draw = i;
            if (yours[i].effect >= 4)
                tmp_spe = i;
        }


        if (tmp_spe >= 0)
        {
            eff = tmp_spe;
        }
        else if (tmp_add >= 0)
            eff = tmp_add;
        else if (tmp_draw >= 0)
            eff = tmp_draw;
        else if (tmp_pen >= 0)
            eff = tmp_pen;

        Card_Effected(yours[eff]);
        yours.RemoveAt(eff);
        Debug.Log(eff);

    }
}
