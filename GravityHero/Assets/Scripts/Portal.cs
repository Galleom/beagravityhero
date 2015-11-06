using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    public float activeTransparency = 1f;
    public float inactiveTransparency = 0.5f;
    public bool active = false;
    private SpriteRenderer sprite;
    private ParticleSystem pe;
    private ParticleSystem pe2;
    // Use this for initialization
    void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
        pe = GetComponent<ParticleSystem>();
        pe2 = transform.Find("Effect2").GetComponent<ParticleSystem>();
        updateTransparency();
    }

    void setparticles(bool visible)
    {

        pe.enableEmission = visible;
        pe2.enableEmission = visible;
    }

    void updateTransparency()
    {
        sprite.color = new Color(1f, 1f, 1f, active ? activeTransparency : inactiveTransparency);
        setparticles(active);
    }

    public void setActive(bool newActive) {
        if (active != newActive)
        {
            active = newActive;
            updateTransparency();
        }
    }

    void setVictory()
    {
        print("Vitoria!");
        Camera.main.GetComponent<CameraFollow>().setFollow(Camera.main.transform);
        GameplayControls.Instance.ShowVictory();
        GameplayControls.Instance.HidePlayer();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (active && other.tag == "Player")
        {
            setVictory();
            //Destroy(other.gameObject);
        }
    }
}
