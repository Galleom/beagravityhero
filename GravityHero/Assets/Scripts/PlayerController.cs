using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //public float moveSpeed = 1;
    public float groundSpeed;
    public float airSpeed;
    //public float tilt;


    Rigidbody2D rb;
    private float baseGrav;
    private int gravitySide = 2;//moveX = 0, moveY = 0, 
    private Transform sprite;
    private Transform groundCheck;          // A position marking where to check if the player is grounded
    private Transform[] arrows;          // A position marking where to check if the player is grounded
    private Vector3 groundCheckDif;        
    private bool grounded = false;			// Whether or not the player is grounded.
    private bool moving = false;
    private bool onGravSelection = false;
    private float speedX = 0, speedY = 0;
    private LineRenderer line;

    public float groundCheckDistance = 0.18f;
    public float groundCheckSize = 1f;

    private Vector3 startPos;
    public float decelerateSpeedMultiplier = 0.5f;
    public float minMouseMove = 0.3f;
    public float arrowsDistance = 0.3f;
    public Camera camera;
    // Use this for initialization
    void Start()
    {
        moving = false;
        baseGrav = Physics2D.gravity.y;
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        sprite = transform.Find("Sprite");
        groundCheck = transform.Find("groundCheck");
        groundCheck.localPosition = new Vector2(0, -groundCheckDistance);
        arrows = new Transform[4];
        arrows[0] = transform.Find("Tri Down");
        arrows[1] = transform.Find("Tri Right");
        arrows[2] = transform.Find("Tri Up");
        arrows[3] = transform.Find("Tri Left");

        line.enabled = false;
    }

    public void setGravitySide(int side)
    {
        if ((gravitySide == side)||(gravitySide <= 0)) return;
        sprite.GetComponent<Rotator>().setSide(side);
        gravitySide = side;
        rb.velocity = new Vector2(0, 0);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject obj in objs)
        {
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        switch (side)
        {
            case 2: // Down
                Physics2D.gravity = new Vector2(0, baseGrav);
                groundCheck.localPosition = new Vector2(0, -groundCheckDistance);
                groundCheckDif = new Vector2(groundCheckSize, 0);
                break;
            case 4: // Right
                Physics2D.gravity = new Vector2(-baseGrav, 0);
                groundCheck.localPosition = new Vector2(groundCheckDistance, 0);
                groundCheckDif = new Vector2(0, groundCheckSize);
                break;
            case 6: // Up
                Physics2D.gravity = new Vector2(0, -baseGrav);
                groundCheck.localPosition = new Vector2(0, groundCheckDistance);
                groundCheckDif = new Vector2(groundCheckSize, 0);
                break;
            case 8: // Left
                Physics2D.gravity = new Vector2(baseGrav, 0);
                groundCheck.localPosition = new Vector2(-groundCheckDistance, 0);
                groundCheckDif = new Vector2(0, groundCheckSize);
                break;
        }
    }

    // Update is called once per frame
    //public Boundary boundary;
    
    public void selectGravitySide(int side)
    {
        line.enabled = false;
        onGravSelection = false;
        switch (side)
        {
            case 0:
                arrows[0].gameObject.SetActive(false);
                arrows[1].gameObject.SetActive(false);
                arrows[2].gameObject.SetActive(false);
                arrows[3].gameObject.SetActive(false);
                break;
            case 2:
                arrows[0].GetComponent<GravityTriangle>().setOut(arrowsDistance);
                arrows[1].gameObject.SetActive(false);
                arrows[2].gameObject.SetActive(false);
                arrows[3].gameObject.SetActive(false);
                break;
            case 4:
                arrows[0].gameObject.SetActive(false);
                arrows[1].GetComponent<GravityTriangle>().setOut(arrowsDistance);
                arrows[2].gameObject.SetActive(false);
                arrows[3].gameObject.SetActive(false);
                break;
            case 6:
                arrows[0].gameObject.SetActive(false);
                arrows[1].gameObject.SetActive(false);
                arrows[2].GetComponent<GravityTriangle>().setOut(arrowsDistance);
                arrows[3].gameObject.SetActive(false);
                break;
            case 8:
                arrows[0].gameObject.SetActive(false);
                arrows[1].gameObject.SetActive(false);
                arrows[2].gameObject.SetActive(false);
                arrows[3].GetComponent<GravityTriangle>().setOut(arrowsDistance);
                break;
        }
        if (side != 0)
            setGravitySide(side);
    }
    void OnMouseUp()
    {
        if (grounded && !onGravSelection)
        {
            onGravSelection = true;
            switch (gravitySide)
            {
                case 0:
                    arrows[0].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[1].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[2].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[3].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    break;
                case 2: // Down
                    arrows[1].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[2].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[3].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    break;
                case 4: // Right
                    arrows[0].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[2].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[3].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    break;
                case 6: // Up
                    arrows[0].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[1].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[3].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    break;
                case 8: // Left
                    arrows[0].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[1].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    arrows[2].GetComponent<GravityTriangle>().activate(arrowsDistance);
                    break;
            }
        } else if (onGravSelection)
        {
            selectGravitySide(0);
        }
    }

    void FixedUpdate()
    {
        speedX = rb.velocity.x;
        speedY = rb.velocity.y;

        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(startPos, Input.mousePosition) >= minMouseMove)
            {
                line.SetPosition(0, camera.ScreenToWorldPoint(startPos) + Vector3.forward);
                line.SetPosition(1, camera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward);
                line.enabled = true;
                if ((gravitySide == 2) || (gravitySide == 6))
                {
                    speedX = Mathf.Cos(Vector2.Angle(Vector2.right, Input.mousePosition - startPos) * Mathf.Deg2Rad) * (grounded ? groundSpeed : airSpeed) * Time.deltaTime;
                }
                else
                {
                    speedY = (Input.mousePosition.y > startPos.y ? 1 : -1) * Mathf.Sin(Vector2.Angle(Vector2.right, Input.mousePosition - startPos) * Mathf.Deg2Rad) * (grounded ? groundSpeed : airSpeed) * Time.deltaTime;
                }
            }
            else
            {
                line.enabled = false;
            }
        }
        if (!moving)
        {
            if ((gravitySide == 2) || (gravitySide == 6))
            {
                speedX *= decelerateSpeedMultiplier;
            }
            else
            {
                speedY *= decelerateSpeedMultiplier;
            }
        }
        rb.velocity = new Vector2(speedX, speedY);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            line.SetPosition(0, camera.ScreenToWorldPoint(startPos) + Vector3.forward);
            line.SetPosition(1, camera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward);
            moving = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            line.enabled = false;
            moving = false;
        }
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(groundCheck.position + groundCheckDif, groundCheck.position - groundCheckDif, 1 << LayerMask.NameToLayer("Ground"));
        //Debug.DrawLine(groundCheck.position + groundCheckDif, groundCheck.position - groundCheckDif);
        /*if (grounded)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                setGravitySide(2);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                setGravitySide(4);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                setGravitySide(6);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                setGravitySide(8);
            }
        }*/
    }
}
