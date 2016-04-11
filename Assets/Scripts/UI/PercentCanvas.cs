using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PercentCanvas : MonoBehaviour {

    private static PercentCanvas instance;
    [SerializeField]
    private Text text;
    private Animator animator;

    [SerializeField]
    private Color goodColor;
    [SerializeField]
    private Color perfectColor;
    [SerializeField]
    private Color badColor;


    void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    public static void ShowGood(int percents)
    {
        instance.text.text = "Good!\n" + percents + "%";
        instance.text.color = instance.goodColor;
        instance.animator.SetTrigger("ShowGood");
    }
    public static void ShowPerfect()
    {
        instance.text.text = "Perfect!\n100%";
        instance.text.color = instance.perfectColor;
        instance.animator.SetTrigger("ShowPerfect");
    }
    public static void ShowBad(int percents)
    {
        instance.text.text = "Bad!\n" + percents + "%";
        instance.text.color = instance.badColor;
        instance.animator.SetTrigger("ShowBad");
    }

}
