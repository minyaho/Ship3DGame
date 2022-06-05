using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MechineGunShootController : MonoBehaviour
{
    [Header("Sound Effect")]
    public AudioSource miniGunWarmSound;            // 音效
    public AudioSource miniGunFireSound;            // 音效
    public AudioSource miniGunStopSound;            // 音效

    [Header("Translate & GameObject")]
    public Transform aimTarget;                 // 準心 (用來將不平整的發射點有個交會點)
    public Transform mechineGunL;               // 左側發射點
    public Transform mechineGunR;               // 右側發射點
    public GameObject mechineGunBullet;         // 射出的 Object
        
    [Header("Delay")]
    public float MmaxMechineGunDelay  = 0.1f;   // 最大 Minigun 間隔射速
    public float maxWarmDelay = 0.5f;           // Minigun 暖機 (配合音效以及真實性)
    private float warmDelay;  
    private float shootDelay;

    public bool AllowUserControl {get; set;} = true;
    void Start()
    {
        warmDelay = maxWarmDelay;
        shootDelay = MmaxMechineGunDelay;
    }
    void Update()
    {
        if( Input.GetMouseButton( 0 ) && AllowUserControl )
        {
            if( (warmDelay -= Time.deltaTime) <= 0 ){
                if ( (shootDelay -= Time.deltaTime) <= 0) 
                { 
                    Shoot();
                    shootDelay = MmaxMechineGunDelay; 
                }
                if( miniGunFireSound.isPlaying == false ){ miniGunFireSound.Play(); }
            }
            if( miniGunWarmSound.isPlaying == false &&  miniGunFireSound.isPlaying == false ){  miniGunWarmSound.Play(); }
        }
        else
        {
            if( miniGunWarmSound.isPlaying || miniGunFireSound.isPlaying ){  
                miniGunStopSound.Play();
                miniGunStopSound.volume = 0.55f;
                StartCoroutine(FadeAudioSource.StartFade(miniGunStopSound, 0.9f, 0));
            }
            warmDelay = maxWarmDelay;
            shootDelay = MmaxMechineGunDelay; 
            miniGunFireSound.Stop();
            miniGunWarmSound.Stop();

        }
    }

    void Shoot()
    {
        GameObject bullectL = Instantiate( mechineGunBullet, mechineGunL.position, mechineGunL.rotation );
        GameObject bullectR = Instantiate( mechineGunBullet, mechineGunR.position, mechineGunR.rotation );

        bullectL.gameObject.transform.LookAt(aimTarget);
        bullectR.gameObject.transform.LookAt(aimTarget);

    }

    IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength)
    {

        float _StartVolume = _AudioSource.volume;

        float _StartTime = Time.time;

        while (Time.time < _StartTime + _FadeLength)
        {

            _AudioSource.volume = _StartVolume + ((_EndVolume - _StartVolume) * ((Time.time - _StartTime) / _FadeLength));

            yield return null;

        }

        if (_EndVolume == 0) { _AudioSource.Stop(); }
    }
}
