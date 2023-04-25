using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public Transform player;
   
    [SerializeField] 
    public float walkspeed;
    public float runspeed;
    public float jumpspeed;
    public float friction;
    private bool doubleJump;
    public float groundRadius;
    public bool grounded;
    public Transform groundCheck;
    private Animator anim;
    private CapsuleCollider2D bc;
    public GameObject open;
    public GameObject closed;
    public int ncoins;
    public LayerMask whatIsGround;
    private bool viradoDireita;
    public bool crouch;

    // Start is called before the first frame update
    private void Start()
    {
        open.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        player = player.transform;
        anim = GetComponent<Animator>();
        bc = GetComponent<CapsuleCollider2D>();
        viradoDireita = true;
        groundRadius = 0.2f;
        crouch = Input.GetKey(KeyCode.LeftControl);
    }

    // Update is called once per frame
    private void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * walkspeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && (crouch || grounded || doubleJump))
        {
            anim.Play("Jump");
            anim.SetBool("Grounded", true);
            if (grounded) doubleJump = false;
            rb.AddForce(Vector2.up * jumpspeed, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = new Vector2((dirX * runspeed), rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("Crouch", true);
            bc.size = new Vector2(0.15f, 0.15f);
            bc.offset = new Vector2(0.0f, -0.05f);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("Crouch", false);
            bc.size = new Vector2(0.2f, 0.3f);
            bc.offset = new Vector2(0.0f, 0.0f);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        anim.SetFloat("Speed", Mathf.Abs(dirX));
        if ((dirX < 0 && viradoDireita) || (dirX > 0 && !viradoDireita)) Flip(); 
        
        if (Coin.totalCoins == ncoins)
            {
                closed.SetActive(false);
                open.SetActive(true);
            }
    }
    void Flip()
    {
        viradoDireita = !viradoDireita;
        Vector3 scale = transform.localScale;
        scale.x *= -1;       //scale.x= scale.x * (-1);
        transform.localScale = scale;
    }

    public void SceneNCoins()
    {
        /*Scene = SceneUtility.GetBuildIndexByScenePath("Scenes/Levels");

        if (scene.name == "Level_1")
        {

        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if(collision.gameObject.CompareTag("Coin"))
        {
            /*Destroy(collision.gameObject);
            ncoins--;
            
        }*/

        if (collision.gameObject.name == ("Spikes"))
        {
            Destroy(player.gameObject);
            player.gameObject.GetComponent<PlayerMovement>().enabled = false;
            SceneManager.LoadScene(sceneName:"DeathScreen", LoadSceneMode.Single);

        }

        if (collision.gameObject.CompareTag("open"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    private void OnDisable()
    {
        if (tag == "Player")
        {
            // Store the current scene index in PlayerPrefs when transitioning to the death screen
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("previousSceneIndex", currentSceneIndex);
            PlayerPrefs.Save();
        }
    }

}