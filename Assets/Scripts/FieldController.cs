using UnityEngine;
using System;

public class FieldController : MonoBehaviour
{
    [SerializeField]
    private const uint HEIGHT = 10;
    private uint width;
    
    //массив частиц на поле
    protected QubeBehaviour[,] columns;

    public bool isMouseDowned = false;

    //"затухание" цвета
    [Range(0f, 2.0f)]
    [SerializeField]
    protected float attenuation = 0.5f;

    public Color idle;
    public Color active;

    //закешированный трансформ
    protected Transform tr;

    private static FieldController instance;
    public static FieldController Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        tr = transform;
    }

    protected virtual void Start()
    {
        width = (uint) (Camera.main.pixelWidth / (Camera.main.pixelHeight / (Camera.main.orthographicSize*2)))+1;
        GameObject simpleParticle = (GameObject)Resources.Load("Prefubs/SimpleParticle");
        if (simpleParticle == null)
        {
            Debug.LogError("SimpleParticle is not found!");
            return;
        }
        columns = new QubeBehaviour[HEIGHT, width];
        for (int i = 0; i < HEIGHT; i++)
            for (int j = 0; j < width; j++)
            {
                GameObject newObj = Instantiate(simpleParticle);
                columns[i, j] = newObj.GetComponent<QubeBehaviour>();
              //  columns[i, j].position = new Vector2(i, j);
                Vector3 newPosition = new Vector3(j - width / 2f+0.5f, 0, i - HEIGHT / 2f+ 0.5f);
                newObj.transform.position = newPosition;
                newObj.transform.SetParent(tr);
            }

        QubeBehaviour.idleColor = idle;
        QubeBehaviour.activeColor = active;

        QubeBehaviour.attenuation = attenuation;

    }


    public virtual void OnMouseDown()
    {
        isMouseDowned = true;
    }
    public virtual void OnMouseExit()
    {
        isMouseDowned = false;

        bool[,] fullResult = new bool[HEIGHT, width];   //вся карта


        uint minX = HEIGHT; //надо будет запомнить
        uint minY = width; //надо будет запомнить
        uint maxX = 0; //надо будет запомнить
        uint maxY = 0; //надо будет запомнить

        for (uint x = 0; x < HEIGHT; x++)
            for (uint y = 0; y < width; y++)
            {
                fullResult[x, y] = columns[x, y].selected;
                if (fullResult[x, y])
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        bool[,] partialResult = new bool[maxX - minX + 1, maxY - minY + 1];   //полезная часть карты
        for (uint x = minX; x < maxX + 1; x++)
            for (uint y = minY; y < maxY + 1; y++)
            {
                partialResult[x - minX, y - minY] = fullResult[x, y];
            }
        GameController.Instance.Comparate(partialResult);
        for (int i = 0; i < HEIGHT; i++)
            for (int j = 0; j < width; j++)
            {
                columns[i, j].selected = false;
            }
    }


    protected float GetDistance(int x1, int y1, int x2, int y2)
    {
        return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }

}
