using UnityEngine;
using System.Collections;
using UnityEditor;

public class TestTextureImporter : AssetPostprocessor {

	void OnPreprocessTexture(){
        TextureImporter textureImporter = assetImporter as TextureImporter;
        textureImporter.isReadable = true;
        //TextureImporterSettings texSettings = new TextureImporterSettings();
        //importer.ReadTextureSettings(texSettings);
        //texSettings.readable = true;
        //importer.SetTextureSettings(texSettings);

    }
}
