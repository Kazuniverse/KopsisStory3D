using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Outline : MonoBehaviour
{
    public enum Mode { OutlineAll, OutlineVisible, OutlineHidden }
    
    public Mode OutlineMode = Mode.OutlineAll;
    public Color OutlineColor = Color.white;
    public float OutlineWidth = 2f;
    
    [SerializeField] 
    private Material outlineMaskMaterial;
    [SerializeField] 
    private Material outlineFillMaterial;
    
    private Renderer[] renderers;
    private bool isEnabled;
    
    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
    
    public void EnableOutline()
    {
        if (isEnabled) return;
        
        // Create materials if they don't exist
        if (outlineMaskMaterial == null)
            outlineMaskMaterial = Instantiate(Resources.Load<Material>("OutlineMask"));
        if (outlineFillMaterial == null)
            outlineFillMaterial = Instantiate(Resources.Load<Material>("OutlineFill"));
        
        outlineFillMaterial.SetColor("_OutlineColor", OutlineColor);
        outlineFillMaterial.SetFloat("_OutlineWidth", OutlineWidth);
        
        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();
            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);
            renderer.materials = materials.ToArray();
        }
        
        isEnabled = true;
    }
    
    public void DisableOutline()
    {
        if (!isEnabled) return;
        
        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();
            materials.Remove(outlineMaskMaterial);
            materials.Remove(outlineFillMaterial);
            renderer.materials = materials.ToArray();
        }
        
        isEnabled = false;
    }
    
    void OnEnable() { if (isEnabled) EnableOutline(); }
    void OnDisable() { DisableOutline(); }
}