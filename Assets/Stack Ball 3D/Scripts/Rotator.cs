using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    #region Serialize Field
    [SerializeField]
    private float speed = 100;
    #endregion

    #region Field

    #endregion
    
    #region Overrided/Impletented Method
    private void Start()
    {
        
    }
    private void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
    #endregion
    
    #region Custom Method

    #endregion
}
