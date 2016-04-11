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
    public static Color correctColor;
    public static Color incorrectColor;
    public static Color checkingColor;
    public static Color missingColor;

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
        rend.material.SetColor("_Color", idleColor);
        currentColor = idleColor;
    }

    void OnMouseEnter()
    {
        if (!FieldController.Instance.turnAllowed) return;
        if (!Input.GetMouseButton(0))
        {
                FieldController.Instance.OnMouseExit();
            return;
        }
        SoundPattern.PlayBubbleSound();
        rend.material.SetColor("_Color", activeColor);
        currentColor = activeColor;
        selected = true;
    }

    void OnMouseDown()
    {
        if (!FieldController.Instance.turnAllowed) return;

        OnMouseEnter();
        FieldController.Instance.OnMouseDown();
    }
    

    void OnMouseUp()
    {
        if (!FieldController.Instance.turnAllowed) return;

        FieldController.Instance.OnMouseExit();
    }

    public void SetCorrectColor()
    {
        currentColor = correctColor;
    }
    public void SetIncorrectColor()
    {
        currentColor = incorrectColor;
    }
    public void SetMissingColor()
    {
        currentColor = missingColor;
    }
    internal void SetCheckingColor()
    {
        currentColor = checkingColor;
    }

    private void Update()
    {
        currentColor = Color.Lerp(currentColor, idleColor, Mathf.PingPong(Time.time, 1)* attenuation);
        rend.material.SetColor("_Color", currentColor);
    }

}
