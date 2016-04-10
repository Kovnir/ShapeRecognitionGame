using UnityEngine;
using System.Collections;
using System;

public class QubeBehaviour : MonoBehaviour
{
    public bool selected = false; 

//    public Vector2 position;
    protected Transform column;

    public static Color idleColor;
    public static Color activeColor;
    public static float attenuation;
    
    public static bool isIdle = false;
    
    private Color currentColor;

    Renderer rend;

    void Awake()
    {
        column = transform.GetChild(0);
    }

    void Start()
    {
        rend = column.GetComponent<Renderer>();
     //   rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_Color", idleColor);
        currentColor = idleColor;
    }

    void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0))
        {
            if (FieldController.Instance.isMouseDowned)
                FieldController.Instance.OnMouseExit();
            return;
        }
        rend.material.SetColor("_Color", activeColor);
        currentColor = activeColor;
        selected = true;
    }

    void OnMouseDown()
    {
        OnMouseEnter();
        FieldController.Instance.OnMouseDown();
    }

    void OnMouseUp()
    {
        FieldController.Instance.OnMouseExit();
    } 


    private void Update()
    {
        currentColor = Color.Lerp(currentColor, idleColor, Mathf.PingPong(Time.time, 1)* attenuation);
        rend.material.SetColor("_Color", currentColor);
    }
}
