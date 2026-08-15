using System.Collections;
using UnityEngine;

public class DiscoFloorChanger : MonoBehaviour
{
    [SerializeField] private float transitionTime = 2f;
    
    [SerializeField] private Color colorOne = new Color(0, 1, 1);     
    [SerializeField] private Color colorTwo = new Color(1, 1, 0);     
    [SerializeField] private Color colorThree = new Color(1, 0, 1);   
    [SerializeField] private int materialIndex = 0; // In case object has multiple materials

    private Material discoMaterial;
    private Color[] colorSequence;
    private int currentColorIndex = 0;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            return;
        }
        discoMaterial = renderer.materials[materialIndex];
        colorSequence = new Color[] { colorOne, colorTwo, colorThree};
        StartCoroutine(CycleColors());
    }

    private IEnumerator CycleColors()
    {
        while (true)
        {
            Color currentColor = colorSequence[currentColorIndex];
            Color nextColor = colorSequence[(currentColorIndex + 1) % colorSequence.Length];
            float elapsedTime = 0f;
            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / transitionTime;
                Color lerpedColor = Color.Lerp(currentColor, nextColor, t);
                discoMaterial.SetColor("_EmissionColor", lerpedColor);
                yield return null;
            }
            discoMaterial.SetColor("_EmissionColor", nextColor);
            currentColorIndex = (currentColorIndex + 1) % colorSequence.Length;
        }
    }
}