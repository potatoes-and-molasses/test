using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    ThiefDetector detector;
    void Start()
    {
        detector = GetComponent<ThiefDetector>();
        detector.OnPlayerEscaped += () => Debug.Log("Player escaped");
        detector.OnStealingDetection += () => Debug.Log("Player enter");
    }


}
