using System.Diagnostics;
using System.IO;
using UnityEngine;
using Vuplex.WebView;

public class CesiumMapViewer : MonoBehaviour
{
    [SerializeField] private GameObject _webView;
    private WebViewPrefab _webViewPrefab;
    private Process _process = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        _process = new Process()
        {
            EnableRaisingEvents = false,
            StartInfo = new()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = string.Empty,
                FileName = Path.Combine(Application.streamingAssetsPath, "express-pkg.exe")
            }
        };

        _process.Start();

        print(_process.StartInfo.FileName);
        print(Application.dataPath);
        print(Application.persistentDataPath);

        _webViewPrefab = _webView.GetComponent<WebViewPrefab>();
        await _webViewPrefab.WaitUntilInitialized();
        _webViewPrefab.WebView.MessageEmitted += (sender, eventArgs) => print("JSON received: " + eventArgs.Value);
    }

    private void OnApplicationQuit()
    {
        if (_process != null && !_process.HasExited)
        {
            _process.Kill();
        }
    }
}
