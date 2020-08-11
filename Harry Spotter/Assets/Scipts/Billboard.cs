using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform _came;

    private void LateUpdate()
    {
        //transform.LookAt(transform.position + _came.forward);
        transform.LookAt(new Vector3(_came.position.x,transform.position.y,_came.position.z));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
