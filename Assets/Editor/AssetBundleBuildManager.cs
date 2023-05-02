using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Policy;
using UnityEngine.Networking;

public class AssetBundleBuildManager
{
    [MenuItem("Mytool/AssetBundle Build")]
    public static void AssetBundleBuild()
    {
        string directory = "./Bundle";

        // 이 경로 존재하지 않는다면 만들어줌
        if(Directory.Exists(directory) == false)
        {
            Directory.CreateDirectory(directory);
        }

        // 빌드 옵션과 빌드 타겟을 설정후 빌드
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        EditorUtility.DisplayDialog("에셋 번들 빌드", "에셋 번들 빌드 완료", "완료");
    }
}
