using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : SingletonMonoBehaviour<BGMPlayer>
{
    [SerializeField] private AudioSource defaultBGM;
    [SerializeField] private AudioSource stageClear;
    [SerializeField] private AudioSource gameOver;

    private Coroutine _corutine = null;
    public bool IsPlayingMusic => _corutine != null;

    void Start()
    {
        defaultBGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsStageClear)
        {
            _corutine = StartCoroutine(_StageClear());
        }
        else if (GameManager.Instance.IsGameOver)
        {
            _corutine = StartCoroutine(_GameOver());
        }
    }

    private IEnumerator _StageClear()
    {
        yield return _FadeOut(1);
        stageClear.Play();
        Destroy(this);
    }

    private IEnumerator _GameOver()
    {
        yield return _FadeOut(1);
        gameOver.Play();
        Destroy(this);
    }

    private IEnumerator _FadeOut(float fadeoutTime)
    {
        for (float i = 0; i < fadeoutTime;)
        {
            defaultBGM.volume *= 0.95f;
            i += Time.deltaTime;
            yield return null;
        }
    }
}
