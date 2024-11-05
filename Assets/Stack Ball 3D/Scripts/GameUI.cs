using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    #region Serialize Field
    [Header("In Game")]
    [SerializeField]
    private Image levelSlider;
    [SerializeField]
    private Image currentLevelImage;
    [SerializeField]
    private Image nextLevelImage;
    #endregion

    #region Field
    private Material ballMaterial;
    #endregion

    #region Overrided/Impletented Method
    private void Awake()
    {
        ballMaterial = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        levelSlider.transform.parent.GetComponent<Image>().color = ballMaterial.color + Color.gray;
        currentLevelImage.color = ballMaterial.color;
        nextLevelImage.color = ballMaterial.color;
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    #endregion
    
    #region Custom Method
    public void FillLevelSlider(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }
    #endregion
}
