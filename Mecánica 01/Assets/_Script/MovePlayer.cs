using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float turnSpeed;
    public float moveSpeed;
    public float speed;

    public bool canJump;
    public float jumpForce;

    public Transform _InitialPos;

    public GameObject[] plataforms;
    public bool IsInGround;

    [SerializeField] GameManager gameManager;

    public bool IsKey;
    public bool OpenDoor;

    Animator anim;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        
        if(Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetBool("IsRunning", true);
        }
        if(!Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetBool("IsRunning", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Danger") && gameManager.lifes > 0)
        {
            transform.position = _InitialPos.position;
            gameManager.lifes -= 1;
        }

        if (other.CompareTag("PowerUp"))
        {
            canJump = true;

            plataforms[0].GetComponent<Rigidbody>().useGravity = true;
            plataforms[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            Destroy(other.gameObject);
        }

        if(other.CompareTag("Key"))
        {
            gameManager.IsKey = true;
        }
        if (other.CompareTag("Ground")) IsInGround = true;

        if (other.CompareTag("Key"))
        {
            gameManager.IsKey = true;
            Destroy(other.gameObject);
        }

        if (IsKey == true)
        {
            anim.SetBool("IsKey", true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground")) IsInGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) IsInGround = false;
    }

    public void Move()
    {
        
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        transform.Translate(0, 0, moveVertical * speed * Time.deltaTime);
        transform.Rotate(0, moveHorizontal, 0 * turnSpeed * Time.deltaTime);

        if (canJump && IsInGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}