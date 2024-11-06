using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Ball;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    #region Serialize Field
    [SerializeField]
    private GameObject homeUI, inGameUI, finishUI, gameOverUI, allButtons;
    [Header("In Game")]
    [SerializeField]
    private Image levelSlider;
    [SerializeField]
    private Image currentLevelImage, nextLevelImage;
    [SerializeField]
    private Text currentLevelText, nextLevelText;
    [Header("PreGame")]
    [SerializeField]
    private Button soundButton;
    [SerializeField]
    private Sprite soundOn, soundOff;
    [Header("Finish")]
    [SerializeField]
    private Text finishLevelText;
    [Header("Gameover")]
    [SerializeField]
    private Text scoreValueText, bestScoreValueText;
    #endregion

    #region Field
    private Material ballMaterial { get; set; }
    private bool isButtonsOn { get; set; } = false;
    private Ball ball { get; set; }
    #endregion

    #region Overrided/Impletented Method
    private void Awake()
    {
        ballMaterial = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        levelSlider.transform.parent.GetComponent<Image>().color = ballMaterial.color + Color.gray;
        currentLevelImage.color = ballMaterial.color;
        nextLevelImage.color = ballMaterial.color;
        ball = FindObjectOfType<Ball>();
        soundButton.onClick.AddListener(() => SoundManager.Instance.ToggleSound());
        soundButton.onClick.AddListener(() => this.ToggleSound());
    }
    private void Start()
    {
        allButtons.SetActive(isButtonsOn);
        currentLevelText.text = $"{FindObjectOfType<LevelSpawner>().Level}";
        nextLevelText.text = $"{FindObjectOfType<LevelSpawner>().Level + 1}";
    }
    private void Update()
    {
        switch (ball.ballState)
        {
            case BallState.Prepare:
                if (Input.GetMouseButtonDown(0) && !IgnoreUI())
                {
                    ball.ballState = BallState.Playing;
                    homeUI.SetActive(false);
                    inGameUI.SetActive(true);
                    finishUI.SetActive(false);
                    gameOverUI.SetActive(false);
                }
                break;
            case BallState.Finish:
                homeUI.SetActive(false);
                inGameUI.SetActive(false);
                finishUI.SetActive(true);
                gameOverUI.SetActive(false);
                finishLevelText.text = $"Level {FindObjectOfType<LevelSpawner>().Level}";
                break;
            case BallState.Died:
                homeUI.SetActive(false);
                inGameUI.SetActive(false);
                finishUI.SetActive(false);
                gameOverUI.SetActive(true);
                scoreValueText.text = ScoreManager.Instance.Score.ToString();
                bestScoreValueText.text = PlayerPrefs.GetInt("HighScore").ToString();
                if (Input.GetMouseButtonDown(0))
                {
                    ScoreManager.Instance.ResetScore();
                    SceneManager.LoadScene(0);
                }
                break;
        }
    }
    #endregion
    
    #region Custom Method
    public void FillLevelSlider(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }
    public void ToggleSettings()
    {
        isButtonsOn = !isButtonsOn;
        allButtons.SetActive(isButtonsOn);
    }
    //toggle sprite of sound button
    private void ToggleSound()
    {
        if (SoundManager.Instance.sound)
        {
            soundButton.GetComponent<Image>().sprite = soundOn;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundOff;
        }
    }
    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i< raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }
        return raycastResults.Count > 0;
    }
    #endregion
}
