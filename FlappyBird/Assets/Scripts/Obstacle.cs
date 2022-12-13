using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float obstacleSpeed;

    // Update is called once per frame
    void Update()
    {
        MoveObstacle();
        DestroyObstacle();
    }

    /// <summary>
    /// ��ֹ� �̵� �޼���.
    /// </summary>
    private void MoveObstacle()
    {
        transform.Translate(Vector3.left * obstacleSpeed * Time.deltaTime);
    }

    /// <summary>
    /// ��ֹ� �ı� �޼���.
    /// </summary>
    private void DestroyObstacle()
    {
        if (transform.position.x < -6)
        {
            PoolingManager.ReturnObject(this);
        }
    }
}
