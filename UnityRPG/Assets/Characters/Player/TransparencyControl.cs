using UnityEngine;
using System.Collections;

public class TransparencyControl : MonoBehaviour
{
    private const float TRANSPARENT = 0f;
    private const float HYSTERESIS = 0f;
    private const float INVERSE = 0f - HYSTERESIS;
    private const float MIN_DELTA = 0f;

    private float regular, transparent;
    private float alpha;
    private Color baseColor;
    private bool isNormal;

    // Use this for initialization
    void Start()
    {
        baseColor = transform.GetComponent<Renderer>().material.GetColor("_Color");
        regular = baseColor.a;

        alpha = 1.0f;
        isNormal = true;
    }

    void OnTriggerEnter(Collider other)
    {
        print("Trigger entered");
        if (other != gameObject && isNormal)
        {
            isNormal = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other != gameObject && !isNormal)
        {
            isNormal = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isNormal)
            alpha = HYSTERESIS * alpha + INVERSE;
        else
            alpha = HYSTERESIS * alpha;

        float trans = alpha * regular + (1.0f - alpha) * TRANSPARENT;
        transform.GetComponent<Renderer>().material.SetColor("_Color",
                new Color(baseColor.r, baseColor.g, baseColor.b, trans));
    }
}