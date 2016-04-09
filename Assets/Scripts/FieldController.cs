using UnityEngine;
using System;

public class FieldController : MonoBehaviour
{
    //Размеры поля
    [SerializeField]
    protected uint height = 20;
    [SerializeField]
    protected uint width = 36;

    //массив частиц на поле
    protected QubeBehaviour[,] columns;

    //"затухания", для гармоничности от него завсит так же и подъём
    [Range(0.1f, 20.0f)]
    [SerializeField]
    protected float attenuation = 3f;
    //"Длина шлейфа" вляет на выгнутость параболы
    [Range(0.55f, 1.0f)]
    [SerializeField]
    protected float lengthOfPlume = 1f;

    public float amplitude = 8;

    //закешированный трансформ
    protected Transform tr;

    //ожидаем ли мы начала анимации по умолчанию
    protected bool idleWait;
    //время через которое начнёмтся анимация по умолчанию, если юзер ничего не сделает
    protected float timeBeforeIdle;
    //через сколько времени бездействия наступит анимация по умолчанию
    protected const float TIME_FOR_IDLE = 0.5f;

    void Awake()
    {
        tr = transform;
    }

    protected virtual void Start()
    {
        GameObject simpleParticle = (GameObject)Resources.Load("Prefubs/SimpleParticle");
        if (simpleParticle == null)
        {
            Debug.LogError("SimpleParticle is not found!");
            return;
        }
        columns = new QubeBehaviour[width, height];
        if (width == 0 || height == 0) return;
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                GameObject newObj = Instantiate(simpleParticle);
                columns[i, j] = newObj.GetComponent<QubeBehaviour>();
                columns[i, j].position = new Vector2(i, j);
                newObj.transform.position = new Vector3(i - width / 2, 0, j - height / 2);
                newObj.transform.SetParent(tr);
            }
        QubeBehaviour.waweSpeed = attenuation;
        QubeBehaviour.amplitude = amplitude;
        QubeBehaviour.mouseEnter = OnMouseEnter;
        QubeBehaviour.mouseExit = OnMouseExit;
        idleWait = true;
    }

    protected virtual void OnMouseEnter(int x, int y)
    {
        idleWait = false;
        QubeBehaviour.isIdle = false;
        if (lengthOfPlume == 0) return;

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                columns[i, j].target = 1 / ((GetDistance(x, y, i, j) + 1) * lengthOfPlume);
        columns[x, y].target = 1;
    }
    protected virtual void OnMouseExit()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                columns[i, j].target = 0;
            }
        idleWait = true;
        timeBeforeIdle = TIME_FOR_IDLE;
    }

    protected float GetDistance(int x1, int y1, int x2, int y2)
    {
        return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }


    private void Update()
    {
        if (idleWait)
        {
            timeBeforeIdle -= Time.deltaTime;
            if (timeBeforeIdle <= 0)
            {
                idleWait = false;
                QubeBehaviour.isIdle = true;
                OnIdleStart();
            }
        }
    }

    //необходимо переопределить
    protected virtual void OnIdleStart()
    {
    }
}
