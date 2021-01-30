using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopController : MonoBehaviour
{
    ThiefDetector detector;
    // Start is called before the first frame update
    void Start()
    {
        detector = GetComponent<ThiefDetector>();
        detector.OnStealingDetection += NoticePlayer;
    }

    void NoticePlayer()
    {
        Debug.Log("MR POLICE");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
