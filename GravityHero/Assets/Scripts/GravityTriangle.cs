using UnityEngine;
using System.Collections;

public class GravityTriangle : MonoBehaviour {
    public int side; // Down = 2; Right = 4; Up = 6; Left = 8;

    private ArrayList transparency;
    private ArrayList move_x;
    private ArrayList move_y;
    private bool selected = false;
    private SpriteRenderer sr;
    private GameObject player;
    // Use this for initialization
    void Awake()
    {
        selected = false;
        transparency = new ArrayList();
        move_x = new ArrayList();
        move_y = new ArrayList();
        sr = GetComponent<SpriteRenderer>();
        sr.material.color = new Color(1.0f, 1.0f, 1.0f, 0);
        player = transform.parent.gameObject;
        transform.gameObject.SetActive(false);
    }

    public void setActive(bool active)
    {
        transform.gameObject.SetActive(false);
        selected = false;
    }

    public void setOut(float dist)
    {
        selected = false;
        setTransparency(true);
        switch (side)
        {
            case 2:
                setY(-dist*2);
                break;
            case 4:
                setX(dist*2);
                break;
            case 6:
                setY(dist*2);
                break;
            case 8:
                setX(-dist*2);
                break;
        }
        StartCoroutine(waitAndSleep());
    }
    public void activate(float dist)
    {
        resetPosition();
        selected = true;
        transform.gameObject.SetActive(true);
        setTransparency(false);
        switch (side)
        {
            case 2:
                setY(-dist);
                break;
            case 4:
                setX(dist);
                break;
            case 6:
                setY(dist);
                break;
            case 8:
                setX(-dist);
                break;
        }
    }
    private IEnumerator waitAndSleep()
    {
        //yield return new WaitForSeconds(5);
        while (((move_x.Count > 0)|| (move_y.Count > 0)|| (transparency.Count > 0)))
        {
            yield return null;
        }
        if (!selected)
        {
            setActive(false);
        }
    }
    public void resetPosition()
    {
        transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
    }

    public void setTransparency(bool transparent)
    {
        if (transparent)
        {
            transparency.Insert(0, 0.9f);
            transparency.Insert(0, 0.7f);
            transparency.Insert(0, 0.5f);
            transparency.Insert(0, 0.4f);
            transparency.Insert(0, 0.3f);
            transparency.Insert(0, 0.2f);
            transparency.Insert(0, 0.1f);
            transparency.Insert(0, 0f);
        }
        else
        {
            transparency.Insert(0, 0.05f);
            transparency.Insert(0, 0.15f);
            transparency.Insert(0, 0.25f);
            transparency.Insert(0, 0.5f);
            transparency.Insert(0, 0.65f);
            transparency.Insert(0, 0.75f);
            transparency.Insert(0, 0.85f);
            transparency.Insert(0, 0.9f);
            transparency.Insert(0, 0.95f);
            transparency.Insert(0, 1f);
        }
    }
    public void setX(float x)
    {
        float old_x = x;
        float cur_x = transform.localPosition.x;
        move_x = new ArrayList();
        bool f = false;

        move_x.Add((float)(cur_x + x) / 2);
        while (!f)
        {
            if (Mathf.Abs((old_x - (float)move_x[move_x.Count - 1])) < 0.05f)
            {
                move_x.Add(x);
                f = true;
            }
            else
            {
                move_x.Insert(0, ((float)move_x[0] + cur_x) / 2);
                move_x.Add((x + (float)move_x[move_x.Count - 1]) / 2);
            }
        }
        move_x.Reverse();
    }
    public void setY(float y)
    {
        float old_y = y;
        float cur_y = transform.localPosition.y;
        move_y = new ArrayList();
        bool f = false;
        move_y.Add((float)(cur_y + y) / 2);
        while (!f)
        {
            if (Mathf.Abs((old_y - (float)move_y[move_y.Count - 1])) < 0.05f)
            {
                move_y.Add(y);
                f = true;
            }
            else
            {
                move_y.Insert(0, ((float)move_y[0] + cur_y) / 2);
                move_y.Add((y + (float)move_y[move_y.Count - 1]) / 2);
            }
        }
        move_y.Reverse();
    }
    void updatePos()
    {
        if (move_x.Count > 0)
        {
            transform.localPosition = new Vector3((float)move_x[move_x.Count - 1], transform.localPosition.y, transform.localPosition.z);
            move_x.RemoveAt(move_x.Count - 1);
        }
        if (move_y.Count > 0)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, (float)move_y[move_y.Count - 1], transform.localPosition.z);
            move_y.RemoveAt(move_y.Count - 1);
        }
        if (transparency.Count > 0)
        {
            sr.material.color = new Color(1.0f, 1.0f, 1.0f, (float)transparency[transparency.Count -1]);
            transparency.RemoveAt(transparency.Count - 1);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        updatePos();
    }

    void OnMouseDown()
    {
        if ((move_x.Count == 0) && (move_y.Count == 0) && (transparency.Count == 0))
        {
            player.GetComponent<PlayerController>().selectGravitySide(side);
        }
    }
}
