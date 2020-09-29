using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueBeadControl : MonoBehaviour
{
    public delegate void BlueDelegate(bool _isOut);
    public static event BlueDelegate BlueScore;

    public static string tagUntagged = "Untagged";
    public static string tagBlueBead = "BlueBead";
   
    [SerializeField]
    private Vector3 blueBeadPos;

    [SerializeField]
    private GameObject beadTarget;

    [SerializeField]
    private Rigidbody rb;

    bool blueVacuum;
    bool _isOut;
    bool isDone;

    public float step;
    public float _delay;

    public List<Material> matList = new List<Material>();

    GameManager b_Game;

    private void Start()
    {

        b_Game = GameManager.instance;
        beadTarget = GameObject.Find("Vacuum/Target").gameObject;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        if (b_Game.pressedButton) return;

        if ( blueVacuum && FindObjectOfType<TouchController>().isTouched)
        {
            if (gameObject.CompareTag("BlueBead"))
            {
                rb.useGravity = false;
                transform.SetParent(beadTarget.transform.parent);
                transform.position = Vector3.Lerp(transform.position, beadTarget.transform.position, step);
            }
        }
        else
        {
            rb.useGravity = true;
            transform.parent = null;
        }                  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BlueVacuum"))
        {
            blueVacuum = true;

        }          

        if (other.gameObject.CompareTag("RedVacuum"))
            blueVacuum = false;

        if (other.gameObject.CompareTag("BlueGround"))
        {
            isDone = true;
            _isOut = false;

            BlueScore(_isOut);
           // Handheld.Vibrate();

            if (gameObject.CompareTag("BlueBead"))
                gameObject.tag = tagUntagged;
        }

        if (other.gameObject.CompareTag("RbCollider"))
        {
            rb.velocity = Vector3.zero;

            if (gameObject.CompareTag("Untagged"))
            {
                gameObject.tag = tagBlueBead;
            }
        }
     
    }

    private void OnTriggerStay(Collider other)
    {
        //Change bead material in same color ground.
        if (other.gameObject.CompareTag("BlueGround"))
        {
            if (FindObjectOfType<TouchController>().isTouched && blueVacuum)
            {
              gameObject.GetComponent<Renderer>().material = matList[1];

            }
            else
            {
                gameObject.GetComponent<Renderer>().material = matList[0];

            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BlueVacuum"))
            blueVacuum = false;

        if(other.gameObject.CompareTag("BlueGround"))
        {
            _isOut = true;
            if (gameObject.CompareTag("Untagged"))
            {
                gameObject.tag = tagBlueBead;
            }
 
            BlueScore(_isOut);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OutGround"))
        {
            isDone = false;
    
            if (gameObject.CompareTag("BlueBead"))
                gameObject.tag = tagUntagged;

            StartCoroutine(OuttoInDelay());
        }

        if (collision.gameObject.CompareTag("InGround")&&!isDone)
        {
                      
            if (gameObject.CompareTag("Untagged"))
                gameObject.tag = tagBlueBead;
        } 
    }

    IEnumerator OuttoInDelay()
    {
        yield return new WaitForSeconds(2f);

        if(transform.position.y<0)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0.6f, 0), _delay * Time.deltaTime);
        }

    }
}
