using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AssetBundleManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetAssetBundle());
    }

    IEnumerator GetAssetBundle()
    {
        UnityWebRequest getAbWebRequest = UnityWebRequestAssetBundle.GetAssetBundle("https://www.my-server.com/myData.unity3d");
        yield return getAbWebRequest.SendWebRequest();

        if (getAbWebRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(getAbWebRequest.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(getAbWebRequest);
        }
    }
}