using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBeadControl : MonoBehaviour
{
    public delegate void RedDelegate(bool _IsOut);
    public static event RedDelegate RedScore;

    public static string tagUntagged = "Untagged";
    public static string tagRedBead = "RedBead";

    [SerializeField]
    private Vector3 redBeadPos;

    [SerializeField]
    private GameObject beadTarget;

    [SerializeField]
    private Rigidbody rb;

    bool redVacuum;
    bool isDone;
    bool _IsOut;

    public float step;
    public float _delay;

    public List<Material> matList = new List<Material>();

    GameManager r_Game;

    private void Start()
    {
        r_Game = GameManager.instance;
        rb = GetComponent<Rigidbody>();
        beadTarget = GameObject.Find("Vacuum/Target").gameObject;
    }
    private void FixedUpdate()
    {
        if (r_Game.pressedButton) return;

        if (redVacuum && FindObjectOfType<TouchController>().isTouched)
        {
            if (gameObject.CompareTag("RedBead"))
            {
                transform.position = Vector3.Lerp(transform.position, beadTarget.transform.position, step);
                rb.useGravity = false;
                transform.SetParent(beadTarget.transform.parent);
            }
                                                                           
        }else
        {
            rb.useGravity = true;
            transform.parent = null;
        }
        
           
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("RedVacuum"))
        {
            
            redVacuum = true;
        }
            

        if (other.gameObject.CompareTag("BlueVacuum"))
        {
            
            redVacuum = false;
        }
           

        if (other.gameObject.CompareTag("RedGround"))
        {
            _IsOut = false;
            RedScore(_IsOut);
            isDone = true;

            //Handheld.Vibrate();

            if (gameObject.CompareTag("RedBead"))
            {
                gameObject.tag = tagUntagged;
            }
        }

        if (other.gameObject.CompareTag("RbCollider"))
        {
            rb.velocity = Vector3.zero;

            if (gameObject.CompareTag("Untagged"))
                gameObject.tag = tagRedBead;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("RedGround"))
        {
            if (FindObjectOfType<TouchController>().isTouched && redVacuum)
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
        if (other.gameObject.CompareTag("RedVacuum"))
            redVacuum = false;

        if (other.gameObject.CompareTag("RedGround"))
        {
            _IsOut = true;
            RedScore(_IsOut);


            if (gameObject.CompareTag("Untagged"))
            {
                gameObject.tag = tagRedBead;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("OutGround"))
        {
            isDone = false;
            if (gameObject.CompareTag("RedBead"))
            {
                gameObject.tag = tagUntagged;
            }
            StartCoroutine(OuttoInDelay());
        }

        if (collision.gameObject.CompareTag("InGround")&& !isDone)
        {
            
            if (gameObject.CompareTag("Untagged"))
                gameObject.tag = tagRedBead;
        }



    }
    IEnumerator OuttoInDelay()
    {
        yield return new WaitForSeconds(2f);

        if (transform.position.y < 0)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0.6f, 0), _delay * Time.deltaTime);
        }

    }


}
