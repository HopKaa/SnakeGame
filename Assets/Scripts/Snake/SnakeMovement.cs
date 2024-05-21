using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private float segmentSpacing = 1f; // ���������� ����� ����������
    private List<Transform> segments;
    private List<Vector3> positions;
    private Vector3 direction = Vector3.forward;
    private Vector3 previousDirection;

    private void Start()
    {
        segments = new List<Transform>();
        positions = new List<Vector3>();
        segments.Add(this.transform);
        positions.Add(transform.position);
    }

    private void Update()
    {
        HandleInput();
        MoveSnake();
        UpdateSegments();
    }

    private void HandleInput()
    {
        if (direction == Vector3.forward || direction == Vector3.back)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector3.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector3.right;
            }
        }
        else if (direction == Vector3.left || direction == Vector3.right)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector3.forward;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector3.back;
            }
        }

        previousDirection = direction;
    }

    private void MoveSnake()
    {
        float move = speed * Time.deltaTime;
        transform.Translate(direction * move);
        positions.Insert(0, transform.position);

        // ������������ ������ ������ �������
        if (positions.Count > segments.Count)
        {
            positions.RemoveAt(positions.Count - 1);
        }
    }

    private void UpdateSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 targetPosition = segments[i - 1].position; // ������ ������� ������� �� ���������� ���������
            segments[i].position = Vector3.Lerp(segments[i].position, targetPosition, speed * Time.deltaTime);
        }
    }

    public void AddSegment()
    {
        Transform segment = Instantiate(segmentPrefab);
        Vector3 newPosition = segments[segments.Count - 1].position - segments[segments.Count - 1].forward * segmentSpacing;
        segment.position = newPosition;
        segments.Add(segment);
        positions.Add(segment.position);

        // ������ �������� ��� ���������� ���������� � ���������
        StartCoroutine(AddColliderWithDelay(segment));
    }

    private IEnumerator AddColliderWithDelay(Transform segment)
    {
        BoxCollider collider = segment.gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.enabled = false; // ��������� ��������� �� ��������� �����
        yield return new WaitForSeconds(1f); // �������� � 1 �������
        segment.tag = "SnakeSegment"; // ����������� ��� "SnakeSegment" �������� ����� ��������
        collider.enabled = true; // �������� ��������� ����� ��������
    }

    public List<Vector3> GetSegmentPositions()
    {
        return new List<Vector3>(positions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SnakeSegment"))
        {
            // ������������ ������ ���� � ��������� - ����� ��������
            Debug.Log("Game Over! You collided with a snake segment!");
            // ����� ����� �������� ������ ��� ���������� ���� ��� ����������� ������
        }
    }
}