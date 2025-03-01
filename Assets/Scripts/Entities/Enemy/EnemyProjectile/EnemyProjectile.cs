using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int damage;
    private float moveSpeed;
    private Vector3 moveDirection;
    private float lifeTime = 5f;

    [Header("Effect Animation Settings:")]
    [SerializeField] private SpriteRenderer abilityEffect;
    [SerializeField] private Sprite[] effectSprites;

    private readonly float animationSpeed = 0.1f;
    private float animationTimer;
    private int currentAnimationIndex;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.GetType().ToString() == Tags.CAPSULECOLLIDER2D)
        {
            collision.gameObject.GetComponent<PlayerHeart>().UpdateCurrentHeart(-damage);
            Despawn();
        }

        if (collision.gameObject.CompareTag("Collisions"))
        {
            Despawn();
        }
    }

    //===========================================================================
    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        // Partical LifeTime
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0.1f)
        {

        }
        if (lifeTime <= 0)
        {
            Despawn();
        }
        ProjectileAnimation();
    }
    private void ProjectileAnimation()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer -= animationSpeed;

            if (currentAnimationIndex == effectSprites.Length)
                currentAnimationIndex = 0;

            abilityEffect.sprite = effectSprites[currentAnimationIndex];
            currentAnimationIndex++;
        }
    }
    private void Despawn()
    {
        lifeTime = 5;
        gameObject.SetActive(false);
        gameObject.transform.localPosition = Vector2.zero;
    }

    //===========================================================================
    public void SetDamage(int newAmount)
    {
        damage = newAmount;
    }
    public void SetMoveDirectionAndSpeed(Vector3 newMoveDirection, float newSpeed)
    {
        moveDirection = newMoveDirection;
        moveSpeed = newSpeed;
        transform.up = moveDirection;
    }
}
