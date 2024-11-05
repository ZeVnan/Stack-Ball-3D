using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    #region Serialize Field
    [SerializeField]
    private AudioClip bounceOffClip;
    [SerializeField]
    private AudioClip deadClip;
    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip destroyClip;
    [SerializeField]
    private AudioClip iDestroyClip;
    [Range(0f, 1f)]
    [SerializeField]
    private float soundFXVolumeScale = 0.5f;
    [SerializeField]
    private GameObject invincibleObject;
    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private Image invincibleFill;
    #endregion

    #region Field
    public enum BallState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }
    public BallState ballState { get; set; } = BallState.Prepare;
    private Rigidbody rigidBody { get; set; }
    private float currentTime { get; set; }
    private bool smash { get; set; }
    private bool invincible { get; set; }
    private int currentBrokenStacks { get; set; }
    private int totalStacks { get; set; }
    #endregion

    #region Overridde/Implemented Method
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        
    }
    private void Start()
    {
        totalStacks = FindObjectsOfType<StackController>().Length;
    }
    private void Update()
    {
        switch (this.ballState)
        {
            case BallState.Playing:
                if (Input.GetMouseButtonDown(0))
                {
                    smash = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    smash = false;
                }
                ProcessInvincible();
                break;
            case BallState.Prepare:
                if (Input.GetMouseButtonDown(0))
                {
                    ballState = BallState.Playing;
                }
                break;
            case BallState.Finish:
                if (Input.GetMouseButtonDown(0))
                {
                    FindObjectOfType<LevelSpawner>().NextLevel();
                }
                break;
        }
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
            rigidBody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            SoundManager.Instance.PlaySoundFX(bounceOffClip, soundFXVolumeScale);
        }
        else
        {
            if (invincible)
            {
                collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
            }
            else
            {
                if (collision.gameObject.tag == "enemy")
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
                if (collision.gameObject.tag == "plane")
                {
                    print("Over");
                    ScoreManager.Instance.ResetScore();
                    SoundManager.Instance.PlaySoundFX(deadClip, soundFXVolumeScale);
                }
            }
        }
        FindObjectOfType<GameUI>().FillLevelSlider(currentBrokenStacks / (float)totalStacks);
        if (collision.gameObject.tag == "Finish" && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.Instance.PlaySoundFX(winClip, soundFXVolumeScale);
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
    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;
        if (!invincible)
        {
            ScoreManager.Instance.AddScore(1);
            SoundManager.Instance.PlaySoundFX(destroyClip, soundFXVolumeScale);
        }
        else
        {
            ScoreManager.Instance.AddScore(2);
            SoundManager.Instance.PlaySoundFX(iDestroyClip, soundFXVolumeScale);
        }
    }
    private void ProcessInvincible()
    {
        if (invincible)
        {
            currentTime -= Time.deltaTime * 0.35f;
            if (!fireEffect.activeInHierarchy)
            {
                fireEffect.SetActive(true);
            }
        }
        else
        {
            if (fireEffect.activeInHierarchy)
            {
                fireEffect.SetActive(false);
            }
            if (smash)
            {
                currentTime += Time.deltaTime * 0.8f;
            }
            else
            {
                currentTime -= Time.deltaTime * 0.5f;
            }
        }
        if (currentTime > 0.3f || invincibleFill.color == Color.red)
        {
            invincibleObject.SetActive(true);
        }
        else
        {
            invincibleObject.SetActive(false);
        }
        if (currentTime >= 1)
        {
            currentTime = 1;
            invincible = true;
            invincibleFill.color = Color.red;
        }
        else if(currentTime <= 0)
        {
            currentTime = 0;
            invincible = false;
            invincibleFill.color = Color.white;
        }
        if (invincibleObject.activeInHierarchy)
        {
            invincibleFill.fillAmount = currentTime / 1f;
        }
    }
    #endregion
}
