
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
[RequireComponent(typeof(Camera))]
public class MiniMap : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] [Range(20, 600)] private float _yOffset = 120;

    private Camera _camera;
    private void Start()
    {
        _camera = GetComponent<Camera>();
    }
    private void Update()
    {
        _camera.transform.position = _player.transform.position + (Vector3.up * _yOffset);
    }
}