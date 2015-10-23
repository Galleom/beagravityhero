using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    public float activeTransparency = 1f;
    public float inactiveTransparency = 0.5f;
    public bool active = false;
    private SpriteRenderer sprite;
    private ParticleSystem pe;
    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        pe = GetComponent<ParticleSystem>();
        updateTransparency();
    }

    void updateTransparency() {
        sprite.color = new Color(1f, 1f, 1f, active ? activeTransparency : inactiveTransparency);
        pe.enableEmission = active;
    }

    public void setActive(bool newActive) {
        if (active != newActive)
        {
            active = newActive;
            updateTransparency();
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (active && other.tag == "Player")
        {
            print("Vitoria!");
            Destroy(other.gameObject);
        }
    }
}
