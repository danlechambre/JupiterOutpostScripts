using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingsIcons : MonoBehaviour
{
    [SerializeField] private RankingsScriptableObject rankingsData;

    [SerializeField] private Image[] rankingsImages;

    [SerializeField] private Sprite noRank;
    [SerializeField] private Sprite rank1;
    [SerializeField] private Sprite rank2;
    [SerializeField] private Sprite rank3;

    public Sprite Rank1Img { get { return rank1; } }
    public Sprite Rank2Img { get { return rank2; } }
    public Sprite Rank3Img { get { return rank3; } }

    void Start()
    {
        RefreshRankings();
    }

    public void RefreshRankings()
    {
        for (int i = 0; i < rankingsData.Rankings.Length; i++)
        {
            switch (rankingsData.Rankings[i])
            {
                case 0:
                    rankingsImages[i].sprite = noRank;
                    break;
                case 1:
                    rankingsImages[i].sprite = rank1;
                    break;
                case 2:
                    rankingsImages[i].sprite = rank2;
                    break;
                case 3:
                    rankingsImages[i].sprite = rank3;
                    break;
            }
        }
    }
}
