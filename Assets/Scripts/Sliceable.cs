using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.1f;
    private bool _allowedToMove = false;
    private bool _gameStarted = false;
    private Vector3 _destination;
    [SerializeField]
    private knife _master;
    [SerializeField]
    private Material[] _materials;
    [SerializeField]
    private Mesh[] _meshes;

    // Start is called before the first frame update
    void Start()
    {
        //Set the destination that the sliceable object will slowly move to
        _destination = new Vector3(transform.position.x + 10, transform.position.y ,transform.position.z);
    }
    //Randomly select a cutable shape each time the scene loads
    public void SelectShape()
    {
        //Get a random int
        int rand = Random.Range(0, 5);
        //get the mesh filter and renderer components
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        //assign a mesh and a material to them
        mf.mesh = _meshes[rand];
        mr.material = _materials[rand];
    }
    // Update is called once per frame
    void Update()
    {
        //If our game has started
        if (_gameStarted)
        {
            //Check if the master gameobject is okay with movement
            _allowedToMove = (_master.GetMoving());
            //Calculate the movement step
            float step = _speed * Time.deltaTime;
            //If the master is okay with movement
            if (!_allowedToMove)
            {
                //Smoothly move the object away from the knife
                transform.position = Vector3.MoveTowards(transform.position, _destination, step);
            }
        }
    }
    // Add some spice to the cutting process by making the phone vibrate
    void OnTriggerStay(Collider other)
    {
        Handheld.Vibrate();
    }

    //Setter getters
    public void SetMaster(knife mas)
    {
        this._master = mas;
    }
    public void SetGameStarted(bool start)
    {
        this._gameStarted = start;
    }
}
