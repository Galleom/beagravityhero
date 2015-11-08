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
    private int facingSide = 0;
    private Transform sprite;
    private Animator animator;
    private Transform groundCheck;          // A position marking where to check if the player is grounded
    private GravityTriangle[] arrows;          // A position marking where to check if the player is grounded
    private Vector3 groundCheckDif;        
    private bool grounded = false;			// Whether or not the player is grounded.
    private bool moving = false;
    private bool onGravSelection = false;

    private LineRenderer line;
    private Vector2 speed;
    public float groundCheckDistance = 0.18f;
    public float groundCheckSize = 1f;

    private Vector3 startPos;
    private ParticleSystem ps;

    public float decelerateSpeedMultiplier = 0.5f;
    public float minMouseMove = 0.3f;
    public float minMouseDeleteArrows = 1f;
    public float arrowsDistance = 0.3f;
    public int maxSpeedMouseDistance;

    public int particlesOnChange = 4;
    // Use this for initialization
    void Start()
    {
        moving = false;
        baseGrav = Physics2D.gravity.y;
        rb = GetComponent<Rigidbody2D>();
        groundCheckDif = new Vector3(groundCheckSize, 0, 0);
        line = GetComponent<LineRenderer>();
        line.sortingLayerName = "UI";
        sprite = transform.Find("Sprite");
        animator = sprite.GetComponent<Animator>();
        animator.SetInteger("animation", 0);

        facingSide = 1;
        groundCheck = transform.Find("groundCheck");
        groundCheck.localPosition = new Vector2(0, -groundCheckDistance);
        arrows = new GravityTriangle[4];
        arrows[0] = transform.Find("Tri Down").GetComponent<GravityTriangle>();
        arrows[1] = transform.Find("Tri Right").GetComponent<GravityTriangle>();
        arrows[2] = transform.Find("Tri Up").GetComponent<GravityTriangle>();
        arrows[3] = transform.Find("Tri Left").GetComponent<GravityTriangle>();
        speed = new Vector2(0, 0);
        selectGravitySide(0);

        line.enabled = false;

        ps = transform.Find("Particles").GetComponent<ParticleSystem>();
    }

    public void setGravitySide(int side)
    {
        if ((gravitySide == side)||(gravitySide <= 0)) return;
        sprite.GetComponent<Rotator>().setSide(side);
        gravitySide = side;
        rb.velocity = new Vector2(0, 0);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Object");
        ps.Emit(particlesOnChange);
        foreach (GameObject obj in objs)
        {
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        switch (side)
        {
            case 2: // Down
                Physics2D.gravity = new Vector2(0, baseGrav);
                Physics.gravity = new Vector3(0, baseGrav, 0);
                groundCheck.localPosition = new Vector2(0, -groundCheckDistance);
                groundCheckDif = new Vector3(groundCheckSize, 0, 0);
                break;
            case 4: // Right
                Physics2D.gravity = new Vector2(-baseGrav, 0);
                Physics.gravity = new Vector3(-baseGrav, 0, 0);
                groundCheck.localPosition = new Vector2(groundCheckDistance, 0);
                groundCheckDif = new Vector3(0, groundCheckSize, 0);
                break;
            case 6: // Up
                Physics2D.gravity = new Vector2(0, -baseGrav);
                Physics.gravity = new Vector3(0, -baseGrav, 0);
                groundCheck.localPosition = new Vector2(0, groundCheckDistance);
                groundCheckDif = new Vector3(groundCheckSize, 0, 0);
                break;
            case 8: // Left
                Physics2D.gravity = new Vector2(baseGrav, 0);
                Physics.gravity = new Vector3(baseGrav, 0, 0);
                groundCheck.localPosition = new Vector2(-groundCheckDistance, 0);
                groundCheckDif = new Vector3(0, groundCheckSize, 0);
                break;
        }
    }
    public void resetGravity()
    {
        gravitySide = 2;
        Physics2D.gravity = new Vector2(0, baseGrav);
        Physics.gravity = new Vector3(0, baseGrav, 0);
    }

    public void selectGravitySide(int side)
    {
        line.enabled = false;
        onGravSelection = false;
        switch (side)
        {
            case 0:
                arrows[0].setActive(false);
                arrows[1].setActive(false);
                arrows[2].setActive(false);
                arrows[3].setActive(false);
                break;
            case 2:
                arrows[0].setOut(arrowsDistance);
                arrows[1].setActive(false);
                arrows[2].setActive(false);
                arrows[3].setActive(false);
                break;
            case 4:
                arrows[0].setActive(false);
                arrows[1].setOut(arrowsDistance);
                arrows[2].setActive(false);
                arrows[3].setActive(false);
                break;
            case 6:
                arrows[0].setActive(false);
                arrows[1].setActive(false);
                arrows[2].setOut(arrowsDistance);
                arrows[3].setActive(false);
                break;
            case 8:
                arrows[0].setActive(false);
                arrows[1].setActive(false);
                arrows[2].setActive(false);
                arrows[3].setOut(arrowsDistance);
                break;
        }
        if (side != 0)
            setGravitySide(side);
    }

    public void gravSelectionOn()
    {
        if (grounded && !onGravSelection)
        {
            onGravSelection = true;
            switch (gravitySide)
            {
                case 0:
                    arrows[0].activate(arrowsDistance);
                    arrows[1].activate(arrowsDistance);
                    arrows[2].activate(arrowsDistance);
                    arrows[3].activate(arrowsDistance);
                    break;
                case 2: // Down
                    arrows[1].activate(arrowsDistance);
                    arrows[2].activate(arrowsDistance);
                    arrows[3].activate(arrowsDistance);
                    break;
                case 4: // Right
                    arrows[0].activate(arrowsDistance);
                    arrows[2].activate(arrowsDistance);
                    arrows[3].activate(arrowsDistance);
                    break;
                case 6: // Up
                    arrows[0].activate(arrowsDistance);
                    arrows[1].activate(arrowsDistance);
                    arrows[3].activate(arrowsDistance);
                    break;
                case 8: // Left
                    arrows[0].activate(arrowsDistance);
                    arrows[1].activate(arrowsDistance);
                    arrows[2].activate(arrowsDistance);
                    break;
            }
        }
        else if (onGravSelection)
        {
            selectGravitySide(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            moving = false;
            line.SetPosition(0, Camera.main.ScreenToWorldPoint(startPos + Vector3.forward*transform.position.z));
            line.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * transform.position.z));
        }
        if (Input.GetMouseButtonUp(0))
        {
            line.enabled = false;
            moving = false;
            animator.SetInteger("animation", grounded ? 0 : 2);
        }
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.

        grounded = Physics2D.Linecast(groundCheck.position + groundCheckDif, groundCheck.position - groundCheckDif, 1 << LayerMask.NameToLayer("Ground"));
        if (grounded)
        {
            animator.SetInteger("animation", 0);
        }

        speed.x = rb.velocity.x;
        speed.y = rb.velocity.y;

        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(startPos, Input.mousePosition) >= minMouseMove)
            {
                line.SetPosition(0, Camera.main.ScreenToWorldPoint(startPos + Vector3.forward * (transform.position.z - Camera.main.transform.position.z)));
                line.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * (transform.position.z - Camera.main.transform.position.z)));
                line.enabled = true;
                if (onGravSelection)
                {
                    if (Vector2.Distance(startPos, Input.mousePosition) >= minMouseDeleteArrows)
                    {
                        selectGravitySide(0);
                    }
                }
                if ((gravitySide == 2) || (gravitySide == 6))
                {
                    speed.x = Mathf.Cos(Vector2.Angle(Vector2.right, Input.mousePosition - startPos) * Mathf.Deg2Rad) * (grounded ? groundSpeed : airSpeed) * Time.deltaTime;
                    if (Vector2.Distance(startPos, Input.mousePosition) < maxSpeedMouseDistance)
                    {
                        animator.speed = (Vector2.Distance(startPos, Input.mousePosition) / (float)maxSpeedMouseDistance);
                        speed.x *= (Vector2.Distance(startPos, Input.mousePosition) / (float)maxSpeedMouseDistance);
                    }
                    else
                    {
                        animator.speed = 1;
                    }
                }
                else
                {
                    speed.y = (Input.mousePosition.y > startPos.y ? 1 : -1) * Mathf.Sin(Vector2.Angle(Vector2.right, Input.mousePosition - startPos) * Mathf.Deg2Rad) * (grounded ? groundSpeed : airSpeed) * Time.deltaTime;
                    if (Vector2.Distance(startPos, Input.mousePosition) < maxSpeedMouseDistance)
                    {
                        speed.y *= (Vector2.Distance(startPos, Input.mousePosition) / (float)maxSpeedMouseDistance);
                    }
                    else
                    {
                        animator.speed = 1;
                    }
                }

                switch (gravitySide)
                {
                    case 2: // Down
                        animator.SetInteger("animation", grounded ? (speed.x == 0 ? 0 : 1) : 2);
                        if (facingSide != (speed.x > 0 ? 1 : -1))
                        {
                            facingSide = (speed.x > 0 ? 1 : -1);
                            sprite.localScale = new Vector3(-sprite.localScale.x, sprite.localScale.y, sprite.localScale.z);
                        }
                        break;
                    case 4: // Right
                        animator.SetInteger("animation", grounded ? (speed.y == 0 ? 0 : 1) : 2);
                        if (facingSide != (speed.y > 0 ? 1 : -1))
                        {
                            facingSide = (speed.y > 0 ? 1 : -1);
                            sprite.localScale = new Vector3(-sprite.localScale.x, sprite.localScale.y, sprite.localScale.z);
                        }
                        break;
                    case 6: // Up
                        animator.SetInteger("animation", grounded ? (speed.x == 0 ? 0 : 1) : 2);
                        if (facingSide != (speed.x < 0 ? 1 : -1))
                        {
                            facingSide = (speed.x < 0 ? 1 : -1);
                            sprite.localScale = new Vector3(-sprite.localScale.x, sprite.localScale.y, sprite.localScale.z);
                        }
                        break;
                    case 8: // Left
                        animator.SetInteger("animation", grounded ? (speed.y == 0 ? 0 : 1) : 2);
                        if (facingSide != (speed.y < 0 ? 1 : -1))
                        {
                            facingSide = (speed.y < 0 ? 1 : -1);
                            sprite.localScale = new Vector3(-sprite.localScale.x, sprite.localScale.y, sprite.localScale.z);
                            //sprite.localPosition = new Vector3(-sprite.localPosition.x, sprite.localPosition.y, sprite.localPosition.z);
                        }
                        break;
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
                speed.x *= decelerateSpeedMultiplier;
            }
            else
            {
                speed.y *= decelerateSpeedMultiplier;
            }
        }
        rb.velocity = speed;

    }
}
