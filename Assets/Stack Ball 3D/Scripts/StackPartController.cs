using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPartController : MonoBehaviour
{
    #region Serialize Field

    #endregion

    #region Field
    private Rigidbody rigidBody { get; set; }
    private MeshRenderer meshRenderer { get; set; }
    private StackController stackController { get; set; }
    private Collider collideR { get; set; }
    #endregion

    #region Overrided/Impletented Method
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        collideR = GetComponent<Collider>();
        stackController = transform.parent.GetComponent<StackController>();
    }
    private void Start()
    {

    }
    private void Update()
    {

    }
    #endregion

    #region Custom Method
    public void Shatter()
    {
        rigidBody.isKinematic = false;
        collideR.enabled = false;

        Vector3 forcePoint = transform.parent.position;
        float parentXPosition = transform.parent.position.x;
        float xPosition = meshRenderer.bounds.center.x;

        Vector3 subDirection = (parentXPosition - xPosition < 0) ? Vector3.right : Vector3.left;
        Vector3 direction = (Vector3.up * 1.5f + subDirection).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        rigidBody.AddForceAtPosition(
            direction * force,
            forcePoint,
            ForceMode.Impulse);
        rigidBody.AddTorque(Vector3.left * torque);
        rigidBody.velocity = Vector3.down;
    }
    public void RemoveAllChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }
    #endregion
}
