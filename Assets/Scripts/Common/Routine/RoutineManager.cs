using System;
using System.Collections;
using UnityEngine;

namespace Common.Routine
{
    public sealed class RoutineManager : MonoSingleton<RoutineManager>
    {
        protected override void OnAwake()
        {
        }

        public static Coroutine RunWithDelay(Action action, float delay) =>
            Instance.StartCoroutine(CallCoroutine(action, delay));


        public static Coroutine Run(IEnumerator task) => Instance.StartCoroutine(task);


        public static void Stop(Coroutine task) => Instance.StopCoroutine(task);



        private static IEnumerator CallCoroutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}