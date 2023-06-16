using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class VignettePulse : MonoBehaviour
{
    PostProcessVolume m_Volume;
    Vignette m_Vignette;
    void Start()
    {
        // Create an instance of a vignette
        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        m_Vignette.intensity.Override(1f);

        // Use the QuickVolume method to create a volume with a priority of 100, and assign the vignette to this volume
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);

        void Update()
        {
            // Change vignette intensity using a sinus curve
            m_Vignette.intensity.value = Mathf.Sin(Time.realtimeSinceStartup);
        }

        void OnDestroy()
        {
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        }
    }
}







/*using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeTest : MonoBehaviour
{

    private Volume volumeVar;
    private Bloom bloomVar;
    private Vignette vignetteVar;
    // Start is called before the first frame update
    void Start()
    {
        volumeVar = GetComponent<Volume>();
        volumeVar.profile.TryGet(out bloomVar);
        volumeVar.profile.TryGet(out vignetteVar);
    }

    [ContextMenu("test")]
    private void Test()
    {
        bloomVar.scatter.value = 0.1f;
        vignetteVar.intensity.value = 0.5f;
    }

}*/