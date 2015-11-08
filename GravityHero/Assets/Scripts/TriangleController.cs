using UnityEngine;
using System.Collections;

public class TriangleController : MonoBehaviour {
    private PlayerController pc;
	// Use this for initialization
	void Start () {
        pc = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void OnMouseUpAsButton()
    {
        pc.gravSelectionOn();
    }
}
