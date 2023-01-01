using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private Vector3 _startPosition, _targetPosition;
    private float _progress;

    [SerializeField] private float _speed = 40f;
    void Start()
    {
        _startPosition = transform.position.WithAxis(Axis.Z, -1);
    }

    void Update()
    {
        _progress += Time.deltaTime * _speed;
        transform.position = Vector3.Lerp(_startPosition, _targetPosition, _progress); 
    }

    public void SetTargetPosition(Vector3 targetPos){
        _targetPosition = targetPos.WithAxis(Axis.Z, -1);
    }
}
