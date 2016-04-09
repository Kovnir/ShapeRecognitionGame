using UnityEngine;
using System;

public class FieldController : MonoBehaviour
{
    private const uint HEIGHT = 10;
    private uint width;

    //массив частиц на поле
    protected QubeBehaviour[,] columns;

    //"затухание" цвета
    [Range(0.1f, 20.0f)]
    [SerializeField]
    protected float attenuation = 3f;

    //закешированный трансформ
    protected Transform tr;

    void Awake()
    {
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
        columns = new QubeBehaviour[width, HEIGHT];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < HEIGHT; j++)
            {
                GameObject newObj = Instantiate(simpleParticle);
                columns[i, j] = newObj.GetComponent<QubeBehaviour>();
                columns[i, j].position = new Vector2(i, j);
                Vector3 newPosition = new Vector3(i - width / 2f+0.5f, 0, j - HEIGHT / 2f+ 0.5f);
                Debug.Log(newPosition.x + " " + newPosition.z);
                newObj.transform.position = newPosition;
                newObj.transform.SetParent(tr);
            }
        QubeBehaviour.waweSpeed = attenuation;
        QubeBehaviour.mouseEnter = OnMouseEnter;
        QubeBehaviour.mouseExit = OnMouseExit;
    }

    protected virtual void OnMouseEnter(int x, int y)
    {
        QubeBehaviour.isIdle = false;
        for (int i = 0; i < width; i++)
            for (int j = 0; j < HEIGHT; j++)
                columns[i, j].target = 1 / ((GetDistance(x, y, i, j) + 1));
        columns[x, y].target = 1;
    }
    protected virtual void OnMouseExit()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < HEIGHT; j++)
            {
                columns[i, j].target = 0;
            }
    }

    protected float GetDistance(int x1, int y1, int x2, int y2)
    {
        return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }

}
