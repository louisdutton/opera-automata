using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CoroutineQueue
    {
        private Queue<IEnumerator> queue;
        private System.Func<IEnumerator, Coroutine> StartCoroutine;
        public IEnumerator active;

        public CoroutineQueue(System.Func<IEnumerator,Coroutine> StartCoroutine)
        {
            queue = new Queue<IEnumerator>();
            this.StartCoroutine = StartCoroutine;
        }

        public void Enqueue(IEnumerator coroutine)
        {
            if (active == null) StartCoroutine(CoroutineManager(coroutine));
            else queue.Enqueue(coroutine);
        }

        public IEnumerator CoroutineManager(IEnumerator coroutine)
        {
            active = coroutine;
            while (coroutine.MoveNext()) yield return coroutine.Current;
            active = null;

            if (queue.Count > 0) Enqueue(queue.Dequeue());

        }
    }

}
