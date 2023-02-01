using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    // unity attributes
    [SerializeField] private InputAction mouseClick;
    [SerializeField] float mouseDragSpeed;


    private Camera mainCamera;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext obj)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D[] hit2DList = Physics2D.GetRayIntersectionAll(ray);

        foreach (RaycastHit2D hit2D in hit2DList)
        {
            if (hit2D.collider != null
            && (hit2D.collider.gameObject.CompareTag("Draggable")
            || hit2D.collider.gameObject.layer == LayerMask.NameToLayer("Draggable")
            || hit2D.collider.gameObject.GetComponent<IDrag>() != null))
            {
                StartCoroutine(DragUpdate(hit2D.collider.gameObject));
                break;
            }
        }
    }

    private IEnumerator DragUpdate(GameObject gameObject)
    {
        gameObject.TryGetComponent<IDrag>(out var iDragComponent);
        float initialDistance = Vector3.Distance(gameObject.transform.position, mainCamera.transform.position);
        iDragComponent?.OnStartDrag();

        while (mouseClick.ReadValue<float>() != 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
            yield return null;
        }

        iDragComponent?.OnEndDrag();
    }
}
