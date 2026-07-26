using UnityEngine;

public interface IGuardianState
{
    void Enter(GuardianBehaviour guardian);
    void Tick(GuardianBehaviour guardian);
    void Exit(GuardianBehaviour guardian);
}

public class GuardianBlockState : IGuardianState
{
    public void Enter(GuardianBehaviour guardian)
    {
        guardian.Animator?.SetFloat("Speed", guardian.MoveSpeed);
    }

    public void Tick(GuardianBehaviour guardian)
    {
        if (guardian.Player == null) return;

        float distance = Vector3.Distance(guardian.transform.position, guardian.Player.position);

        if (distance > guardian.StopDistance)
        {
            Vector3 direction = (guardian.Player.position - guardian.transform.position);
            direction.y = 0f;
            direction.Normalize();

            guardian.transform.position += direction * guardian.MoveSpeed * Time.deltaTime;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                guardian.transform.rotation = Quaternion.Slerp(guardian.transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }
    }

    public void Exit(GuardianBehaviour guardian)
    {
        guardian.Animator?.SetFloat("Speed", 0f);
    }
}

public class GuardianDeadState : IGuardianState
{
    public void Enter(GuardianBehaviour guardian)
    {
        guardian.Animator?.SetTrigger("Die");
        guardian.OnDefeated();
    }

    public void Tick(GuardianBehaviour guardian) { }
    public void Exit(GuardianBehaviour guardian) { }
}