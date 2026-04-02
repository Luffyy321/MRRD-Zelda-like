/* Author : Raphaël Marczak - 2018/2020, for MIAMI Teaching (IUT Tarbes) and MMI Teaching (IUT Bordeaux Montaigne)
 * 
 * This work is licensed under the CC0 License. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Represents the cardinal directions (South, North, West, East)
public enum CardinalDirections { CARDINAL_S, CARDINAL_N, CARDINAL_W, CARDINAL_E };

public class PlayerBehavior : MonoBehaviour
{
    public float m_speed = 1f; // Speed of the player when he moves
    private CardinalDirections m_direction; // Current facing direction of the player

    public Sprite m_frontSprite = null;
    public Sprite m_leftSprite = null;
    public Sprite m_rightSprite = null;
    public Sprite m_backSprite = null;

    public bool hasKey = false;

    public GameObject messagePrefab = null;
    public GameObject worlds = null;

    public GameObject m_map = null;
    public DialogManager m_dialogDisplayer;

    private Dialog m_closestNPCDialog;

    public PlayerTransform transformScript;

    private Vector2 m_lastDirection = Vector2.up;


    Rigidbody2D m_rb2D;
    SpriteRenderer m_renderer;
    Animator m_animator;


    void Awake()
    {
        m_rb2D = gameObject.GetComponent<Rigidbody2D>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        m_animator = gameObject.GetComponent<Animator>();


        m_closestNPCDialog = null;
    }

    private void Start()
    {
        StartCoroutine(FindDialogManager("https://clementvigier.alwaysdata.net/Zelda_like/getMessage.php"));

    }

    IEnumerator FindDialogManager(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            string message = webRequest.downloadHandler.text;
            string[] ensMessage = message.Split('\n');
            for (int i = 0; i < ensMessage.Length - 1; i++)
            {
                string[] elementMessage = ensMessage[i].Split(';');

                if (elementMessage.Length < 5)
                {
                    Debug.LogWarning("Message mal formaté : " + ensMessage[i]);
                    continue;
                }

                GameObject newMessage = Instantiate(messagePrefab);
                float x = float.Parse(elementMessage[1]);
                float y = float.Parse(elementMessage[2]);
                float z = float.Parse(elementMessage[3]);
                newMessage.transform.position = new Vector3(x, y, z);

                foreach (Transform child in worlds.transform)
                {
                    if (child.gameObject.name == elementMessage[4])
                    {
                        newMessage.transform.parent = child;
                    }
                }
                newMessage.GetComponent<Dialog>().MessageParTerre(elementMessage[0]);


            }

        }
    }

    // This update is called at a very precise and constant FPS, and
    // must be used for physics modification
    // (i.e. anything related with a RigidBody)
    void FixedUpdate()
    {
        // If a dialog is on screen, the player should not be updated
        // If the map is displayed, the player should not be updated
        if (m_dialogDisplayer.IsOnScreen() || m_map.activeSelf)
        {
            return;
        }

        // Moves the player regarding the inputs
        Move();
    }

    private void Move()
    {
        float horizontalOffset = Input.GetAxis("Horizontal");
        float verticalOffset = Input.GetAxis("Vertical");

        // Translates the player to a new position, at a given speed.
        Vector2 newPos = new Vector2(transform.position.x + horizontalOffset * m_speed,
                                     transform.position.y + verticalOffset * m_speed);
        m_rb2D.MovePosition(newPos);


        // ---- ANIMATION ----

        Vector2 movement = new Vector2(horizontalOffset, verticalOffset);

        float speedValue = movement.magnitude;
        m_animator.SetFloat("Speed", speedValue);

        // ---- FLIP SPRITE ----
        if (horizontalOffset > 0.01f)
        {
            m_renderer.flipX = transformScript != null && transformScript.isTransformed ? true : false;
        }
        else if (horizontalOffset < -0.01f)
        {
            m_renderer.flipX = transformScript != null && transformScript.isTransformed ? false : true;
        }

        if (movement.magnitude > 0.01f)
            m_lastDirection = movement.normalized;

        // Computes the player main direction (North, Sound, East, West)
        if (Mathf.Abs(horizontalOffset) > Mathf.Abs(verticalOffset))
        {
            if (horizontalOffset > 0)
            {
                m_direction = CardinalDirections.CARDINAL_E;
            }
            else
            {
                m_direction = CardinalDirections.CARDINAL_W;
            }
        }
        else if (Mathf.Abs(horizontalOffset) < Mathf.Abs(verticalOffset))
        {
            if (verticalOffset > 0)
            {
                m_direction = CardinalDirections.CARDINAL_N;
            }
            else
            {
                m_direction = CardinalDirections.CARDINAL_S;
            }
        }
    }


    // This update is called at the FPS which can be fluctuating
    // and should be called for any regular actions not based on
    // physics (i.e. everything not related to RigidBody)
    private void Update()
    {


        // If the player presses M, the map will be activated if not on screen
        // or desactivated if already on screen
        if (Input.GetKeyDown(KeyCode.M))
        {
            m_map.SetActive(!m_map.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // If a dialog is on screen, the player should not be updated
        // If the map is displayed, the player should not be updated
        if (m_dialogDisplayer.IsOnScreen() || m_map.activeSelf)
        {
            m_animator.SetFloat("Speed", 0f);
            return;
        }

        // ChangeSpriteToMatchDirection();

        // If the player presses SPACE, then two solution
        // - If there is a dialog ready to be displayed (i.e. the player is closed to a NPC)
        //   then a dialog is set to the dialogManager
        // - If not, then the player will shoot a fireball
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_closestNPCDialog != null)
            {
                m_dialogDisplayer.SetDialog(m_closestNPCDialog.GetDialog());
            }
            else
            {
                ThrowSword();
            }
        }
    }

    // Changes the player sprite regarding it position
    // (back when going North, front when going south, right when going east, left when going west)
    private void ChangeSpriteToMatchDirection()
    {
        if (m_direction == CardinalDirections.CARDINAL_N)
        {
            m_renderer.sprite = m_backSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_S)
        {
            m_renderer.sprite = m_frontSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_E)
        {
            m_renderer.sprite = m_rightSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_W)
        {
            m_renderer.sprite = m_leftSprite;
        }
    }

    // Creates a fireball, and launches it
    private void ThrowSword()
    {
        PlayerSword playerSword = GetComponent<PlayerSword>();
        if (playerSword == null || !playerSword.hasSword) return;

        playerSword.ThrowSword(m_lastDirection);
    }


    // This is automatically called by Unity when the gameObject (here the player)
    // enters a trigger zone. Here, two solutions
    // - the player is in an NPC zone, then he grabs the dialog information ready to be
    //   displayed when SPACE will be pressed
    // - the player is in an instantDialog zone, then he grabs the dialog information and
    //   displays it instantaneously
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            m_closestNPCDialog = collision.GetComponent<Dialog>();
        }
        else if (collision.tag == "InstantDialog")
        {
            Dialog instantDialog = collision.GetComponent<Dialog>();
            if (instantDialog != null)
            {
                m_dialogDisplayer.SetDialog(instantDialog.GetDialog());
            }
        }
    }

    // This is automatically called by Unity when the gameObject (here the player)
    // leaves a trigger zone. Here, two solutions
    // - the player was in an NPC zone, then the dialog information is removed
    // - the player was in an instantDialog zone, then the instantDialog is destroyed
    //   (as it has been displayed, and must only be displayed once)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            m_closestNPCDialog = null;
        }
        else if (collision.tag == "InstantDialog")
        {
            Destroy(collision.gameObject);
        }
    }

    public bool IsFacingLeft()
    {
        return m_renderer.flipX;
    }
}
