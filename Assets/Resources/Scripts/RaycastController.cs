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
/*
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, targetLayer))
        {
            if (hit.collider.gameObject.CompareTag("Target Cube"))
            {
                reticule.color = targetColor;
            }
        }
        else
        {
            reticule.color = defaultColor;
        }
    }
*/
    void Update()
    {/*
        RaycastHit hit;
        bool isTargetHit = Physics.Raycast(transform.position, transform.forward, out hit, rayLength, targetLayer);

        if (isTargetHit && hit.collider.gameObject.CompareTag("ActivityExecuteButton"))
        {
            ButtonController buttonController = hit.collider.gameObject.GetComponent<ButtonController>();
            
            if (buttonController != null && buttonController.ButtonEnabled)
            {
                reticule.color = targetColor;
            }
            else
            {
                reticule.color = disabledColor;
            }
        }
        else if (isTargetHit && hit.collider.gameObject.CompareTag("ActivityExecuteButtonExcluded"))
        {
            reticule.color = disabledColor;
        }
        else
        {
            reticule.color = defaultColor;
        }
        */
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
            else
            {
                reticule.color = defaultColor;
            }
        }
    }
}