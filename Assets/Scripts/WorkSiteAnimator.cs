using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class WorkSiteAnimator : MonoBehaviour
{

    public GameObject[] walking = new GameObject[10];

    public GameObject[] jumping = new GameObject[10];


    bool animating = false;
    List<Sequence> sequences;

    Vector3[] directions = new Vector3[4] { new Vector3(1, 1), new Vector3(-1, 1), new Vector3(1, -1), new Vector3(-1, -1), };
    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        sequences = new List<Sequence>();
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayAnimation()
    {

        if (!animating)
        {
            animating = true;

            foreach (GameObject obj in walking)
            {
                if (obj != null)
                {
                    //Vector3 dir = directions[random.Next(directions.Length) - 1];
                    Vector3 dir = directions[2];

                    int distance = random.Next(50);
                    float delay = random.Next(100) / 100f;
                    Sequence s = DOTween.Sequence();
                    s.Append(obj.GetComponent<Transform>().DOLocalMove(dir * distance, 2).From(true));
                    s.Append(obj.GetComponent<Transform>().DOLocalMove(dir * (- distance), 2).From(true));
                    s.PrependInterval(delay);
                    s.SetLoops(-1, LoopType.Restart);
                    sequences.Add(s);
                }
            }

            foreach (GameObject obj in jumping)
            {
                if (obj != null)
                {
                    float delay = random.Next(100) / 100f;
                    Sequence s = DOTween.Sequence();
                    s.Append(obj.GetComponent<Transform>().DOJump(obj.GetComponent<Transform>().position, 0.05f, 1, 0.5f));

                    s.PrependInterval(delay);
                    s.SetLoops(-1, LoopType.Yoyo);
                    sequences.Add(s);
                }
            }



        }
    }

    public void StopAnimation()
    {
        if(animating)
        {
            animating = false;

            foreach (Sequence s in sequences)
            {
                s.Kill();
            }

            sequences.Clear();
        }
    }

}
