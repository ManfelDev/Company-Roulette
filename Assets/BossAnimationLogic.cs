using UnityEngine;

public class BossAnimationLogic : MonoBehaviour
{

    [SerializeField] private Animator pistolAnimator;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameManager gameManager;

    private bool shootsHimself = false;
    private bool shotIsReal = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }





    public void ShootBoss()
    {
        shootsHimself = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
        shotIsReal = UnityEngine.Random.Range(0, 2) == 1 ? true : false;

        if (shootsHimself && shotIsReal) BossShotBang();
        if (!shootsHimself && shotIsReal) PlayerShotBang();
        if (shootsHimself && !shotIsReal) BossShotNoBang();
        if (!shootsHimself && !shotIsReal) PlayerShotNoBang();
    }

    public void PlayerShotBang()
    {
        bossAnimator.SetTrigger("ShootPlayerBang");
    }

    public void PlayerShotNoBang()
    {
        bossAnimator.SetTrigger("ShootPlayerNoBang");
    }

    public void BossShotBang()
    {
        bossAnimator.SetTrigger("ShootBossBang");
    }

    public void BossShotNoBang()
    {
        bossAnimator.SetTrigger("ShootBossNoBang");
    }

    public void PistolBang()
    {
        pistolAnimator.SetTrigger("ShotBang");

        gameManager.AnimatorBossShot(shootsHimself, shotIsReal);
    }

    public void PistolNoBang()
    {
        pistolAnimator.SetTrigger("ShotNoBang");

        gameManager.AnimatorBossShot(!shootsHimself, shotIsReal);
    }
}
