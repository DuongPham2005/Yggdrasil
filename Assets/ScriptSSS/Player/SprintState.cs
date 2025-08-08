using UnityEngine;
public class SprintState : State
{
    float gravityValue;
    Vector3 currentVelocity;

    bool grounded;
    bool sprint;
    float playerSpeed;
    bool sprintJump;
    Vector3 cVelocity;
    
    public SprintState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        sprint = false;
        sprintJump = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        playerSpeed = character.sprintSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput()
    {
        base.Enter();
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
        
        // Check if player wants to sprint and has stamina
        if (sprintAction.triggered || input.sqrMagnitude == 0f)
        {
            sprint = false;
        }
        else
        {
            // Only allow sprinting if player has stamina
            if (character.playerStamina != null && character.playerStamina.HasStaminaForSprint)
            {
                sprint = true;
            }
            else
            {
                sprint = false;
                // Force player to stop sprinting if no stamina
                stateMachine.ChangeState(character.standing);
            }
        }
        
        if (jumpAction.triggered)
        {
            sprintJump = true;
        }
    }

    public override void LogicUpdate()
    {
        if (sprint)
        {
            // Use stamina for sprinting
            if (character.playerStamina != null)
            {
                if (!character.playerStamina.UseStaminaForSprint())
                {
                    // If no stamina, stop sprinting
                    sprint = false;
                    stateMachine.ChangeState(character.standing);
                    return;
                }
            }
            
            character.animator.SetFloat("speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);
        }
        else
        {
            stateMachine.ChangeState(character.standing);
        }
        
        if (sprintJump)
        {
            // Check if player has stamina for sprint jump
            if (character.playerStamina != null && character.playerStamina.HasStaminaForJump)
            {
                if (character.playerStamina.UseStaminaForJump())
                {
                    stateMachine.ChangeState(character.sprintjumping);
                }
            }
            else
            {
                // If no stamina for jump, just do normal jump
                stateMachine.ChangeState(character.jumping);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }
        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);

        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }
    }
}