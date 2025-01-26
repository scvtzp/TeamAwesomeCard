using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Editor
{
    public class CustomSpriteCutterWithBorder : AssetPostprocessor
    {
        private Object[] textures;

        // Border 설정
        private static Vector4 border = Vector4.zero; // (좌, 우, 하, 상)
        private static Vector4 position = new Vector4(102, 12, 308, 488); // (좌, 우, 하, 상)

        // private void OnPreprocessTexture()
        // {
        //     if (assetPath.EndsWith(".png") && assetPath.Contains("TestAssets/Card/Texture/")) 
        //     {
        //         var factory = new SpriteDataProviderFactories();
        //         factory.Init();
        //         var dataProvider = factory.GetSpriteEditorDataProviderFromObject(assetImporter);
        //         dataProvider.InitSpriteEditorDataProvider();
        //
        //         SetPivot(dataProvider, new Vector2(0.5f, 0.5f));
        //
        //         dataProvider.Apply();
        //     }
        // }
        //
        // static void SetPivot(ISpriteEditorDataProvider dataProvider, Vector2 pivot)
        // {
        //     var spriteRects = dataProvider.GetSpriteRects();
        //     foreach (var rect in spriteRects)
        //     {
        //         rect.alignment = SpriteAlignment.Center;
        //         rect.rect = new Rect(102,12,308,488);
        //         rect.border = border;
        //     }
        //     dataProvider.SetSpriteRects(spriteRects);
        // }
    }
}