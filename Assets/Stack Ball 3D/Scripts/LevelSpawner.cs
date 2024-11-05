using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawner : MonoBehaviour
{
    #region Serialize Field
    [SerializeField]
    private GameObject[] model;
    [SerializeField]
    private GameObject winPrefab;
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private int addOn = 7;
    #endregion

    #region Field
    public GameObject[] ModelPrefab { get; set; } = new GameObject[4];
    private GameObject temp1 { get; set; }
    private GameObject temp2 { get; set; }
    private float i { get; set; } = 0;
    #endregion
    
    #region Overrided/Impletented Method
    private void Awake()
    {
        level = PlayerPrefs.GetInt("Level", 1);
        if (level > 9)
        {
            addOn = 0;
        }
        ModelSelection();
        float random = Random.value;
        for (i = 0; i > - level - addOn; i -= 0.5f)
        {
            if (level <= 20)
            {
                temp1 = Instantiate(ModelPrefab[Random.Range(0, 2)]);
            }
            else if (level <= 50)
            {
                temp1 = Instantiate(ModelPrefab[Random.Range(1, 3)]);
            }
            else if (level <= 100)
            {
                temp1 = Instantiate(ModelPrefab[Random.Range(2, 4)]);
            }
            else
            {
                temp1 = Instantiate(ModelPrefab[Random.Range(3, 4)]);
            }
            temp1.transform.position = new Vector3(0, i - 0.01f, 0);
            temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
            if (Mathf.Abs(i) >= level * 0.3f && Mathf.Abs(i) <= level * 0.6f)
            {
                temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
                temp1.transform.eulerAngles += Vector3.up * 180;
            }
            else if (Mathf.Abs(i) >= level * 0.8f)
            {
                temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
                if (random > 0.75f)
                {
                    temp1.transform.eulerAngles += Vector3.up * 180;
                }
            }
            temp1.transform.parent = FindObjectOfType<Rotator>().transform;
        }
        temp2 = Instantiate(winPrefab);
        temp2.transform.position = new Vector3(0, i - 0.01f, 0);
    }
    private void Update()
    {
        
    }
    #endregion
    
    #region Custom Method
    public void NextLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(0);
    }
    private void ModelSelection()
    {
        int randomNumber = Random.Range(0, 5);
        for (int i = 0; i < 4; i++)
        {
            ModelPrefab[i] = model[i + randomNumber * 4];
        }
    }
    #endregion
}
