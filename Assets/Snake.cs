using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class snake : MonoBehaviour
{
    private Vector2Int _direction = Vector2Int.right;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    public int initialSize = 4;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        // Check if W key was just pressed and direction isn't currently pointing downwards
        if (Keyboard.current.wKey.wasPressedThisFrame && _direction != Vector2Int.down)
            _direction = Vector2Int.up;

        else if (Keyboard.current.sKey.wasPressedThisFrame && _direction != Vector2Int.up) // Same as above but for downwards
            _direction = Vector2Int.down;

        else if (Keyboard.current.aKey.wasPressedThisFrame && _direction != Vector2Int.right)  // Same as before but for left
            _direction = Vector2Int.left;

        else if (Keyboard.current.dKey.wasPressedThisFrame && _direction != Vector2Int.left)    // Same as above but for right
            _direction = Vector2Int.right;
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
        // Move the snake at a constant speed
        this.transform.position = new Vector2(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);

        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(this.transform);

        this.transform.position = Vector2.zero;

        for (int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Grow();
        }
        else if (collision.tag == "Obstacle" || collision.tag == "Wall")
        {
            ResetState();
        }
    }
}
