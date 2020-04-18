using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private NavMeshAgent _agent;
    private Vector3 _mouseOffset;
    private int _distance = 10;
    private float _mouseZposition;
    private float _waitingTime;
    [SerializeField] private string habitat; 
    private Coroutine _movementCoroutine;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _movementCoroutine = StartCoroutine(Movement());
    }

    private void OnMouseDown()
    {
        Cursor.visible = false;
        _mouseZposition = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        _mouseOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + _mouseOffset;
    }

    private void OnMouseUp()
    {
        Cursor.visible = true;
        _mouseOffset = Vector3.zero;
        _mouseZposition = 0;
        _agent.isStopped = true;
        StopCoroutine(_movementCoroutine);
        _movementCoroutine = StartCoroutine(Movement());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(habitat) || other.CompareTag("Untagged")) return;
        GameManager.instance.GameOver();
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(habitat) || other.gameObject.CompareTag("Untagged")) return;
        GameManager.instance.GameOver();
        Destroy(gameObject);
    }

    private Vector3 GetMouseWorldPosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = _mouseZposition;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private Vector3 GetNextPosition()
    {
        var randomDirection = Random.insideUnitSphere * _distance;

        randomDirection += transform.position;

        NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, _distance, layerMask);

        return navHit.position;
    }

    private IEnumerator Movement()
    {
        _waitingTime = Random.Range(2, 5);
        _agent.isStopped = false;
        var targetPosition = GetNextPosition();
        _agent.destination = targetPosition;
        yield return new WaitForSeconds(_waitingTime);
        _movementCoroutine = StartCoroutine(Movement());
    }
}