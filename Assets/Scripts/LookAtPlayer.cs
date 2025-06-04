using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        LockOnTarget();
    }
    
    void LockOnTarget()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f; // offset by 90 bc gun facing down by default
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
