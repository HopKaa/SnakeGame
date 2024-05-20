using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private BoxCollider playArea;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = playArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        transform.position = new Vector3(Mathf.Round(x), 0.5f, Mathf.Round(z));
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