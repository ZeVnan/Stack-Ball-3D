using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    #region Serialize Field
    [SerializeField]
    private StackPartController[] stackPartControllers;
    #endregion

    #region Field

    #endregion

    #region Overrided/Impletented Method
    private void Start()
    {

    }
    private void Update()
    {

    }
    #endregion

    #region Custom Method
    public void ShatterAllParts()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
            FindObjectOfType<Ball>().IncreaseBrokenStacks();
        }
        foreach (StackPartController stackPartController in stackPartControllers)
        {
            stackPartController.Shatter();
        }
        StartCoroutine(RemoveParts());
    }
    private IEnumerator RemoveParts()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
    #endregion
}
