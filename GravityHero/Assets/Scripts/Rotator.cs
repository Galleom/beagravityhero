using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
    public float rotationTime = 1;
    private int side = 2;

    public void setSide(int newSide)
    {
        if(newSide != side)
        {
            side = newSide;
            switch (newSide)
            {
                case 2: // Down
                    StartCoroutine(RotateMe(new Vector3(0, 0, 0), rotationTime));
                    break;
                case 4: // Right
                    StartCoroutine(RotateMe(new Vector3(0, 0,90), rotationTime));
                    break;
                case 6: // Up
                    StartCoroutine(RotateMe(new Vector3(0, 0,180), rotationTime));
                    break;
                case 8: // Left
                    StartCoroutine(RotateMe(new Vector3(0, 0,270), rotationTime));
                    break;
            }
        }
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        Quaternion fromAngle = transform.rotation;
        Quaternion toAngle = Quaternion.Euler(byAngles);
        for (float t = 0f; t < 1f; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
        transform.rotation = toAngle;
    }
 
	// Update is called once per frame
	void Update ()
    {
	
	}
}
