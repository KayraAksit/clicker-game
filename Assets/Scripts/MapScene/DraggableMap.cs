using UnityEngine;

public class DraggableMap : MonoBehaviour
{
    private Vector3 _previousTouchPosition; // Store the last touch position
    private Camera _camera;
    public float dragSpeed = 0.5f;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 1) // Single touch input
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Record the starting touch position
                _previousTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the difference in touch positions between frames
                Vector3 currentTouchPosition = touch.position;
                Vector3 delta = currentTouchPosition - _previousTouchPosition;

                // Convert the delta to world space and apply it to the map's position
                Vector3 worldDelta = _camera.ScreenToWorldPoint(new Vector3(delta.x, delta.y, 0)) - _camera.ScreenToWorldPoint(Vector3.zero);

                transform.position += worldDelta * dragSpeed;

                // Update the previous touch position
                _previousTouchPosition = currentTouchPosition;
            }

        }
    }


}
