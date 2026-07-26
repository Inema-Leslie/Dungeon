

using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    public static ArrowPool Instance { get; private set; }

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private int poolSize = 15;

    private Queue<Arrow> pool = new Queue<Arrow>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewArrow();
        }
    }

    private void CreateNewArrow()
    {
        GameObject obj = Instantiate(arrowPrefab, transform);
        Arrow arrow = obj.GetComponent<Arrow>();
        obj.SetActive(false);
        pool.Enqueue(arrow);
    }

    public Arrow GetArrow()
    {
        if (pool.Count == 0)
        {
            Debug.LogWarning("[ArrowPool] Pool exhausted, creating an extra arrow.");
            CreateNewArrow();
        }

        Arrow arrow = pool.Dequeue();
        arrow.gameObject.SetActive(true);
        return arrow;
    }

    public void ReturnArrow(Arrow arrow)
    {
        arrow.gameObject.SetActive(false);
        pool.Enqueue(arrow);
    }
}