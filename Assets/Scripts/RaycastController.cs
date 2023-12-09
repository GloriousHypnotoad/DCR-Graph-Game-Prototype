using UnityEngine;
using UnityEngine.UI;

public class RaycastController : MonoBehaviour
{
    public Image reticule;  // Assign the reticule UI Image in the Inspector.
    public Color defaultColor = Color.white;
    public Color targetColor = Color.green;
    public Color disabledColor = Color.red;    
    public float rayLength = 100f;  // Adjust as needed based on your scene scale.
    private LayerMask _targetLayer;   // Set this to the layer of your target objects in the Inspector.

    void Awake()
    {
        reticule = GetComponentInChildren<Image>();
    }

    public void SetTargetLayer(int targetLayer){
        _targetLayer = 1 << targetLayer;
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, _targetLayer))
        {
            GameObject hitGameObject = hit.collider.gameObject;
            if (hitGameObject.CompareTag("ButtonsContainer"))
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
                    //TODO: emit an event for the controller instead.
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