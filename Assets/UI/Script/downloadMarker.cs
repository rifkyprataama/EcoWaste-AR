using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class downloadMarker : MonoBehaviour
{
    // Provide 3 URLs in the inspector
    public string[] markerUrls = new string[3];

    // Optional: names for saved files (without extension)
    public string[] saveNames = new string[3] { "marker1", "marker2", "marker3" };

    // Start download on start (can be triggered from UI instead)
    void Start()
    {
        StartCoroutine(DownloadAllMarkers());
    }

    IEnumerator DownloadAllMarkers()
    {
        int count = Mathf.Min(markerUrls.Length, 3);
        for (int i = 0; i < count; i++)
        {
            if (!string.IsNullOrEmpty(markerUrls[i]))
                yield return StartCoroutine(DownloadAndSave(markerUrls[i], saveNames.Length > i ? saveNames[i] : "marker" + (i+1)));
        }
    }

    IEnumerator DownloadAndSave(string url, string saveName)
    {
        // If URL is a Google Drive sharing link, convert to a direct download link
        if (!string.IsNullOrEmpty(url) && url.Contains("drive.google.com"))
        {
            string id = null;
            // try to extract id from common patterns
            var m = Regex.Match(url, "/d/([a-zA-Z0-9_-]+)");
            if (m.Success)
                id = m.Groups[1].Value;
            else
            {
                m = Regex.Match(url, "[?&]id=([a-zA-Z0-9_-]+)");
                if (m.Success) id = m.Groups[1].Value;
            }

            if (!string.IsNullOrEmpty(id))
                url = "https://drive.google.com/uc?export=download&id=" + id;
        }

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (uwr.result != UnityWebRequest.Result.Success)
#else
            if (uwr.isNetworkError || uwr.isHttpError)
#endif
            {
                Debug.LogError("Failed to download marker: " + url + " Error: " + uwr.error);
                yield break;
            }

            Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
            if (tex == null)
            {
                Debug.LogError("Downloaded texture is null: " + url);
                yield break;
            }

            byte[] pngData = tex.EncodeToPNG();
            if (pngData == null)
            {
                Debug.LogError("Failed to encode texture to PNG: " + url);
                yield break;
            }

            string path = Path.Combine(Application.persistentDataPath, saveName + ".png");
            try
            {
                File.WriteAllBytes(path, pngData);
                Debug.Log("Saved marker to: " + path);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save marker: " + e.Message);
            }
        }
    }
}
