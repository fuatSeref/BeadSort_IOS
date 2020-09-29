using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public ParticleSystem confetti;

    public GameObject startPage;
    public GameObject blueCollider;
    public GameObject redCollider;
    public GameObject levelOverPage;
    public GameObject buttons;
    public GameObject vacuum;

    public float vacuumSpeed;

    public bool pressedButton;
    public bool isStartTap;
    public bool isLevelOver;


    public Material[] m_Material;
    public Renderer vacuumRend;
    public Renderer theRingRend;

    enum PageState
    {
        Initialize,
        Started,
        RedCollider,
        BlueCollider,
        LevelOverPage
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {

        OnGameStarted();

    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isStartTap)
        {
            SetPageState(PageState.Started);
            isStartTap = true;
        }

        if(FindObjectOfType<ScoreManager>().isHundread && !isLevelOver)
        {
            isLevelOver = true;
            OnLevelOver();

        }       

    }
    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.Initialize:
                buttons.SetActive(false);
                startPage.SetActive(true);
                redCollider.SetActive(false);
                blueCollider.SetActive(false);
                levelOverPage.SetActive(false);
                vacuumRend.sharedMaterial = m_Material[0];
                theRingRend.sharedMaterial = m_Material[0];
                break;

            case PageState.Started:
                buttons.SetActive(true);
                startPage.SetActive(false);
                redCollider.SetActive(false);
                blueCollider.SetActive(true);
                vacuumRend.sharedMaterial = m_Material[0];
                theRingRend.sharedMaterial = m_Material[0];
                break;

            case PageState.RedCollider:
                startPage.SetActive(false);
                redCollider.SetActive(true);
                blueCollider.SetActive(false);
                vacuumRend.sharedMaterial = m_Material[1];
                theRingRend.sharedMaterial = m_Material[1];
                break;

            case PageState.BlueCollider:
                startPage.SetActive(false);
                redCollider.SetActive(false);
                blueCollider.SetActive(true);
                vacuumRend.sharedMaterial = m_Material[0];
                theRingRend.sharedMaterial = m_Material[0];
                break;

            case PageState.LevelOverPage:
                buttons.SetActive(false);
                startPage.SetActive(false);
                redCollider.SetActive(false);
                blueCollider.SetActive(false);
                levelOverPage.SetActive(true);
                break;

        }
    }
    //when game initialize
    void OnGameStarted()
    {

        vacuum.GetComponent<Transform>().DOLocalMoveY(0.51f, vacuumSpeed);
        isLevelOver = false;
        SetPageState(PageState.Initialize);
        levelOverPage.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
    }
    public void RedButton()
    {
        if (isLevelOver) return;
        SetPageState(PageState.RedCollider);
 
    }

    public void BlueButton()
    {
        if (isLevelOver) return;
        SetPageState(PageState.BlueCollider);

    }
    
    //is not Used; to prevent bead scattering on Awake/Start
    public void FreezePosition(Rigidbody _rb, bool _isGround)
    {
        _rb.constraints = RigidbodyConstraints.FreezePositionX;
        _rb.constraints = RigidbodyConstraints.FreezePositionZ;

        if (_isGround)
            _rb.constraints = RigidbodyConstraints.None;
    }

    public void OnLevelOver()
    {
        confetti.Play();
        StartCoroutine(OverPageDelay());                
    }
   
    IEnumerator OverPageDelay()
    {      
        yield return new WaitForSeconds(1.2f);
 
        SetPageState(PageState.LevelOverPage);
        levelOverPage.GetComponent<CanvasGroup>().DOFade(1, 1.2f);
    }


}
