using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class ARMultipleObjectController : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager arRaycastManager;

    [SerializeField]
    Camera arCamera;

    GameObject selectedPrefab;
    ARObject selectedObject;

    private static List<ARRaycastHit> arHits = new List<ARRaycastHit>();
    private static RaycastHit physicsHit;

    private float scale = 1.0f;
    private float angle = 0.0f;

    public void SetSelectedPrefab(GameObject selectedPrefab)
    {
        this.selectedPrefab = selectedPrefab;
    }

    public void UpdateScale(float sliderValue)
    {
        scale = sliderValue;
        if (selectedObject)
        {
            selectedObject.transform.localScale = Vector3.one * scale;
        }
    }

    public void UpdateRotation(float sliderValue)
    {
        angle = sliderValue;
        if (selectedObject)
        {
            selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    private void Awake()
    {
        selectedPrefab = arRaycastManager.raycastPrefab;
    }

    // �����Ӹ��� ������Ʈ
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        if (IsPointOverUIObject(touchPosition))
        {
            return ;
        }

        // ��ü�� ���õǸ� Selected
        if (touch.phase == TouchPhase.Began)    // ��ġ�� ó�� �߻��� ������ ����
        {
            SelectARObject(touchPosition);
        }
        else if (touch.phase == TouchPhase.Ended) // ��ġ���� ���� �������� ��
        {
            if (selectedObject)
            {
                selectedObject.Selected = false;
            }
        }
        
        SelectARPlane(touchPosition);   // ARObject�� ���õ��� �ʾҴٸ�(=�� ����� ���õǸ�) arRaycast ���(�� ��ü ����)
    }

    private bool SelectARObject(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out physicsHit))   // �������� ������Ʈ�� �ε����� ��
        {
            selectedObject = physicsHit.transform.GetComponent<ARObject>();
            if (selectedObject)
            {
                selectedObject.Selected = true;
                return true;
            }
        }
        return false;
    }

    private void SelectARPlane(Vector2 touchPosition)
    {
        if (arRaycastManager.Raycast(touchPosition, arHits, TrackableType.PlaneWithinPolygon))   // TouchPhase.Began : ó�� ��ġ�� �� �������� Raycast�� ��� �� ��ġ�� ��ü�� ����
        {
            Pose hitPose = arHits[0].pose;

            if (!selectedObject)
            {
                var newARObj = Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);
                selectedObject = newARObj.AddComponent<ARObject>();
                selectedObject.transform.localScale = Vector3.one * scale;    // ��ü ������ �̸� ������ scale�� ����
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));   // ��ü ������ �̸� ������ rotation���� ����
            }
            else if (selectedObject.Selected)
            {
                selectedObject.transform.position = hitPose.position;
                selectedObject.transform.rotation = hitPose.rotation;
            }
        }
    }

    // UI Ŭ���� ������Ʈ ���� X
    // UI�� arRaycast���� �켱������ ���� ����
    bool IsPointOverUIObject(Vector2 pos)
    {
        PointerEventData eventDataCurPosition = new PointerEventData(EventSystem.current);
        eventDataCurPosition.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurPosition, results);
        return results.Count > 0;
    }
}
