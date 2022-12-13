using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    //자기 자신을 인스턴스화
    private static PoolingManager instance;

    //오브젝트 풀링을 위한 장애물 Queue.
    private Queue<Obstacle> obstacleQueue = new Queue<Obstacle>();

    public GameObject obstacle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //처음에 장애물 3개를 만들어 놓음.
        Initialize(3);
    }

    /// <summary>
    /// 오브젝트 초기 생성 메서드.
    /// </summary>
    /// <param name="count">생성할 오브젝트의 수.</param>
    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            obstacleQueue.Enqueue(CreateNewObject());
        }
    }

    /// <summary>
    /// 새로운 오브젝트 생성 메서드.
    /// </summary>
    /// <returns></returns>
    private Obstacle CreateNewObject()
    {
        Obstacle newObject = Instantiate(obstacle, transform).GetComponent<Obstacle>();
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    /// <summary>
    /// 오브젝트풀에서 오브젝트를 빌리는 메서드.
    /// </summary>
    /// <returns></returns>
    public static Obstacle GetObject()
    {
        if (instance.obstacleQueue.Count > 0)
        {
            Obstacle obj = instance.obstacleQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            Obstacle newObject = instance.CreateNewObject();
            newObject.transform.SetParent(null);
            newObject.gameObject.SetActive(true);
            return newObject;
        }
    }

    /// <summary>
    /// 빌려준 오브젝트를 돌려받는 메서드.
    /// </summary>
    /// <param name="obstacle">돌려받을 오브젝트.</param>
    public static void ReturnObject(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(false);
        obstacle.transform.SetParent(instance.transform);
        instance.obstacleQueue.Enqueue(obstacle);
    }
}
