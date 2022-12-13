using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    private Vector3 lookDirection;

    [SerializeField]
    private float jumpPower;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        jumpPower = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    /// <summary>
    /// 플레이어 이동 메서드.
    /// </summary>
    private void MovePlayer()
    {
        if (GameManager.Instance.GameState == eGameState.Play)
        {
            // 게임 시작시 리지드바디 중력을 true로 적용.
            if (!_rigidbody.useGravity)
            {
                _rigidbody.useGravity = true;
            }
            if (Input.GetMouseButtonDown(0) && transform.position.y < 2f)
            {
                _rigidbody.velocity = new Vector3(0, 0, 0);
                _rigidbody.AddForce(0, jumpPower, 0, ForceMode.VelocityChange);
                SoundManager.Instance.SFXPlay(_audioSource, _audioSource.clip);
            }
            lookDirection.z = _rigidbody.velocity.y * 10f + 20f;
        }

        Quaternion R = Quaternion.Euler(lookDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, R, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            _rigidbody.velocity = new Vector3(0, -3, 0);
            lookDirection = new Vector3(0, 0, -90);
            GameManager.Instance.GameOver();
        }
        else if (other.CompareTag("Safe"))
        {
            GameManager.Instance.GetScore(1);
        }
    }
}
