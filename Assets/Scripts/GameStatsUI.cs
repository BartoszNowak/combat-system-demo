using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatsUI : MonoBehaviour
{
    [SerializeField]
    private Transform stats;
    [SerializeField]
    private TMP_Text time;
    [SerializeField]
    private TMP_Text hitsTaken;
    [SerializeField]
    private TMP_Text allContent;
    [SerializeField]
    private TMP_Text swordOnly;
    [SerializeField]
    private TMP_Text magicOnly;
    [SerializeField]
    private Color rewardColor;
    [SerializeField]
    private Color failColor;
    [SerializeField]
    private float transitionTime;

    [SerializeField]
    private float spacingBetweenTweens;
    [SerializeField]
    private float textExpandTime;

    [SerializeField]
    private AudioClip achivedSound;
    [SerializeField]
    private AudioClip failedSound;

    [SerializeField]
    private AudioSource backgroundMusic;
    [SerializeField]
    private AudioClip menuMusic;

    private GameStats gameStats;
    private AudioSource audioSource;

    public void SetValues()
    {
        gameStats = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<GameStats>();
        time.text = gameStats.TotalMinutes;
        hitsTaken.text = "hits taken: " + gameStats.hitsTaken.ToString();
    }

    void Start()
    {
        stats.localPosition = new Vector2(0, Screen.height);
        audioSource = GetComponent<AudioSource>();
    }

    public void CinematicOpen(Action callback)
    {
        backgroundMusic.clip = menuMusic;
        backgroundMusic.Play();
		time.transform.localScale = Vector3.zero;
		hitsTaken.transform.localScale = Vector3.zero;
		allContent.transform.localScale = Vector3.zero;
		swordOnly.transform.localScale = Vector3.zero;
		magicOnly.transform.localScale = Vector3.zero;
		SetValues();
		stats.localPosition = new Vector2(0, Screen.height);
		var seq = LeanTween.sequence();

        // time
        seq.append(spacingBetweenTweens);
        seq.append(LeanTween.moveLocalY(stats.gameObject, 0, transitionTime));
        seq.append(spacingBetweenTweens);
        seq.append(LeanTween.scale(time.gameObject, Vector3.one, textExpandTime));

        // hits taken
        seq.append(spacingBetweenTweens);
        seq.append(LeanTween.scale(hitsTaken.gameObject, Vector3.one, textExpandTime));
        if(gameStats.hitsTaken == 0)
		{
            seq.append(spacingBetweenTweens);
            seq.append(LeanTween.scale(hitsTaken.gameObject, Vector3.one * 1.1f, 0.1f));
            seq.append(NoHitsTaken);
        }

        // all content
        seq.append(spacingBetweenTweens);
        seq.append(LeanTween.scale(allContent.gameObject, Vector3.one, textExpandTime));
        if (gameStats.AllContent)
        {
            seq.append(spacingBetweenTweens);
            seq.append(LeanTween.scale(allContent.gameObject, Vector3.one * 1.2f, 0.1f));
            seq.append(AllContentAchived);
        }
        else
		{
            seq.append(spacingBetweenTweens);
            seq.append(LeanTween.scale(allContent.gameObject, Vector3.one * 0.8f, 0.1f));
            seq.append(AllContentFailed);
        }

        // sword only
        seq.append(spacingBetweenTweens);
        seq.append(LeanTween.scale(swordOnly.gameObject, Vector3.one, textExpandTime));
        if (gameStats.swordOnly)
        {
            seq.append(spacingBetweenTweens);
            seq.append(LeanTween.scale(swordOnly.gameObject, Vector3.one * 1.2f, 0.1f));
            seq.append(SwordOnlyAchived);
        }
        else
        {
            seq.append(spacingBetweenTweens);
            seq.append(LeanTween.scale(swordOnly.gameObject, Vector3.one * 0.8f, 0.1f));
            seq.append(SwordOnlyFailed);
        }

        // magic only
        seq.append(spacingBetweenTweens);
        seq.append(LeanTween.scale(magicOnly.gameObject, Vector3.one, textExpandTime));
        if (gameStats.magicOnly)
        {
            seq.append(spacingBetweenTweens);
            seq.append(LeanTween.scale(magicOnly.gameObject, Vector3.one * 1.2f, 0.1f));
            seq.append(MagicOnlyAchived);
        }
        else
        {
            seq.append(spacingBetweenTweens);
            seq.append(LeanTween.scale(magicOnly.gameObject, Vector3.one * 0.8f, 0.1f));
            seq.append(MagicOnlyFailed);
        }

        seq.append(spacingBetweenTweens);
        seq.append(LeanTween.moveLocalY(stats.gameObject, 200, textExpandTime));

        seq.append(callback);
    }

    private void NoHitsTaken()
	{
        hitsTaken.color = rewardColor;
        audioSource.PlayOneShot(achivedSound);
	}

    private void AllContentAchived()
	{
        allContent.color = rewardColor;
        audioSource.PlayOneShot(achivedSound);
    }
    private void AllContentFailed()
    {
        allContent.color = failColor;
        audioSource.PlayOneShot(failedSound);
    }

    private void SwordOnlyAchived()
    {
        swordOnly.color = rewardColor;
        audioSource.PlayOneShot(achivedSound);
    }
    private void SwordOnlyFailed()
    {
        swordOnly.color = failColor;
        audioSource.PlayOneShot(failedSound);
    }

    private void MagicOnlyAchived()
    {
        magicOnly.color = rewardColor;
        audioSource.PlayOneShot(achivedSound);
    }
    private void MagicOnlyFailed()
    {
        magicOnly.color = failColor;
        audioSource.PlayOneShot(failedSound);
    }
}
