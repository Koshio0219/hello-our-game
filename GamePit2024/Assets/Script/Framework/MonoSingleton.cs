using UnityEngine;

namespace Game.Framework
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        private static readonly object locker = new object();

        private static bool bAppQuitting;

        public static T Instance
        {
            get
            {
                if (bAppQuitting)
                {
                    instance = null;
                    return instance;
                }

                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            Debug.LogError($"singleton は匯つしかないはずだ��" +
                                $"{instance.gameObject.name}というgameObjectを���砲垢戮�だ��");
                            return instance;
                        }

                        if (instance == null)
                        {
                            var singleton = new GameObject();
                            instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton)" + typeof(T);
                            singleton.hideFlags = HideFlags.None;
                            DontDestroyOnLoad(singleton);
                        }
                        else
                        {
                            DontDestroyOnLoad(instance.gameObject);
                        }
                    }
                    instance.hideFlags = HideFlags.None;
                    return instance;
                }
            }
        }

        private void Awake()
        {
            bAppQuitting = false;
        }

        private void OnDestroy()
        {
            bAppQuitting = true;
        }
    }
}

