using System.Collections;
using UnityEngine;

public class SpriteWithSFXTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource soundToTriggerSource;
    [SerializeField] private SpriteRenderer SpriteOnTrigger;
    [SerializeField] private float SFXTriggerVolume;
    [SerializeField] private float DelayBetweenClips;
    [SerializeField] private float DelayBetweenAnimations;
    [SerializeField] private float DelayToDestroy;
    [SerializeField] private bool destroyOnDelay;
    [SerializeField] private Animator anim;
    [SerializeField] private Animation[] FadeAnimationTrigger;
    [SerializeField] private AudioClip[] soundToTrigger;
    [SerializeField] private bool isTriggered;
    int count = 1;

    private void Start()
    {
        SFXTriggerVolume = 0.5f;
        SpriteOnTrigger = this.GetComponent<SpriteRenderer>();
        SpriteOnTrigger.enabled = false;
    }

    private void Update()
    {
        if (isTriggered == true && destroyOnDelay == true)
        {
            Destroy(this.gameObject, DelayToDestroy);
            count = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isTriggered = true;

            if (isTriggered && count == 1)
            {
                SpriteOnTrigger.enabled = true;
                StartCoroutine(PlayMultipleSFX());
                count -= 1;
            }
        }
    }
    
    IEnumerator PlayMultipleAnimations()
    {
        anim.Play("Fade_In");
        yield return new WaitForSeconds(DelayBetweenAnimations);
        anim.Play("Fade_Out");
    }

    IEnumerator PlayMultipleSFX()
    {
        soundToTriggerSource.PlayOneShot(soundToTrigger[0], SFXTriggerVolume);
        yield return new WaitForSeconds(DelayBetweenClips);
        soundToTriggerSource.PlayOneShot(soundToTrigger[1], SFXTriggerVolume);
        yield return new WaitForSeconds(DelayBetweenClips);
        soundToTriggerSource.PlayOneShot(soundToTrigger[2], SFXTriggerVolume);
    }

    private void DestroyOnDelay()
    { 
        if (tag == "Player")
        {
            isTriggered = true;
            Destroy(this.gameObject, DelayToDestroy);
            count = 0;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isTriggered = false;
            Destroy(this.gameObject, soundToTrigger[2].length);
            count = 0;
        }
    }

}
