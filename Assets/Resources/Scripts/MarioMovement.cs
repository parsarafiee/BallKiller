using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Character
{
    Mario, Luigi
}
public class MarioMovement : MonoBehaviour
{
    public Character character;
    //moving variables
    #region private variables
    [Header("movement")]
    public float MoveForce;
    public float Flyforce;
    public float JumpForce;
    public float flipForce;
    public float SecondJumpForce;
    public float rollForce;
    public Vector2 throwDirection;
    public float timeToMaxCharge;
    public Vector2 throwForceRange;
    public float burstForce;
    public float dampener;
    public float SideWayForceBurst;

    #endregion
    [Space(20)]

    public BoxCollider2D groundColi;
    public Transform spriteObject;
    public GameObject boombSprite;
    public GameObject cooldownSprite;
    public UnityEngine.UI.Slider healthSlider;
    [Header("Health")]
    public Vector2 collisionDamageRange = new Vector2(0, 100f);
    public Vector2 collisionForceRange = new Vector2(100f, 500f);
    public float maxHealth = 100;
    float currentHealth;


    Rigidbody2D rb;
    // for ground 
    CapsuleCollider2D Coli;
    bool doubkeJumpAllowed;
    bool isFlipping;

    protected bool isAlive = true;

    protected virtual bool facingRight { get; set; }
    bool flippingRight = true;

    float timeCharged = 0;
    bool isCharging;
    //float velocityOnSin0;
    //coolDwn variables 
    public float bombCooldwon;
    float coolDownTimer = 0;
    bool coolingDown;

    GameObject boombToClone;
    GameObject bombSpritLineMaker;
    public RoundManager roundManager;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        boombToClone = Resources.Load<GameObject>("Prefabs/bomb");
        bombSpritLineMaker = Resources.Load<GameObject>("Prefabs/bombSprite");
        rb = GetComponent<Rigidbody2D>();
        Coli = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProccessInput();
        if (isFlipping)
            CheckToStopFlip();
        if (isCharging)
        {

            timeCharged += Time.deltaTime;

            if (timeCharged <= timeToMaxCharge)
            {

                Vector2 trueThrow_Sprite_Direction = new Vector2(facingRight ? throwDirection.x : -throwDirection.x, throwDirection.y).normalized;
                float throwForce_BombSprite = Mathf.Lerp(throwForceRange.x, throwForceRange.y, Mathf.Clamp01(timeCharged / timeToMaxCharge));
                //Debug.Log(throwForce_BombSprite);
                //Debug.Log(throwForceRange.y);

                float throwForce_BombSprite2 = Mathf.Lerp(facingRight ? 10f : -13f, facingRight ? 118f : -118f, Mathf.Clamp01(timeCharged / timeToMaxCharge));

                GameObject bombSpriteLine = GameObject.Instantiate<GameObject>(bombSpritLineMaker, new Vector3(this.transform.position.x + throwForce_BombSprite2, -23.65f, 0), Quaternion.identity);


                // Debug.Log(throwForce_BombSprite2);

                //  ThrowBombSpriteLine();
            }



        }

        if (coolingDown)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= bombCooldwon)
            {
                coolingDown = false;
                coolDownTimer = 0;
                cooldownSprite.SetActive(true);
            }
        }


    }


    protected virtual void ProccessInput()
    {
    }

    protected void MoveLeft()
    {
        if (facingRight)
            ChangeDirection(false);
        rb.AddForce(Vector2.left * (IsGrounded() ? MoveForce : Flyforce) * Time.deltaTime);
    }

    protected void BurstLeft()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.left * burstForce, ForceMode2D.Impulse);
        }
    }
    protected void DampenLeft()
    {
        // because we maybe in the sky so we dont need to get damping so we add another condition
        if (IsGrounded() && rb.velocity.x < 0)
        {
            rb.AddForce(new Vector3(-rb.velocity.x * dampener, 0, 0), ForceMode2D.Impulse);
        }
    }

    protected void MoveRight()
    {
        if (!facingRight) { }
        ChangeDirection(true);
        rb.AddForce(Vector2.right * (IsGrounded() ? MoveForce : Flyforce) * Time.deltaTime);

    }
    protected void BurstRight()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.right * burstForce, ForceMode2D.Impulse);
        }
    }
    protected void DampenRight()
    {
        // because we maybe in the sky so we dont need to get damping so we add another condition
        if (IsGrounded() && rb.velocity.x > 0)
        {
            rb.AddForce(new Vector3(-rb.velocity.x * dampener, 0, 0), ForceMode2D.Impulse);
        }
    }

    protected void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            doubkeJumpAllowed = true;
            StartFlip(facingRight, flipForce);
        }
        else if (doubkeJumpAllowed)
        {
            rb.AddForce(Vector2.up * SecondJumpForce, ForceMode2D.Impulse);
            doubkeJumpAllowed = false;
            StartFlip(facingRight, flipForce);

        }
    }
    void StartFlip(bool turnRight, float force)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddTorque(turnRight ? -force : +force);
        isFlipping = true;
        flippingRight = turnRight;
    }
    void CheckToStopFlip()
    {

        if (transform.rotation.eulerAngles.z < (flippingRight ? 20 : 360) && transform.rotation.eulerAngles.z > (flippingRight ? 0 : 340))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.identity;
            isFlipping = false;
        }

    }

    bool IsGrounded()
    {
        List<Collider2D> hitList = new List<Collider2D>();
        Coli.OverlapCollider(new ContactFilter2D(), hitList);
        foreach (Collider2D hit in hitList)
        {
            if (hit.CompareTag("ground"))
            {
                return true;
            }

        }
        return false;
    }
    protected void MoveDown()
    {
        rb.AddForce(Vector2.down * (MoveForce / 2) * Time.deltaTime);

    }

    void ChangeDirection(bool faceRight)
    {

        spriteObject.localScale = new Vector3(faceRight ? 1 : -1, 1, 1);
        facingRight = faceRight;
    }

    protected void GroundRoll(bool right)
    {
        if (IsGrounded())
        {
            StartFlip(right, rollForce);
            rb.AddForce((right ? Vector2.right : Vector2.left) * SideWayForceBurst, ForceMode2D.Impulse);
        }

    }

    protected void StartChargingBamb()
    {
        if (!coolingDown)
        {

            boombSprite.SetActive(true);
            isCharging = true;
            cooldownSprite.SetActive(false);
        }

    }

    protected void ThrowBomb()
    {
        if (!coolingDown)
        {
            boombSprite.SetActive(false);

            isCharging = false;
            GameObject boombObj = GameObject.Instantiate<GameObject>(boombToClone, boombSprite.transform.position, Quaternion.identity);
            Rigidbody2D boombRb = boombObj.GetComponent<Rigidbody2D>();
            Vector2 trueThrowDirection = new Vector2(facingRight ? throwDirection.x : -throwDirection.x, throwDirection.y).normalized;
            float throwForce = Mathf.Lerp(throwForceRange.x, throwForceRange.y, Mathf.Clamp01(timeCharged / timeToMaxCharge));


            //part of calculation for boombsprite//get velocity on v0sin0
            // velocityOnSin0 = Mathf.Sin(trueThrowDirection.y) * throwForce;

            //end 

            boombRb.AddForce(transform.TransformDirection(trueThrowDirection) * throwForce, ForceMode2D.Impulse);
            timeCharged = 0;

            coolingDown = true;
        }
    }


    ///make a line to see where the bomb will fall down
    //protected void ThrowBombSpriteLine()
    //{
    //    GameObject bombSpriteLine = GameObject.Instantiate<GameObject>(bombSpritLineMaker, boombSprite.transform.position, Quaternion.identity);
    //    Rigidbody2D boombSpriteRb = bombSpriteLine.GetComponent<Rigidbody2D>();
    //    Vector2 trueThrow_Sprite_Direction = new Vector2(facingRight ? throwDirection.x : -throwDirection.x, throwDirection.y).normalized;
    //    float throwForce_BombSprite = Mathf.Lerp(throwForceRange.x, throwForceRange.y, Mathf.Clamp01(timeCharged / timeToMaxCharge));
    //    boombSpriteRb.AddForce(transform.TransformDirection(trueThrow_Sprite_Direction) * throwForce_BombSprite, ForceMode2D.Impulse);

    //}

    float calculateTimeTofall(float time)
    {


        return 0;
    }

    public void TakeDamage(float amountDamage)
    {
        if (isAlive)
        {
            // Debug.Log(amountDamage);
            currentHealth -= amountDamage;
            healthSlider.value = Mathf.Clamp01(currentHealth / maxHealth);
            if (currentHealth <= 0)
            {
                Die();
            }

        }
    }

    protected void Die()
    {
        spriteObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        rb.constraints = RigidbodyConstraints2D.None;
        boombSprite.SetActive(false);
        cooldownSprite.SetActive(false);
        isAlive = false;
        roundManager.RoundEnds(character);



    }

    public void OnCollisionEnter2D(Collision2D collision)
    {   //force = how hard we hit
        float hitForce = collision.contacts[0].normalImpulse;

        if (hitForce > collisionForceRange.x)
        {
            float interpolator = Mathf.Clamp01(Mathf.InverseLerp(collisionForceRange.x, collisionForceRange.y, hitForce));
            float damage = Mathf.Lerp(collisionDamageRange.x, collisionDamageRange.y, interpolator);
            TakeDamage(damage);
        }


    }
}
