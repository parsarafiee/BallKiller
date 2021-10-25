using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomb : MonoBehaviour
{
    public float explosionRadius;
    //to make a lerp 
    public Vector2 ForceRangeBomb;
    public Vector2 damageRange;
    public float timeToExplode;
    public Gradient colorGradient;
    public SpriteRenderer rend;
    public SpriteRenderer explosionSprite;

    public float explosionFadeTime;
    float timer = 0;
    float fadeTimer = 0;
    bool bombHasExploded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToExplode&& !bombHasExploded)
            Explode();
        rend.color = colorGradient.Evaluate(timer / timeToExplode);
        if (bombHasExploded)
        {
            fadeTimer += Time.deltaTime;
            explosionSprite.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), fadeTimer / explosionFadeTime);
            if (fadeTimer >= explosionFadeTime)
                GameObject.Destroy(this.gameObject);
        }


    }

    void Explode()
    {
        Collider2D[] hitObjectArray = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        for (int i = 0; i < hitObjectArray.Length; i++)
        {
            Rigidbody2D rb = hitObjectArray[i].GetComponent<Rigidbody2D>();
            if (rb)
            {
                Vector2 directionOfForce = (hitObjectArray[i].transform.position - this.transform.position).normalized;

                float distanceBetwenBombAndCenter = Vector3.Distance(hitObjectArray[i].transform.position, transform.position);
                float Interpolator = distanceBetwenBombAndCenter / explosionRadius;
                float forceOfExplition = Mathf.Lerp(ForceRangeBomb.y, ForceRangeBomb.x, Interpolator);

                rb.AddForce(directionOfForce * forceOfExplition, ForceMode2D.Impulse);
                if (rb.CompareTag("Player"))
                {
                    float damageInterpolator = Mathf.Clamp01(Mathf.InverseLerp(ForceRangeBomb.x, ForceRangeBomb.y, forceOfExplition));
                    float dmage = Mathf.Lerp(damageRange.x, damageRange.y, damageInterpolator);
                    MarioMovement player = rb.GetComponent<MarioMovement>();
                    player.TakeDamage(dmage);
                    Debug.Log(dmage);


                }



            }

        }
        explosionSprite.gameObject.SetActive(true);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<CircleCollider2D>().enabled = false;                                                                                
        rend.enabled = false;
        bombHasExploded = true;

       // GameObject.Destroy(this.gameObject);
    }
}
