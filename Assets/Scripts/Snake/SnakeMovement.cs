using UnityEngine;
using System.Collections.Generic;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private float segmentSpacing = 1f; // Расстояние между сегментами
    private List<Transform> segments;
    private List<Vector3> positions;

    private void Start()
    {
        segments = new List<Transform>();
        positions = new List<Vector3>();
        segments.Add(this.transform);
        positions.Add(transform.position);
    }

    private void Update()
    {
        MoveSnake();
        UpdateSegments();
    }

    private void MoveSnake()
    {
        float move = speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * rotation);

        positions.Insert(0, transform.position);

        // Ограничиваем размер списка позиций
        if (positions.Count > segments.Count + 1)
        {
            positions.RemoveAt(positions.Count - 1);
        }
    }

    private void UpdateSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 targetPosition = segments[i - 1].position; // Каждый сегмент следует за предыдущим сегментом
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
    }
}