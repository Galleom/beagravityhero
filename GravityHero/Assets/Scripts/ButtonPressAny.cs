using UnityEngine;
using System.Collections;

public class ButtonPressAny : MonoBehaviour {
    public GameObject portal;
    private int number;
    private float size;
	// Use this for initialization
	void Start () {
        number = 0;
        size = transform.localScale.y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Object")
        {
            number += 1;
            portal.GetComponent<Portal>().setActive(true);
        }
        if (number > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, size / 2, transform.localScale.z);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Object")
        {
            number -= 1;
        }
        if (number <= 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, size, transform.localScale.z);
            number = 0;
            portal.GetComponent<Portal>().setActive(false);
        }
    }
}
