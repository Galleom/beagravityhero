using UnityEngine;
using System.Collections;

public class ButtonPressAny : MonoBehaviour {
    public GameObject portal;
    private int number;
	// Use this for initialization
	void Start () {
        number = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Object")
        {
            number += 1;
            portal.GetComponent<Portal>().setActive(true);
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
            number = 0;
            portal.GetComponent<Portal>().setActive(false);
        }
    }
}
