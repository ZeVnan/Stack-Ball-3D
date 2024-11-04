using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Serialize Field

    #endregion

    #region Field
    private Vector3 cameraFollow { get; set; }
    private Transform ballTransform { get; set; }
    private Transform winTransform { get; set; }
    #endregion
    
    #region Overrided/Impletented Method
    private void Start()
    {
        ballTransform = FindObjectOfType<Ball>().transform;
    }
    private void Update()
    {
        if (winTransform == null)
        {
            winTransform = GameObject.Find("Win(Clone)").GetComponent<Transform>();
        }
        if (this.transform.position.y > ballTransform.position.y &&
            this.transform.position.y > winTransform.position.y + 4f)
        {
            cameraFollow = new Vector3(
                this.transform.position.x,
                ballTransform.position.y,
                this.transform.position.z);
        }
        this.transform.position = new Vector3(
            this.transform.position.x,
            cameraFollow.y,
            -5);
    }
    #endregion
    
    #region Custom Method

    #endregion
}
