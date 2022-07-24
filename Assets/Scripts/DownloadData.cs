using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadData : MonoBehaviour
{
    [SerializeField] private string url;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    // Album Data List
    private AlbumData[] _allAlbums;
    [SerializeField] private RawImage loadedImage;

    [System.Serializable]
    public class AlbumData
    {
        public int albumId;
        public int id;
        public string title;
        public string url;
        public string thumbnailUrl;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetAlbums());
        }
    }

    private IEnumerator GetAlbums()
    {
        using (UnityWebRequest getRequest = UnityWebRequest.Get(url))
        {
            yield return getRequest.SendWebRequest();

            if (getRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Successfully Completed the request. Downloaded the Text");

                // var text = getRequest.downloadHandler.text;
                // AlbumData data = JsonUtility.FromJson<AlbumData>(text);
                // textMeshProUGUI.text = data.title;

                _allAlbums = JsonHelper.GetArray<AlbumData>(getRequest.downloadHandler.text);
                StartCoroutine(GetAlbumData());
            }
            else
            {
                Debug.LogError(getRequest.error);
            }
        }
    }

    IEnumerator GetAlbumData()
    {
        var firstData = _allAlbums[0];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(firstData.thumbnailUrl + ".png");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log(request.error);
        else
        {
            // loadedImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;

            loadedImage.texture = DownloadHandlerTexture.GetContent(request);
            Debug.Log("Successfully Completed the request. Image Loaded");
        }
    }
}