using System.IO;
using UnityEditor;

public class AssetBundleBuildManager
{
    [MenuItem("Mytool/AssetBundle Build")]
    public static void AssetBundleBuild()
    {
        string directory = "Assets/Bundle";

        // 이 경로 존재하지 않는다면 만들어줌
        if(Directory.Exists(directory) == false)
        {
            Directory.CreateDirectory(directory);
        }

        // 빌드 옵션과 빌드 타겟을 설정후 빌드
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        // 빌드 완료시 뜨는 팝업임
        // 타이틀, 메시지, 버튼에 뜨는 텍스트 
        EditorUtility.DisplayDialog("에셋 번들 빌드", "에셋 번들 빌드 완료", "완료");
    }
}
