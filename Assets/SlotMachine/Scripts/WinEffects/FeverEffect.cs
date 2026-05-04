using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachine.Scripts;
using SlotMachine;

public class FeverEffect : MonoBehaviour, ISlotWinEffect
{
    [Header("Fever Settings")]
    [Tooltip("フィーバー中に表示するテキスト")]
    public string feverText = "Fever";
    [Tooltip("フィーバーの継続時間（秒）")]
    public float feverDuration = 10.0f;
    [Tooltip("フィーバー中に落とすコインの総数")]
    public int totalCoinsToDrop = 100;
    [Tooltip("フィーバーテキストを表示する時間（秒）")]
    public float textDisplayDuration = 3.0f;

    [Header("UI References")]
    [Tooltip("フィーバーテキストを表示するUI Text")]
    public Text feverTextUI;
    [Tooltip("画面を暗くするためのパネル（UIで暗くする場合に使用）")]
    public Image darkPanel;

    [Header("Light Settings")]
    [Tooltip("通常時のメインライト（フィーバー中に暗くするため）")]
    public Light mainLight;
    [Tooltip("フィーバー中のメインライトの明るさ")]
    public float feverLightIntensity = 1.0f;

    [Tooltip("ディスコライトとして使用するライト")]
    public Light[] discoLights;
    [Tooltip("フィーバー中に点灯させるライトの最大数（処理落ち対策）")]
    public int maxActiveLights = 3;
    [Tooltip("フィーバー中のディスコライトの明るさ")]
    public float discoLightIntensity = 5.0f;
    [Tooltip("フィーバー中のライトの下向き角度")]
    public float feverLightTiltAngle = 45.0f;
    [Tooltip("ライトの色変化の速度")]
    public float lightChangeSpeed = 1.0f;
    [Tooltip("ライトの回転速度")]
    public float lightRotateSpeed = 90.0f;
    
    [Header("Light Movement Settings")]
    [Tooltip("ライトを揺らす（往復移動させる）かどうか")]
    public bool enableLightMovement = true;
    [Tooltip("ライトの移動範囲（角度）")]
    public float lightMoveAngle = 30.0f;
    [Tooltip("ライトの移動速度")]
    public float lightMoveSpeed = 2.0f;

    [Header("Debug Settings")]
    [Tooltip("デバッグ用のキー（例: F）")]
    public KeyCode debugKey = KeyCode.F;

    private CoinSpawner coinSpawner;
    private EffectsManager effectsManager;
    private bool isFeverActive = false;
    private float originalLightIntensity;
    private Color originalLightColor;
    
    // ライトの初期状態を保存するための配列
    private Quaternion[] originalLightRotations;
    private float[] originalDiscoLightIntensities;

    // Start is called before the first frame update
    void Start()
    {
        coinSpawner = FindObjectOfType<CoinSpawner>();
        effectsManager = FindObjectOfType<EffectsManager>();
        
        if (darkPanel != null)
        {
            darkPanel.gameObject.SetActive(false);
        }
        if (feverTextUI != null)
        {
            feverTextUI.gameObject.SetActive(false);
        }
        
        // ライトの初期状態保存
        if (discoLights != null && discoLights.Length > 0)
        {
            originalLightRotations = new Quaternion[discoLights.Length];
            originalDiscoLightIntensities = new float[discoLights.Length];
            for (int i = 0; i < discoLights.Length; i++)
            {
                if (discoLights[i] != null)
                {
                    discoLights[i].gameObject.SetActive(false);
                    originalLightRotations[i] = discoLights[i].transform.localRotation;
                    originalDiscoLightIntensities[i] = discoLights[i].intensity;
                }
            }
        }

        // メインライトが設定されていない場合、Directional Lightを探してみる
        if (mainLight == null)
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light l in lights)
            {
                if (l.type == LightType.Directional)
                {
                    mainLight = l;
                    break;
                }
            }
        }

        if (mainLight != null)
        {
            originalLightIntensity = mainLight.intensity;
            originalLightColor = mainLight.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFeverActive)
        {
            UpdateDiscoLights();
        }

        // デバッグキー入力のチェック
        if (Input.GetKeyDown(debugKey))
        {
            StartFever();
        }
    }

    public bool IsApplicable(SlotValue symbol, int score)
    {
        // 'seven' が揃った時に実行 (Feverなので777を想定)
        return symbol == SlotValue.seven;
    }

    public void Execute(SlotValue symbol, int score)
    {
        StartFever();
    }

    public void StartFever()
    {
        if (!isFeverActive)
        {
            StartCoroutine(FeverRoutine());
        }
    }

    private IEnumerator FeverRoutine()
    {
        isFeverActive = true;

        // UI表示
        if (darkPanel != null)
        {
            darkPanel.gameObject.SetActive(true);
        }
        if (feverTextUI != null)
        {
            feverTextUI.text = feverText;
            feverTextUI.gameObject.SetActive(true);
            
            // 指定時間後にテキストを非表示にするコルーチンを開始
            StartCoroutine(HideTextAfterDelay(textDisplayDuration));
        }

        // メインライトを暗くする
        if (mainLight != null)
        {
            mainLight.intensity = feverLightIntensity;
        }

        // ディスコライト点灯
        int activeCount = 0;
        for (int i = 0; i < discoLights.Length; i++)
        {
            if (discoLights[i] != null)
            {
                // 最大数を超えたら点灯させない
                if (activeCount < maxActiveLights)
                {
                    discoLights[i].gameObject.SetActive(true);
                    discoLights[i].intensity = discoLightIntensity; // 明るさ設定
                    
                    // ライトを下向きにする
                    Vector3 currentEuler = discoLights[i].transform.localEulerAngles;
                    discoLights[i].transform.localEulerAngles = new Vector3(feverLightTiltAngle, currentEuler.y, currentEuler.z);
                    
                    activeCount++;
                }
                else
                {
                    // 念のため非表示
                    discoLights[i].gameObject.SetActive(false);
                }
            }
        }

        // 壁エフェクトの発生
        if (effectsManager != null)
        {
            effectsManager.triggerBumperEffect();
        }

        // コイン落下処理
        int coinsDropped = 0;
        float timeElapsed = 0f;
        
        // 要望に合わせて「一定の間隔で100枚」を実現するために計算
        float interval = feverDuration / totalCoinsToDrop;

        while (timeElapsed < feverDuration && coinsDropped < totalCoinsToDrop)
        {
            if (coinSpawner != null)
            {
                // ランダムな位置にコインを1枚落とす
                coinSpawner.spawnSingleCoinRandomly();
            }

            coinsDropped++;
            yield return new WaitForSeconds(interval);
            timeElapsed += interval;
        }

        // 残りの時間待機（もしコインが早く落ちきった場合）
        if (timeElapsed < feverDuration)
        {
            yield return new WaitForSeconds(feverDuration - timeElapsed);
        }

        // 終了処理
        isFeverActive = false;

        if (darkPanel != null)
        {
            darkPanel.gameObject.SetActive(false);
        }
        if (feverTextUI != null)
        {
            feverTextUI.gameObject.SetActive(false);
        }
        
        // メインライトを元に戻す
        if (mainLight != null)
        {
            mainLight.intensity = originalLightIntensity;
            mainLight.color = originalLightColor;
        }

        // ディスコライト消灯とリセット
        for (int i = 0; i < discoLights.Length; i++)
        {
            if (discoLights[i] != null)
            {
                discoLights[i].gameObject.SetActive(false);
                // 角度を元に戻す
                discoLights[i].transform.localRotation = originalLightRotations[i];
                // 明るさを元に戻す
                discoLights[i].intensity = originalDiscoLightIntensities[i];
            }
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (feverTextUI != null && isFeverActive)
        {
            feverTextUI.gameObject.SetActive(false);
        }
    }

    private void UpdateDiscoLights()
    {
        // 色の変化
        float hue = Mathf.Repeat(Time.time * lightChangeSpeed, 1.0f);
        Color color = Color.HSVToRGB(hue, 1.0f, 1.0f);

        // メインライトの色も変更
        if (mainLight != null)
        {
            mainLight.color = color;
        }

        for (int i = 0; i < discoLights.Length; i++)
        {
            // アクティブでないライトは処理しない
            if (discoLights[i] == null || !discoLights[i].gameObject.activeSelf) continue;

            discoLights[i].color = color;

            // 回転と揺れの計算
            // Y軸（パン）: 継続的に回転
            float currentY = discoLights[i].transform.localEulerAngles.y;
            float nextY = currentY + lightRotateSpeed * Time.deltaTime;

            // X軸（チルト）: 基準角度を中心に揺らす
            float nextX = feverLightTiltAngle;
            if (enableLightMovement)
            {
                // サイン波で揺らす
                float wave = Mathf.Sin(Time.time * lightMoveSpeed + i * 0.5f) * lightMoveAngle;
                nextX += wave;
            }

            // 上方向（0度未満）に向かないように制限 (0〜90度の範囲にクランプ)
            // 0度=水平, 90度=真下
            nextX = Mathf.Clamp(nextX, 0f, 90f);

            // 適用 (Z軸は0固定)
            discoLights[i].transform.localEulerAngles = new Vector3(nextX, nextY, 0f);
        }
    }
}