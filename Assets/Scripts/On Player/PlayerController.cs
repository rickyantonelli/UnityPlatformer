using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _grounderRadius = 0.2f;
    [SerializeField] private float _grounderOffset = -1;

    [Header("Player Movement")]
    [SerializeField] private float _walkSpeed = 4;
    [SerializeField] private float _acceleration = 2;
    [SerializeField] private float _currentMovementLerpSpeed = 100;
    [SerializeField] private float _dashSpeed = 15;
    [SerializeField] private float _dashLength = 1;
    [SerializeField] private float _coyoteTime = 0.2f;
    [SerializeField] private float _fallMultiplier = 7;
    [SerializeField] private float _jumpVelocityFalloff = 8;

    public float slidingSpeed = -10, jumpForce = 6, cooldownTime = 1f, airMovement = 1f, cooldownTimeJump = 0.1f, dashYvelo = 3, earlyJumpBuffer = 0.1f;

    [Header("Ball Attributes")]
    [SerializeField] private float ballSpeed = 10.0f;
    public float ballOffset = 2;

    [Header("For Bug Testing")]
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private GameObject checkPoint;
    public Collider2D[] _ground = new Collider2D[1];
    public GameObject Ball, Player1, Player2, player;
    [SerializeField] private bool isAlive = true;
    public bool ballMoving, ballColliding, equipped, isGrounded, canDash, hasDashed = false, canParent = true, dashUnlocked = true;
    public float horizontal, vertical, playerVelocity, caughtTime, jumpedTime;
    public Animator animator;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false, passed = false, sceneReset = false, dashed = false, _facingLeft, dashing, coyoteCheck, canJump = true, _hasDashed, _dashing, m_FacingRight = true, canEarlyJump = false;
    private float _timeStartedDash, _timeLeftGrounded, earlyJumpTime;
    private Vector2 _dashDir;
    Rigidbody2D _rigidbody;
    CapsuleCollider2D _capsulecollider;
    SpriteRenderer _spriterenderer;
    private FrameInputs _inputs;
    

    [Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsulecollider = GetComponent<CapsuleCollider2D>();
        _spriterenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ballMoving = false; //ensure at the start that the ball isnt moving 
        if(GameObject.Find("Player1") == null)
        {
            transform.position = checkPoint.transform.position;
            gameObject.name = "Player1";
            //Debug.Log(_spriterenderer.material.color);
            //_spriterenderer.material.color = Color.red;
        }
        else
        {
            transform.position = checkPoint.transform.position;
            gameObject.name = "Player2";
            //_spriterenderer.material.color = Color.black;
        }

        Ball = GameObject.Find("Ball(Clone)");
    }

    public void OnMove(InputAction.CallbackContext context) //for movement on controller
    {
        SetVelocity(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context) // for jumping on controller
    {
        jumped = context.action.triggered;
        /*if (!isGrounded)
        {
            earlyJumpTime = Time.time;
            canEarlyJump = true;
        }*/
    }

    public void OnPassBall(InputAction.CallbackContext context) // for passing ball on controller
    {
        passed = context.action.triggered;
    }
    
    public void OnReset(InputAction.CallbackContext context) // for resetting the game in case something blows up
    {
        sceneReset = context.action.triggered;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        dashed = context.action.triggered;
    }


    public void SetVelocity(Vector2 direction)
    {
        horizontal = direction.x;
        vertical = direction.y;
    }

    private void ExecuteThrow(GameObject catcher)
    {
        float step = ballSpeed * Time.deltaTime;
        if(ballMoving == true)
            {           
            Ball.transform.position = Vector2.MoveTowards(Ball.transform.position, catcher.transform.position, step);
            if(Ball.transform.position == catcher.transform.position) 
            {
            Ball.transform.SetParent(catcher.transform);
            Ball.transform.position = new Vector2(Ball.transform.position.x, Ball.transform.position.y + ballOffset);
            canParent = true;
            ballColliding = false;
            ballMoving = false;
            caughtTime = Time.time;
            }
            }
    }

    private void ThrowBall()
    {
        if(gameObject.name == "Player1")
        {
            ExecuteThrow(Player2);
        }
        if(gameObject.name == "Player2")
        {
            ExecuteThrow(Player1);
        }
    }


    void Update()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        
        if(isAlive)
        {
            groundedCheck();        
            WalkingHandler();
            JumpingHandler();
            DashingHandler();
            PassBall();
            ResetScene();
        }
    }

    void PassBall()
    {
        if (passed && Ball.transform.parent == player.transform && Time.time > caughtTime + cooldownTime)  //for when you pass the ball - change bools and issue ThrowBall method
        {
            ballMoving = true;
            ballColliding = true;
            equipped = false;
            canDash = false;
            canParent = false;
            Ball.transform.SetParent(null);
            //_spriterenderer.material.color = Color.white;
        }
        if (ballMoving) ThrowBall();
    }

    void ResetScene()
    {
        if(sceneReset) 
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }



    private void OnTriggerEnter2D(Collider2D other) //when player collides with ball - no physics
    {
        if(other.gameObject == Ball && canParent == true)
        {


            if(Ball.transform.parent == null)
            {
                equipped = true;
                canDash = true;
                hasDashed = false;
                Ball.transform.position = transform.position;
                Ball.transform.SetParent(player.transform);
                Ball.transform.position = new Vector2(Ball.transform.position.x, Ball.transform.position.y + ballOffset);
                //Debug.Log(Ball.transform.parent);
                //ballMoving = false;
                caughtTime = Time.time;
                //_spriterenderer.material.color = Color.red;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) //when players collide we are setting their velocity to 0 so they dont "stick" to the walls for a bit as their velocity is still not countered out by gravity
    {
        if(_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        }
    }
    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Slope")
        {
            _rigidbody.velocity = new Vector3 (_rigidbody.velocity.x, slidingSpeed, 0);
        }
    }

    //private void DrawGrounderGizmos() {
    //    Gizmos.color = Color.red;
     //   Gizmos.DrawWireSphere(transform.position + new Vector3(0, _grounderOffset), _grounderRadius);
    //}
    //private void OnDrawGizmos() 
    //{
    //    DrawGrounderGizmos();
    //}

    private void groundedCheck()
    {
        
        isGrounded = Physics2D.OverlapCircleNonAlloc(transform.position + new Vector3(0, _grounderOffset), _grounderRadius, _ground, platformLayerMask) > 0;
        
        if(isGrounded)
        {
            coyoteCheck = true;
            /*if (Time.time - earlyJumpTime < earlyJumpBuffer && canEarlyJump)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
                canEarlyJump = false;
                equipped = false;
                jumpedTime = Time.time;
                // this is definitely not finished, the jump is very short, might need to be commented out for a bit - see how Tarodev does it
            }*/
        }
        if(coyoteCheck && !isGrounded)
        {
            coyoteCheck = false;
            _timeLeftGrounded = Time.time;
        }
    }    

    private void WalkingHandler()
    {
        //animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (horizontal < 0) {
            if (_rigidbody.velocity.x > 0) _inputs.X = 0; // Immediate stop and turn. Just feels better
            _inputs.X = Mathf.MoveTowards(_inputs.X, horizontal, _acceleration * Time.deltaTime);
        }
        else if (horizontal > 0) {
            if (_rigidbody.velocity.x < 0) _inputs.X = 0;
            _inputs.X = Mathf.MoveTowards(_inputs.X, horizontal, _acceleration * Time.deltaTime);
        }
        else {
            _inputs.X = Mathf.MoveTowards(_inputs.X, 0, _acceleration * 2 * Time.deltaTime);
        }

        var idealVel = new Vector3(_inputs.X * _walkSpeed, _rigidbody.velocity.y);
        // _currentMovementLerpSpeed should be set to something crazy high to be effectively instant. But slowed down after a wall jump and slowly released
        _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, idealVel, _currentMovementLerpSpeed * Time.deltaTime);

        if(horizontal < 0 && m_FacingRight)
        {
            FlipPlayer();
        }
        if(horizontal > 0 && !m_FacingRight)
        {
            FlipPlayer();
        }

        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);       
    }

    private void JumpingHandler()
    {
        if (jumped && canJump && isGrounded && Time.time > jumpedTime + cooldownTimeJump || jumped && canJump && Time.time > jumpedTime + cooldownTimeJump && Time.time < _timeLeftGrounded + _coyoteTime) //normal jump when player is on the ground
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
            equipped = false;
            jumpedTime = Time.time;
            //animator.SetBool("IsJumping", true);
            //Debug.Log("jumped");
        }

        if (jumped && canJump && equipped && Time.time > jumpedTime + cooldownTimeJump) //jump reset after you've caught the ball while in mid air
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
            equipped = false;
            jumpedTime = Time.time;
            //animator.SetBool("IsJumping", true);
        }

        if (_rigidbody.velocity.y < _jumpVelocityFalloff || _rigidbody.velocity.y > 0 && !jumped)
            _rigidbody.velocity += _fallMultiplier * Physics2D.gravity.y * Vector2.up * Time.deltaTime;

        //if(jumpedTime + 0.1 <Time.time && isGrounded)
        //{
        //    OnLanding();
        //}      
    }
    private void DashingHandler()
    {
        if(dashed && dashUnlocked && canDash &&!hasDashed)
        {
            //Debug.Log("dash");
            _dashDir = new Vector2(horizontal, vertical).normalized;
            if (_dashDir == Vector2.zero) _dashDir = _facingLeft ? Vector2.left : Vector2.right;
            dashing = true;
            _timeStartedDash = Time.time;
            //_rigidbody.gravityScale = 0;
            hasDashed = true;
        }

        if(dashing)
        {
            canJump = false;
            _rigidbody.velocity = _dashDir * _dashSpeed;
            //animator.SetBool("IsDashing", true);
            if (Time.time >= _timeStartedDash + _dashLength) 
            {
                dashing = false;
                canJump = true;
                // Clamp the velocity so they don't keep shooting off
                //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
                 _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y > dashYvelo ? dashYvelo : _rigidbody.velocity.y);
                //_rigidbody.gravityScale = 3;
                //if (isGrounded) _hasDashed = false;
                //animator.SetBool("IsDashing", false);
            }
        }
    }
    private struct FrameInputs 
    {
        public float X, Y;
        public int RawX, RawY;
    }
    private void GatherInputs() 
    {
        _inputs.RawX = (int) Input.GetAxisRaw("Horizontal");
        _inputs.RawY = (int) Input.GetAxisRaw("Vertical");
        _inputs.X = Input.GetAxis("Horizontal");
        _inputs.Y = Input.GetAxis("Vertical");
        _facingLeft = _inputs.RawX != 1 && (_inputs.RawX == -1 || _facingLeft);
    }

    private void FlipPlayer()
	{
		// Switch the way the player is labeled as facing
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector2 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public void OnLanding()
    {
        //animator.SetBool("IsJumping", false);
    }
}