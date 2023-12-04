using UnityEngine;
using UnityEngine.UI;

public class ReticuleRaycaster : MonoBehaviour
{
    public Image reticule;  // Assign the reticule UI Image in the Inspector.
    public Color defaultColor = Color.white;
    public Color targetColor = Color.green;
    public Color disabledColor = Color.red;

    
    public float rayLength = 100f;  // Adjust as needed based on your scene scale.
    public LayerMask targetLayer;   // Set this to the layer of your target objects in the Inspector.
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, targetLayer))
        {
            GameObject hitGameObject = hit.collider.gameObject;
            if (hitGameObject.CompareTag("ActivityExecuteButton") || hitGameObject.CompareTag("ActivityExecuteButtonExcluded"))
            {
                ButtonController buttonController = hitGameObject.GetComponent<ButtonController>();
                if (buttonController.ButtonEnabled)
                {
                    reticule.color = targetColor;
                }
                else
                {
                    reticule.color = disabledColor;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    hitGameObject.SendMessage("OnSelected", SendMessageOptions.DontRequireReceiver);
                }
            }
        }        
        else
        {
            reticule.color = defaultColor;
        }
    }
}