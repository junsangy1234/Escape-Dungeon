using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource bgmSource;
    AudioSource sfxSource;

    public float bgmVolum = 1.0f; 
    public float sfxVolum = 1.0f;

    public AudioClip Stage1Bgm;
    public AudioClip Stage2Bgm;
    public AudioClip BossBgm;
    public AudioClip DeadBgm;

    public AudioClip Swing;

    public AudioClip step_A; //발걸음 효과음
    public AudioClip step_B; //발걸음 효과음
    public AudioClip step_C; //발걸음 효과음

    public AudioClip GoblinIdle; //고블린 처음
    public AudioClip GoblinHit; //고블린 맞을때 나는 소리
    public AudioClip GoblinHit2; //고블린 맞을때 나는 소리
    public AudioClip GoblinAtk; //고블린 공격할때 나는 소리
    public AudioClip GoblinAtk2; //고블린 공격할때 나는 소리
    public AudioClip GoblinDeath; //고블린 죽을때 나는 소리

    public AudioClip SkeletonDamage;
    public AudioClip SkeletonDie;
    public AudioClip SkeletonHit;
    public AudioClip SkeletonWalk;

    public AudioClip BossSkeletonDamage;
    public AudioClip BossSkeletonDie;
    public AudioClip BossSkeletonHit;
    public AudioClip BossSkeletonWalk;

    public AudioClip D2AtkFirst;
    public AudioClip D2AtkHit;
    public AudioClip Summon;
    public AudioClip Sheild;


    public AudioClip GetCoin;
    public AudioClip DropCoin;


    public AudioClip HeartBeat;

    public AudioClip MetalDoor;

    public AudioClip VictoryBgm;

    public AudioClip itemUse; //아이템 사용 효과음
    public AudioClip levelUp; //레벨업 효과음

    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySfx(Vector3 pos, AudioClip sfx, float delayed, float volum)
    {
        StartCoroutine(PlaySfxIE(pos, sfx, delayed, volum));
    }
    IEnumerator PlaySfxIE(Vector3 pos, AudioClip sfx, float delayed, float volum)
    {
        yield return new WaitForSeconds(delayed);

        //게임 오브젝트 동적 생성
        GameObject sfxObj = new GameObject("sfx");

        AudioSource _aud = sfxObj.AddComponent<AudioSource>();

        _aud.clip = sfx;
        sfxObj.transform.position = pos;

        //거리에 의한 소리 높낮이
        _aud.minDistance = 5.0f;
        _aud.maxDistance = 10.0f;

        _aud.volume = volum; //볼륨은 최대크기는 1
        _aud.Play(); //효과음 재생

        Destroy(sfxObj, sfx.length); //효과음 종료되면 제거
    }

    public void PlayBGM(AudioClip bgm, float delayed, bool loop)
    {
        StartCoroutine(PlayBGMIE(bgm, delayed, loop));
    }

    IEnumerator PlayBGMIE(AudioClip bgm, float delayed, bool loop)
    {
        yield return new WaitForSeconds(delayed);

        GameObject bgmObj = new GameObject("BGM");

        if (!bgmSource) bgmSource = bgmObj.AddComponent<AudioSource>();

        bgmSource.clip = bgm;
        bgmSource.volume = bgmVolum;
        bgmSource.loop = loop;
        bgmSource.Play();

    }

}
