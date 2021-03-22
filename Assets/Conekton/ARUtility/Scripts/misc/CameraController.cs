using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.Player.Domain;
using UnityEngine;
using Zenject;

public class CameraController : ITickable
{
    [Inject] private IPlayer _player = null;
    
    private float _moveSpeed = 3f;
    private float _rotateSpeed = 10f;
    private float _boost = 2f;
    
    private bool _isMoveMode = false;
    private Vector3 _prevPos = Vector3.zero;

    private Transform Target => _player.Root;

    private float MoveSpeed
    {
        get
        {
            float speed = _moveSpeed * Time.deltaTime;
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed *= _boost;
            }

            return speed;
        }
    }

    private float RotateSpeed => _rotateSpeed * Time.deltaTime;

    public CameraController(float moveSpeed, float rotateSpeed, float boost)
    {
        _moveSpeed = moveSpeed;
        _rotateSpeed = rotateSpeed;
        _boost = boost;
    }
    
    void ITickable.Tick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartMove();
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            EndMove();
        }

        if (_isMoveMode)
        {
            TryMove();
            TryRotate();
        }
    }

    private void StartMove()
    {
        _isMoveMode = true;
        _prevPos = Input.mousePosition;
    }

    private void EndMove()
    {
        _isMoveMode = false;
    }

    private void TryMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Target.position += Target.forward * MoveSpeed;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            Target.position  += -Target.right * MoveSpeed;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            Target.position += -Target.forward * MoveSpeed;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            Target.position += Target.right * MoveSpeed;
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            Target.position += -Target.up * MoveSpeed;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            Target.position += Target.up * MoveSpeed;
        }
    }

    private void TryRotate()
    {
        Vector3 delta = Input.mousePosition - _prevPos;
        
        Target.Rotate(Vector3.up, delta.x * RotateSpeed, Space.World);
        
        Vector3 rightAxis = Vector3.Cross(Target.forward, Vector3.up);
        Target.Rotate(rightAxis.normalized, delta.y * RotateSpeed, Space.World);
        
        _prevPos = Input.mousePosition;
    }
}