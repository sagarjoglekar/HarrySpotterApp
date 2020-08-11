using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 50f;
    [SerializeField] private float _Amplitude = 2.0f;
    [SerializeField] private float _frequencey = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y + 500, transform.position.z);
        //_startPose = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.fixedTime * Mathf.PI * _frequencey) * _Amplitude) + 15, transform.position.z);
        //Vector3 tempPos = _startPose;
        //tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * _frequencey) * _Amplitude;

        //transform.position = tempPos;
    }
}
