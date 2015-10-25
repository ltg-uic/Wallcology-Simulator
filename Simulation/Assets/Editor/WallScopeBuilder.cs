using UnityEditor;

public class WallScopeBuilder
{

    static void BuildWallScope( string id)
    {
        string buildPath = "/Users/krbalmryde/Dropbox/Code-Projects/LTG/wallcology/interfaces/visualization/build/wallscope" + id ;
        string[] scene = { "Assets/Scenes/Habitat" + id + ".unity" };
        BuildPipeline.BuildPlayer( scene, buildPath, BuildTarget.WebPlayer, BuildOptions.None);
    }

    static void BuildWallScopes()
    {
        string[] sceneIDnumbers = { "1", "2", "3", "4" };
        foreach( string id in sceneIDnumbers )
        {
            BuildWallScope(id);
        }
    }
}