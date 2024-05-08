using System;
using UnityEngine;

public class RandomTest: MonoBehaviour
{


    // 주어진 평균과 표준 편차에 따라 정규 분포를 따르는 랜덤 값을 생성하여 반환하는 메소드
    public static double GetRandomNumber(double mean, double standardDeviation)
    {
        System.Random random = new System.Random();
        double u1 = 1.0 - random.NextDouble(); // 난수 생성
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); // 정규 분포를 따르는 값 생성
        double randNormal = mean + standardDeviation * randStdNormal; // 평균과 표준 편차 적용

        // 평균 값의 최소 50%, 최대 200%로 값을 제한
        randNormal = Mathf.Clamp((float)randNormal, (float)(mean * 0.5), (float)(mean * 2));

        return randNormal;
    }

    private void Start()
    {
        double mean = 100; // 평균
        double standardDeviation = 10; // 표준 편차

        for (int i = 0; i < 100; i++)
        {
            double randomValue = GetRandomNumber(mean, standardDeviation);
            Debug.Log("랜덤 값: " + randomValue);
        }
    }
}