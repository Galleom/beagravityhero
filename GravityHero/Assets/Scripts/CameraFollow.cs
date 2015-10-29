using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform followedObject;
    public float leftLimit;
    public float rightLimit;
    public float upLimit;
    public float downLimit;
    private Vector3 myPos;
    void Start()
    {
        if (followedObject == null)
            followedObject = gameObject.transform;
        myPos = new Vector3(0, 0, transform.position.z);
    }
    public void setLimits(float leftLimit, float rightLimit, float upLimit, float downLimit)
    {
        this.leftLimit = leftLimit;
        this.rightLimit = rightLimit;
        this.upLimit = upLimit;
        this.downLimit = downLimit;
    }
    public void setFollow(Transform followed)
    {
        followedObject = followed;
    }
    // Update is called once per frame
    void Update ()
    {
        myPos.x = Mathf.Clamp(followedObject.transform.position.x, leftLimit, rightLimit);
        myPos.y = Mathf.Clamp(followedObject.transform.position.y, downLimit, upLimit);
        transform.position = myPos;
    }
}
