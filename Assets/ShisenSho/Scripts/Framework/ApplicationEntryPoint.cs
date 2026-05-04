
namespace ShisenSho.Framework
{
    public class ApplicationEntryPoint : SingletonMonoBehaviour<ApplicationEntryPoint>
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
