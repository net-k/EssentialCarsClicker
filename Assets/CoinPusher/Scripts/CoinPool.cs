using UnityEngine;
using System.Collections.Generic;
using TohoReversi.Effect.TextEffect;

/// <summary>
/// コインなどのGameObjectを再利用してパフォーマンスを向上させるためのシンプルなオブジェクトプール。
/// これにより、InstantiateとDestroyの頻繁な呼び出しを回避します。
/// </summary>
public class CoinPool : MonoBehaviour
{
    /// <summary>
    /// シングルトンインスタンスでどこからでも簡単にアクセスできるようにします。
    /// </summary>
    public static CoinPool Instance { get; private set; }

    // 各プレハブに対応するプールを保持するためのDictionary。
    private Dictionary<GameObject, Queue<GameObject>> _poolDictionary;
    // インスタンスから元のプレハブを検索するためのルックアップ。オブジェクトをプールに戻す際に使用します。
    private Dictionary<GameObject, GameObject> _prefabLookup;

    [SerializeField]
    FloatingTextEffect _floatingTextEffect = null;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
            _prefabLookup = new Dictionary<GameObject, GameObject>();
        }
        else
        {
            // シングルトンパターンを強制するため、既にインスタンスが存在する場合はこのオブジェクトを破棄します。
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 指定されたプレハブのプールを生成します。
    /// </summary>
    /// <param name="prefab">プールするプレハブ。</param>
    /// <param name="initialSize">事前にインスタンス化しておくオブジェクトの数。</param>
    public void CreatePool(GameObject prefab, int initialSize)
    {
        if (prefab == null || _poolDictionary.ContainsKey(prefab))
        {
            return;
        }

        var queue = new Queue<GameObject>();
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.name = prefab.name; // 元の名前を維持
            obj.SetActive(false);
            
            // CoinEffect の Initialize を呼ぶ
            var coinEffect = obj.GetComponent<CoinEffect>();
            if (coinEffect != null)
            {
                coinEffect.Initialize(_floatingTextEffect);
            }
            
            queue.Enqueue(obj);
            _prefabLookup.Add(obj, prefab);
        }
        _poolDictionary.Add(prefab, queue);
    }

    /// <summary>
    /// 指定されたプレハブのオブジェクトをプールから取得します。
    /// </summary>
    /// <param name="prefab">取得するオブジェクトのプレハブ。</param>
    /// <param name="position">オブジェクトを配置する位置。</param>
    /// <param name="rotation">オブジェクトの回転。</param>
    /// <returns>すぐに使用できる状態のGameObject。</returns>
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (!_poolDictionary.ContainsKey(prefab) || _poolDictionary[prefab].Count == 0)
        {
            // プールが存在しないか空の場合、新しいオブジェクトを生成します。
            // これにより、必要に応じてプールが動的に拡張されます。
            obj = Instantiate(prefab, position, rotation);
            obj.name = prefab.name;
            _prefabLookup.Add(obj, prefab); // 新しいオブジェクトも追跡対象に追加

            // プールが存在しなかった場合は、ここで作成します。
            if (!_poolDictionary.ContainsKey(prefab))
            {
                _poolDictionary.Add(prefab, new Queue<GameObject>());
            }
            
            // CoinEffect の Initialize を呼ぶ
            var coinEffect = obj.GetComponent<CoinEffect>();
            if (coinEffect != null)
            {
                coinEffect.Initialize(_floatingTextEffect);
            }
        }
        else
        {
            obj = _poolDictionary[prefab].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
        }
        
        // 物理演算の状態をリセット
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        return obj;
    }

    /// <summary>
    /// オブジェクトをプールに戻します。
    /// </summary>
    /// <param name="obj">プールに戻すオブジェクト。</param>
    public void Return(GameObject obj)
    {
        if (_prefabLookup.TryGetValue(obj, out GameObject prefab))
        {
            if (_poolDictionary.TryGetValue(prefab, out Queue<GameObject> queue))
            {
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            else
            {
                // プールが作られていれば、このケースにはならないはずです。
                Destroy(obj);
            }
        }
        else
        {
            // プールによって作成されなかったオブジェクトの場合、単に破棄します。
            Debug.LogWarning($"オブジェクト {obj.name} はプールによって作成されたものではなかったため、破棄します。");
            Destroy(obj);
        }
    }
}