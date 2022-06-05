using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDictionary : MonoBehaviour
{
    protected class LockingData{
        public float lockingTime = 0.0f;
        public GameObject gameObject;
        public Renderer renderer;
        public Color defalutColor;
        public LockingData(GameObject gameObject)
        {
            this.gameObject     = gameObject;
            this.renderer       = gameObject.GetComponent<Renderer>();
            this.defalutColor   = renderer.material.color;
        }
    }
    private Dictionary<int, LockingData> _objTable = new Dictionary<int, LockingData>();
    
    public void AddLockedObject(GameObject obj)
    {
        int id = obj.GetInstanceID();
        if( _objTable.ContainsKey( id ) == false )
        {
            LockingData data = new LockingData(obj);
            _objTable.Add(id, data );
            data.renderer.material.color = new Color(0, 1.0f, 0);
            Debug.Log( obj.name + " is been locked!" );
        }
    }

    public void RemoveLockedObject(GameObject obj)
    {
        int id = obj.GetInstanceID();
        if( _objTable.ContainsKey( id ) )
        {
            LockingData data = _objTable[ id ];
            _objTable.Remove( id );
            data.renderer.material.color = data.defalutColor;            
            Debug.Log( obj.name + " lock failed!" );
        }
    }

    public void Clear()
    {
        _objTable.Clear();
    }
}
