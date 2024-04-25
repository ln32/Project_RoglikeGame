using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Effect_PopGather : MonoBehaviour
{
    public GameObject particleObj;
    public float totalTime;
    public float coef;

    public void GoldEffect_byV3(Vector3 _srcV3, Vector3 _dstV3)
    {
        StartCoroutine(moveBlockTime());

        IEnumerator moveBlockTime()
        {
            ParticleSystem particle = Instantiate(particleObj, transform).GetComponent<ParticleSystem>();

            float lifeTime = totalTime;
            Transform movObj = particle.transform;

            var _particleMain = particle.main;
            _particleMain.startLifetime = totalTime;

            float elapsedTime = 0.0f;
            if (!particle.isPlaying) // 재생할 때
                particle.Play();
            else
                Debug.Log("sad");

            while (true)
            {
                if (elapsedTime > lifeTime) break;

                movObj.position = Vector3.Lerp(_srcV3, _dstV3, Mathf.Pow((elapsedTime / lifeTime), coef));
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            movObj.position = _dstV3;
        }
    }

    public void GoldEffect_byTrans(Transform src, Transform dst)
    {
        StartCoroutine(moveBlockTime());

        IEnumerator moveBlockTime()
        {
            ParticleSystem particle = Instantiate(particleObj, transform).GetComponent<ParticleSystem>();

            float lifeTime = totalTime;
            Transform movObj = particle.transform;

            var _particleMain = particle.main;
            _particleMain.startLifetime = totalTime;

            Vector3 _srcV3 = src.position;
            Vector3 _dstV3 = dst.position;


            float elapsedTime = 0.0f;
            if (!particle.isPlaying) // 재생할 때
                particle.Play();
            else
                Debug.Log("sad");

            while (true)
            {
                if (elapsedTime > lifeTime) break;

                movObj.position = Vector3.Lerp(_srcV3, _dstV3, Mathf.Pow((elapsedTime / lifeTime), coef));
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            movObj.position = _dstV3;
        }
    }
}
