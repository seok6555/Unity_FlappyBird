using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    //�ڱ� �ڽ��� �ν��Ͻ�ȭ
    private static PoolingManager instance;

    //������Ʈ Ǯ���� ���� ��ֹ� Queue.
    private Queue<Obstacle> obstacleQueue = new Queue<Obstacle>();

    public GameObject obstacle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //ó���� ��ֹ� 3���� ����� ����.
        Initialize(3);
    }

    /// <summary>
    /// ������Ʈ �ʱ� ���� �޼���.
    /// </summary>
    /// <param name="count">������ ������Ʈ�� ��.</param>
    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            obstacleQueue.Enqueue(CreateNewObject());
        }
    }

    /// <summary>
    /// ���ο� ������Ʈ ���� �޼���.
    /// </summary>
    /// <returns></returns>
    private Obstacle CreateNewObject()
    {
        Obstacle newObject = Instantiate(obstacle, transform).GetComponent<Obstacle>();
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    /// <summary>
    /// ������ƮǮ���� ������Ʈ�� ������ �޼���.
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
    /// ������ ������Ʈ�� �����޴� �޼���.
    /// </summary>
    /// <param name="obstacle">�������� ������Ʈ.</param>
    public static void ReturnObject(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(false);
        obstacle.transform.SetParent(instance.transform);
        instance.obstacleQueue.Enqueue(obstacle);
    }
}
