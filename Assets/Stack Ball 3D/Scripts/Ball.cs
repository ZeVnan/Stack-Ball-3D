using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    #region Serialize Field

    #endregion

    #region Field
    private Rigidbody rigidBody { get; set; }
    private float currentTime { get; set; }
    private bool smash { get; set; }
    private bool invincible { get; set; }
    #endregion

    #region Overridde/Implemented Method
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            smash = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            smash = false;
        }
        ProcessInvincible();
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            smash = true;
            rigidBody.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
        }
        if (rigidBody.velocity.y > 5)
        {
            rigidBody.velocity = new Vector3
                (rigidBody.velocity.x,
                5,
                rigidBody.velocity.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!smash)
        {
            rigidBody.velocity = new Vector3(
                0,
                50 * Time.deltaTime * 5,
                0);
        }
        else
        {
            if (collision.gameObject.tag == "enemy")
            {
                Destroy(collision.transform.parent.gameObject);
            }
            if (collision.gameObject.tag == "plane")
            {
                print("Over");
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!smash || collision.gameObject.tag == "Finish")
        {
            rigidBody.velocity = new Vector3(
                0,
                50 * Time.deltaTime * 5,
                0);
        }
    }
    #endregion

    #region Custom Method
    private void ProcessInvincible()
    {
        if (invincible)
        {
            currentTime -= Time.deltaTime * 0.35f;
        }
        else
        {
            if (smash)
            {
                currentTime += Time.deltaTime * 0.8f;
            }
            else
            {
                currentTime -= Time.deltaTime * 0.5f;
            }
        }
        if (currentTime >= 1)
        {
            currentTime = 1;
            invincible = true;
        }
        else if(currentTime <= 0)
        {
            currentTime = 0;
            invincible = false;
        }
    }
    #endregion
}
