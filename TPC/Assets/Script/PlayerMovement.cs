using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] float gravety;
    [SerializeField] float jumpForce;
    [SerializeField] float ladderDist;
    [SerializeField] float sensitivity;
    [SerializeField] float speedSprint;
    [SerializeField] float speedWalk;

    private bool isClimbing;
    private bool isJumping;
    private float currSpeed;
    private Quaternion playerTargetRot;
    private Quaternion camTargetRot;
    private Vector3 ladderDir;
    private Vector3 velocity;

    private Effects effects;

    private void Start()
    {
        currSpeed = speedWalk;
        playerTargetRot = transform.localRotation;
        camTargetRot = player.cam.transform.localRotation;

        effects = GameManager.instance.effects;
    }

    private void Update()
    {
        SpeedChange();
        Look();
        Move();
        Gravity();
        Fall();
    }


    private void SpeedChange()
    {
        currSpeed = Input.GetKey(KeyCode.LeftShift) && !isClimbing ? speedSprint : speedWalk;
    }

    private void Look()
    {
        if (isClimbing) return;
        Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * sensitivity;
        float oldYRotation = transform.eulerAngles.y;

        playerTargetRot *= Quaternion.Euler(0f, lookInput.x, 0f);
        camTargetRot *= Quaternion.Euler(-lookInput.y, 0f, 0f);
        camTargetRot = ClampRotationAroundXAxis(camTargetRot);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, playerTargetRot, 5 * Time.deltaTime);
        player.cam.transform.localRotation = Quaternion.Slerp(player.cam.transform.localRotation, camTargetRot, 5 * Time.deltaTime);
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -45, 15);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + player.cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            moveDir = Ladder(moveDir, vertical);
            player.controller.Move(moveDir.normalized * currSpeed * Time.deltaTime);
        }

        SetAnim(direction);
    }

    private void SetAnim(Vector3 direction)
    {
        player.animator.SetFloat("climb", direction.z);
        player.animator.SetBool("isJumping", isJumping);

        if (!isJumping && !isClimbing && player.controller.isGrounded)
        {
            direction *= currSpeed == speedWalk ? 0.5f : 1f;
            player.animator.SetFloat("horrizontal", direction.x);
            player.animator.SetFloat("vertical", direction.z);
        }
    }

    private void Gravity()
    {
        if (isClimbing) return;

        if (player.controller.isGrounded) velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && player.controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravety);
            player.animator.Play("Jump");
            isJumping = true;
        }

        velocity.y += gravety * Time.deltaTime;
        player.controller.Move(velocity * Time.deltaTime);
        if (isJumping && player.controller.isGrounded) isJumping = false;
    }

    private Vector3 Ladder(Vector3 moveDir, float vertical)
    {
        if (!isClimbing)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, moveDir, out RaycastHit hit, ladderDist))
            {
                if (hit.transform.CompareTag("Climb")) GrapLadder(moveDir);
            }
        }
        else
        {
            moveDir.y = vertical;
            moveDir.x = 0;
            moveDir.z = 0;

            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, ladderDir, out RaycastHit hit, ladderDist))
            {
                if (!hit.transform.CompareTag("Climb")) DropLadder();
            }
            else DropLadder();

            if (vertical < 0)
            {
                if ((Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, ladderDist)))
                {
                    isClimbing = false;
                    player.animator.Play("Movement");
                }
            }
        }

        return moveDir;
    }

    private void GrapLadder(Vector3 moveDir)
    {
        ladderDir = moveDir;
        isClimbing = true;
        player.animator.Play("Climb");
    }

    private void DropLadder()
    {
        isClimbing = false;
        Vector3 jumpVelocity = transform.forward;
        jumpVelocity.y = 0.5f;
        player.controller.Move(jumpVelocity);
        player.animator.Play("Movement");
    }

    private void Fall()
    {
        if (velocity.y < -14 && player.controller.isGrounded)
        {
            effects.Fall(transform.position);
        }
    }


    public void WalkSound()
    {

    }
}