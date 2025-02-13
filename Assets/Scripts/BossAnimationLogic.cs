using UnityEngine;

public class BossAnimationLogic : MonoBehaviour
{

    [SerializeField] private Animator pistolAnimator;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameManager gameManager;

    private bool shootsHimself = false;
    private bool shotIsReal = false;

    [SerializeField] private AudioClip[] shootingSound;
    [SerializeField] private AudioClip[] noBulletShootingSound;






    public void ShootBoss()
    {
        shootsHimself = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
        shotIsReal = UnityEngine.Random.Range(0, 2) == 1 ? true : false;

        Debug.Log("shootsHimself: " + shootsHimself);
        Debug.Log("shotIsReal: " + shotIsReal);

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

        gameManager.AnimatorBossShot(!shootsHimself, shotIsReal);
        GlobalAudioSystem.Instance.PlaySound(shootingSound[Random.Range(0, shootingSound.Length - 1)], gameObject.transform.position);

    }

    public void PistolNoBang()
    {
        pistolAnimator.SetTrigger("ShotNoBang");

        gameManager.AnimatorBossShot(!shootsHimself, shotIsReal);
        GlobalAudioSystem.Instance.PlaySound(noBulletShootingSound[Random.Range(0, noBulletShootingSound.Length - 1)], gameObject.transform.position);

    }


}
