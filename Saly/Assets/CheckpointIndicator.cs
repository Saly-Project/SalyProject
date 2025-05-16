using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointIndicator : MonoBehaviour
{
    public Transform checkpointTarget;
    public Camera cam;
    public RectTransform indicatorUI;
    public float screenEdgeBuffer = 50f;

    private RectTransform canvasRect;
    public CheckpointManager manager;

    


    

    void Awake()
    {
        
        Canvas parentCanvas = indicatorUI.GetComponentInParent<Canvas>();
        
        canvasRect = parentCanvas.GetComponent<RectTransform>();
        
        checkpointTarget = null;
        
        
    }




    void Update()
    {
        
        

        if (checkpointTarget == null || cam == null || indicatorUI == null)
        {
            indicatorUI.gameObject.SetActive(false);
            return;
        }
            

        Vector3 screenPoint = cam.WorldToScreenPoint(checkpointTarget.position);

        // Est-ce que le checkpoint est devant la caméra ?
        bool isBehind = screenPoint.z < 0;

        // Si derrière, on inverse la direction pour pointer vers l'arrière
        if (isBehind)
        {
            screenPoint *= -1;
        }

        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        Vector2 screenPos = new Vector2(screenPoint.x, screenPoint.y);

        Vector2 dir = (screenPos - screenCenter).normalized;

        // Bord de l'écran en tenant compte du buffer
        float maxX = (canvasRect.rect.width / 2f) - screenEdgeBuffer;
        float maxY = (canvasRect.rect.height / 2f) - screenEdgeBuffer;

        // Calcule la position sur le bord de l'écran en fonction de la direction
        Vector2 cappedPos = dir * Mathf.Min(
            Mathf.Abs(maxX / dir.x),
            Mathf.Abs(maxY / dir.y)
        );

        indicatorUI.anchoredPosition = cappedPos;

        // Rotation (flèche doit pointer vers le checkpoint)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        indicatorUI.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Activation uniquement si le checkpoint n’est pas à l’écran
        bool isOffScreen = screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height;
        indicatorUI.gameObject.SetActive(isOffScreen);



        // Pulsation
        float pulse = 0.5f + Mathf.Sin(Time.time * 10f) * 0.03f;
        indicatorUI.localScale = new Vector3(pulse, pulse, 1f);


    }

    
}
