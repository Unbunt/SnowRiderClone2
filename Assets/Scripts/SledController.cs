using UnityEngine;

public class SledController : MonoBehaviour
{
    [Header("Bewegung")]
    public float sideSpeed = 5f;        // Seitwärtsgeschwindigkeit
    public float forwardSpeed = 10f;    // Automatische Vorwärtsgeschwindigkeit
    public float jumpForce = 6f;        // Sprungkraft

    [Header("Boden-Check")]
    public Transform groundCheck;       // Leeres Objekt unter dem Schlitten
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Rotation einfrieren damit der Schlitten nicht kippt
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Boden-Check (Kugel-Cast nach unten)
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        // Springen mit W
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Seitliche Steuerung: A = links, D = rechts
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal =  1f;

        // Automatisch vorwärts + seitliche Steuerung
        Vector3 move = new Vector3(horizontal * sideSpeed, 0f, forwardSpeed);
        
        // Bestehende Y-Geschwindigkeit (Gravitation/Sprung) beibehalten
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }
}