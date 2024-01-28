using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{   
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private GameObject panelToToggle;
    public Text displayText;

    private Transform _currentHover;
    private Material _originalMaterialHover;

    private void Update() {
        if (_currentHover != null) {
            var hoverRenderer = _currentHover.GetComponent<Renderer>();
            if (hoverRenderer != null) {
                hoverRenderer.material = _originalMaterialHover;
            }
            _currentHover = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag)) {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null) {
                    // Highlight logic
                    if (_currentHover != selection) {
                        _originalMaterialHover = selectionRenderer.material;
                        selectionRenderer.material = highlightMaterial;
                        _currentHover = selection;
                    }

                    if (Input.GetMouseButtonDown(0)) {
                        panelToToggle.SetActive(!panelToToggle.activeSelf); // Toggle the panel
                        UpdateDescription(selection);
                    }
                }
            }
        }
    }

    private void UpdateDescription(Transform selectedObject) {
        CoralDescription coralDesc = selectedObject.GetComponent<CoralDescription>();
        Debug.Log(coralDesc.description);
        if (coralDesc != null) {
            Debug.Log("entered");
            displayText.text = (coralDesc.description);
            
        }
        else {
            displayText.text = "Shelf coral \n Montipora capricornis \nA type of shelf coral. This flat, mushroom-shaped plant is a hard coral of the stony coral species. Their pattern of growth is optimized to increase exposed surface area and access to light";
        }
    }
}
