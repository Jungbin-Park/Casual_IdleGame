using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float speed;
    Transform target;
    Vector3 targetPos;
    double damage;
    string creatureName;
    bool isHit = false;

    Dictionary<string, GameObject> projectiles = new Dictionary<string, GameObject>();
    Dictionary<string, ParticleSystem> muzzles = new Dictionary<string, ParticleSystem>();

    private void Awake()
    {
        Transform projectile = transform.GetChild(0);
        Transform muzzle = transform.GetChild(1);

        for(int i = 0; i < projectile.childCount; i++)
        {
            projectiles.Add(projectile.GetChild(i).name, projectile.GetChild(i).gameObject);
        }

        for(int i = 0; i < muzzle.childCount; i++)
        {
            muzzles.Add(muzzle.GetChild(i).name, muzzle.GetChild(i).GetComponent<ParticleSystem>());
        }
    }

    public void Init(Transform _target, double _damage, string _creatureName)
    {
        target = _target;
        transform.LookAt(target);
        isHit = false;
        targetPos = target.position;

        damage = _damage;
        creatureName = _creatureName;
        projectiles[creatureName].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isHit) return;

        targetPos.y = 0.5f;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

        if(Vector3.Distance(transform.position, targetPos) <= 0.1f)
        {
            if(target != null)
            {
                isHit = true;
                target.GetComponent<MonsterController>().GetDamage(10);

                projectiles[creatureName].gameObject.SetActive(false);
                muzzles[creatureName].Play();

                StartCoroutine(CoReturnObject(muzzles[creatureName].duration));
            }
        }
    }

    IEnumerator CoReturnObject(float timer)
    {
        yield return new WaitForSeconds(timer);
        Managers.Pool.pools["Bullet"].Push(this.gameObject);
    }

}
