using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FightManager : MonoBehaviour
{
    public static bool startFgiht = false;
    public static bool isPlayerWinner;
    [SerializeField] private LocalUser _localUser;
    //0:No House 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
    [SerializeField] private Material[] _mageMaterial;
    [SerializeField] private SkinnedMeshRenderer _meshMage;

    private void Awake()
    {
        _localUser.LoadData();
    }
    // Start is called before the first frame update
    void Start()
    {
        startFgiht = false;
        _meshMage.material = _mageMaterial[_localUser.currentEventHouse];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}



