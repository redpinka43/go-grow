using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FluffyHippo
{
    public class ColorChanger : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            ChangeColor();
            Debug.Log("hit collider");
        }

        public void ChangeColor()
        {
            Color newColor = new Color(Random.Range(0.0f, 1.0f),
                            Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
            GetComponent<Image>().color = newColor;
        }
    }
}
