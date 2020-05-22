using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class knife : MonoBehaviour
{
    [SerializeField]
    private Material _mat;
    [SerializeField]
    private LayerMask _mask;
    private int _orgpos = 8;
    private float _speed=2;
    private Vector3 _destination;
    private bool _moving = false;
    private bool _gameStarted = false;
    [SerializeField]
    private GameManager _gm;

    // Update is called once per frame
    void Update()
    {   //If the game has started
        if (_gameStarted) {
            //Calculate movement step
            float step = _speed * Time.deltaTime;
            //If the user taps or clicks
            if (Input.GetMouseButton(0))
            {   //Allow movement for the knife but not for the sliceable
                _moving = true;
                //Set the destination to a point below the sliceable
                _destination = new Vector3(1.85f, 6.5f, -4.7f);
            }
            else
            {   //Set the destination to the original position
                _destination = new Vector3(1.85f, 8, -4.7f);
            }
            if (this.transform.position.y > 7.5f)//if the knife is above the object 
            {   //allow movement for the sliceable but not for the knife
                _moving = false;
            }
            //move the knife towards the destination smoothly
            transform.position = Vector3.MoveTowards(transform.position, _destination, step);
        }
    }
    // Cut the gameobjects and add the required components to them
    public void Cut()
    {
        //Get all the colliders that can be used to created slicedhulls into an array
        Collider[] toBeCut = Physics.OverlapBox(transform.position, new Vector3(1f, 0.1f, .1f), transform.rotation, _mask);
        //Iterate through the array
        foreach (Collider col in toBeCut)
        {
            //Skip the iteration if our collider is a trigger this is done to avoid duplicate cuts 
            //since the sliceable object has one trigger and one normal collider
            if (col.isTrigger)
            {
                continue;
            }
            //Create a sliced hull!
            SlicedHull CutObject = Slice(col.GetComponent<Collider>().gameObject, _mat);
            //If our cut object isn't null i.e. we made a valid cut
            if (CutObject != null)
            {
                //Create a cut up and down portion from the object
                GameObject cutup = CutObject.CreateUpperHull(col.gameObject, _mat);
                GameObject cutdown = CutObject.CreateLowerHull(col.gameObject, _mat);
                //Add the required components (colliders etc) to the objects depending on if they're staying or flying away
                //The cut up gameobject will be the slice so it will fly away
                AddComponent(cutup, false);
                //The cut down will stay and will be sliceable
                AddComponent(cutdown, true);
                //Destroy the current game object that was just sliced
                Destroy(col.gameObject);

            }
            else
            {
                //Not a valid slice
                Debug.Log("Cutobject is null, continuing");
                break;
            }
        }
    }
    //Slice the two game objects with easyslice
    public SlicedHull Slice(GameObject obj, Material mat = null)
    {
        return obj.Slice(transform.position, transform.up, mat);
    }
    //Add different components to the game objects depending on if they're staying or not
    void AddComponent(GameObject obj,bool stay)
    {
        
        obj.gameObject.layer = 8;
        obj.AddComponent<BoxCollider>();
        obj.GetComponent<BoxCollider>().size = new Vector3(obj.GetComponent<BoxCollider>().size.x, 0, obj.GetComponent<BoxCollider>().size.z);
        obj.GetComponent<BoxCollider>().center = new Vector3(obj.GetComponent<BoxCollider>().center.x, 0, obj.GetComponent<BoxCollider>().center.z);
        BoxCollider bcol = obj.AddComponent<BoxCollider>();
        bcol.isTrigger = true;

        if (!stay)
        { 
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        obj.GetComponent<Rigidbody>().AddExplosionForce(100,obj.transform.position,20);
            Destroy(obj, 3f);
        }
        else
        {
        obj.AddComponent<Sliceable>();
        Sliceable temp = obj.GetComponent<Sliceable>();
        temp.SetMaster(this);
        temp.SetGameStarted(true);
        _gm.SetSliceable(temp);
        }
    }
    //OCE function to call the cut function when we reach the bottom of the object
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Cutting!");
        Cut();
    }

    //Setter Getters
    public bool GetMoving()
    {
        return _moving;
    }
    public bool GetGameStarted()
    {
        return _gameStarted;
    }
    public void SetGameStarted(bool gs)
    {
        _gameStarted = gs;
    }
}
