using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brogue.Core{
public class FrameRateSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         Application.targetFrameRate = 60;
    }
}
}
