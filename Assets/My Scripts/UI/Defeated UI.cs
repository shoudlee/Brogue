using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

namespace Brogue.UI
{
    public class DefeatedUI : MonoBehaviour
    {
        [SerializeField] private Image defeatedBackgroundImage;
        
        [Range(0, 0.01f)]
        [SerializeField] private float colorRChangingSpeed = 0.001f;
        private float minColorR = 0.725f;
        private float maxColorR = 1f;
        private float currentR;
        private Color currentColor;
        private bool order;
        // private float textBorderness;

        private void Awake()
        {
            order = true;
            currentR = minColorR;
        }

        private void Start()
        {
            currentColor = defeatedBackgroundImage.color;
        }

        private void Update()
        {
            if (order)
            {
                currentR += colorRChangingSpeed;
                currentColor.r = currentR;
                defeatedBackgroundImage.color = currentColor;
                if (currentR >= maxColorR)
                {
                    currentR = maxColorR;
                    order = false;
                }
            }
            else
            {
                currentR -= colorRChangingSpeed;
                currentColor.r = currentR;
                defeatedBackgroundImage.color = currentColor;
                if (currentR <= minColorR)
                {
                    currentR = minColorR;
                    order = true;
                }
            }
        }
    }

}
