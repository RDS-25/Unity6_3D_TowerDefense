using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    public Vector3 distance = Vector3.down *20f;
    public Transform targetTransform;
    public RectTransform rectTransform;
    public void Setup(Transform  target){
        targetTransform =target;
        rectTransform =GetComponent<RectTransform>();
    }
    // Update is called once per frame
     private void LateUpdate() {
        if(targetTransform ==null){
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos= Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position =screenPos+distance;
    }
}
