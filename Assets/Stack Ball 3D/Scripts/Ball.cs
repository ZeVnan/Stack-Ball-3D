using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    #region Serialize Field
    [SerializeField]
    private AudioClip bounceOffClip, deadClip, winClip, destroyClip, iDestroyClip;
    [Range(0f, 1f)]
    [SerializeField]
    private float soundFXVolumeScale = 0.5f;
    [SerializeField]
    private GameObject invincibleObject;
    [SerializeField]
    private Image invincibleFill;
    [SerializeField]
    private GameObject fireEffect, winEffect, splashEffect;
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
        if (ballState == BallState.Playing)
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
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!smash)
        {
            rigidBody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            if (collision.gameObject.tag != "Finish")
            {
                GameObject splash = Instantiate(splashEffect);
                splash.transform.SetParent(collision.transform);
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
                float randomScale = Random.Range(0.18f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - 0.22f,
                    transform.position.z);
                splash.GetComponent<SpriteRenderer>().color =
                    transform
                    .GetChild(0)
                    .GetComponent<MeshRenderer>()
                    .material
                    .color;
            }
            SoundManager.Instance.PlaySoundFX(bounceOffClip, soundFXVolumeScale);
        }
        else
        {
            if (invincible)
            {
                StackController temp = collision.transform.parent.GetComponent<StackController>();
                if (temp != null)
                {
                    temp.ShatterAllParts();
                }
            }
            else
            {
                if (collision.gameObject.tag == "enemy")
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
                if (collision.gameObject.tag == "plane")
                {
                    rigidBody.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    ballState = BallState.Died;
                    SoundManager.Instance.PlaySoundFX(deadClip, soundFXVolumeScale);
                }
            }
        }
        FindObjectOfType<GameUI>().FillLevelSlider(currentBrokenStacks / (float)totalStacks);
        if (collision.gameObject.tag == "Finish" && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.Instance.PlaySoundFX(winClip, soundFXVolumeScale);
            GameObject win = Instantiate(winEffect);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localPosition = Vector3.up * 1.5f;
            win.transform.eulerAngles = Vector3.zero;
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
