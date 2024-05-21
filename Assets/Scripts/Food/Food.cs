using UnityEngine;
using System.Collections.Generic;

public class Food : MonoBehaviour
{
    [SerializeField] private BoxCollider playArea;
    private SnakeMovement snakeMovement;

    private void Start()
    {
        snakeMovement = FindObjectOfType<SnakeMovement>();
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = playArea.bounds;
        Vector3 newPosition;
        bool isPositionValid;

        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            newPosition = new Vector3(Mathf.Round(x), 0.5f, Mathf.Round(z));

            isPositionValid = true;
            List<Vector3> segmentPositions = snakeMovement.GetSegmentPositions();

            foreach (Vector3 position in segmentPositions)
            {
                if (Vector3.Distance(newPosition, position) < 0.5f)
                {
                    isPositionValid = false;
                    break;
                }
            }

        } while (!isPositionValid);

        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RandomizePosition();
            SnakeMovement snake = other.GetComponent<SnakeMovement>();
            if (snake != null)
            {
                snake.AddSegment();
            }
            else
            {
                Debug.LogError("SnakeMovement component not found on the Player object.");
            }
        }
    }
}