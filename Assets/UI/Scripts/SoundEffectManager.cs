using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum EffectType
{
    Button,
    ShotCube,
    HumanDie,
    AlienDie,
    SkillReady,
    HitProtect,
    GameStart,
    Skill_Red,
    Skill_Yellow,
    Skill_Green,
    Skill_Blue,
    Skill_All
}


[System.Serializable]
public class SoundEffect
{
    public EffectType type;
    public AudioClip clip;
}

public class SoundEffectManager : Singleton<SoundEffectManager> {


    public SoundEffect[] soundEffects;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);

        if (instance != null)
            Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(EffectType type)
    {
        PlaySound(type, Camera.main.transform.position);
    }

    public void PlaySound(EffectType type, Vector3 point)
    {
        var clip = soundEffects.FirstOrDefault(_ => _.type == type).clip;
        if (clip)
            AudioSource.PlayClipAtPoint(clip, point);
    }

}
