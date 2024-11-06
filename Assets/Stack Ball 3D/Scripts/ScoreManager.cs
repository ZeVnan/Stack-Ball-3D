using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    #region Serialize Field
    [SerializeField]
    public int Score = 0;
    #endregion

    #region Field
    private Text scoretext { get; set; }

    #endregion

    #region Overrided/Impletented Method
    protected override void Awake()
    {
        base.Awake();
        scoretext = GameObject.Find("ScoreText").GetComponent<Text>();
    }
    private void Start()
    {
        AddScore(0);
    }
    private void Update()
    {
        if (scoretext == null)
        {
            scoretext = GameObject.Find("ScoreText").GetComponent<Text>(); 
        }
        scoretext.text = Score.ToString();
    }
    #endregion
    
    #region Custom Method
    public void AddScore(int amount)
    {
        Score += amount;
        if (Score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", Score);
        }
    }
    public void ResetScore()
    {
        Score = 0;
    }
    #endregion
}
