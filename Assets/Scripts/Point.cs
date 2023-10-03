using UnityEngine;

public class Point : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Transform _pointTransform;

    private Color _defaultColor;
    private void Awake()
    {
        _pointTransform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
    }

    private void OnMouseEnter()
    {
        _spriteRenderer.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        _spriteRenderer.color = _defaultColor;
    }

    private void OnMouseDrag()
    {
        var mouseWorldPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        _pointTransform.position = mouseWorldPosition;
    }
}