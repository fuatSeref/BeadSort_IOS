
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchController : MonoBehaviour
{ 
    public Vector3 offset;

    public float speed = 1f;
    public bool isTouched;

    float _x;
    float _z;

    Touch touch;
    GameManager game;

    private void Start()
    {
        game = GameManager.instance;
    }
    private void Update()
    {
       
        
        if (game.isLevelOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
                
        }
        if (game.isLevelOver || !game.isStartTap) return;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isTouched = true;

            }
            if (touch.phase == TouchPhase.Stationary)
            {
                isTouched = true;              
            }
            if (touch.phase == TouchPhase.Moved)
            {
                isTouched = true;

                _x = transform.position.x + touch.deltaPosition.x * speed * Time.deltaTime;
                _z = transform.position.z + touch.deltaPosition.y * speed * Time.deltaTime;

                if(_x <= -0.5f)
                {
                    _x = -0.5f;
                }
                if(_x >= 0.5f)
                {
                    _x = 0.5f;
                }
                if (_z <= -1.2f)
                {
                    _z = -1.2f;
                }
                if (_z >= 1.5f)
                {
                    _z = 1.5f;
                }
                // float x = Mathf.Clamp(_x, -0.5f, 0.5f);
                // float z = Mathf.Clamp(_z, 1.5f, -1.2f);
                transform.position = new Vector3(_x,transform.position.y, _z);

            }

            if (touch.phase == TouchPhase.Ended)
            {
                isTouched = false;

                
            }
        }
    }

   

}

 
