using UnityEngine;

public class SelectionManager : MonoBehaviour
{   
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    private Transform _selection;
    private Material _originalMaterial;

    private void Update() {
        if (_selection != null) {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            if (selectionRenderer != null) {
                // Reset to original material
                selectionRenderer.material = _originalMaterial;
            }
            _selection = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag)) {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null) {
                    // Store the original material
                    _originalMaterial = selectionRenderer.material;
                    // Apply the highlight material
                    selectionRenderer.material = highlightMaterial;
                    _selection = selection;
                }
            }
        }
    }
}
