using UnityEngine;
using System.Collections;
using System;

public class QubeBehaviour : MonoBehaviour
{
    public Vector2 position;
    protected Transform column;

    public static Color idleColor;
    public static Color activeColor;
    public static float attenuation;

    public static Action<int, int> mouseEnter;
    public static Action mouseExit;
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
        rend.material.SetColor("_Color", activeColor);
        currentColor = activeColor;
        //mouseEnter((int)position.x, (int)position.y);
    } //вызывается в момент захода мыши на коллайдер объекта
    void OnMouseExit() { mouseExit(); } //вызывается в момент выхода мыши с коллайдера объекта

    private void Update()
    {
        currentColor = Color.Lerp(currentColor, idleColor, Mathf.PingPong(Time.time, 1)* attenuation);
        rend.material.SetColor("_Color", currentColor);
    }

    protected virtual float IdleUpdate(float speed)
    {
        return speed;
    }
    protected virtual void InteractiveUpdate(float speed)
    {
    }
}
