using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class SaveTileAsPNG
{
	[MenuItem("Assets/Save Tile As PNG")]
	private static void SaveSelectedTileAsPNG()
	{
		_SaveTileAsPNG(Selection.activeObject);
	}
	[MenuItem("Assets/Save Tile As PNG", validate = true)]
	private static bool CanSaveSelectedTileAsPNG()
	{
		return Selection.activeObject != null && Selection.activeObject.GetType() == typeof(UnityEngine.Tilemaps.Tile);
	}
	[MenuItem("CONTEXT/TileSet/Save Tile As PNG")]
	private static void ContextSaveSelectedTileAsPNG(MenuCommand menuCommand)
	{
		_SaveTileAsPNG(menuCommand.context);
	}
	[MenuItem("CONTEXT/TileSet/Save Tile As PNG", validate = true)]
	private static bool ContextCanSaveSelectedTileAsPNG(MenuCommand menuCommand)
	{
		return menuCommand.context.GetType() == typeof(UnityEngine.Tilemaps.Tile);
	}
	private static void _SaveTileAsPNG(UnityEngine.Object selectedObject)
	{
		if (selectedObject == null)
			return;
		var selectedTile = selectedObject as UnityEngine.Tilemaps.Tile;
		if (selectedTile == null)
			return;

		var path = EditorUtility.SaveFilePanel(
			"Save tile as PNG",
			"",
			selectedTile.name + ".png",
			"png");

		if (path.Length <= 0)
			return;

		try
		{
			var croppedTexture = GetTextureForTile(selectedTile);
			var pngData = croppedTexture.EncodeToPNG();
			if (pngData != null)
				File.WriteAllBytes(path, pngData);
		}
		catch(Exception ex)
		{
			EditorUtility.DisplayDialog("Error", String.Format("There was an error saving the selected tile to a PNG: {0}", ex.Message), "Okay");
		}
		Debug.Log(String.Format("<color=green>{0} saved successfully!</color>", path));
	}
	private static Texture2D GetTextureForTile(UnityEngine.Tilemaps.Tile tile)
	{
		//var sprite = tile.sprite;
		//var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
		////var tmp = sprite.texture.GetRawTextureData<Color32>();
		////Texture2D tmpTexture = new Texture2D(sprite.texture.width, sprite.texture.height);
		////tmpTexture.LoadRawTextureData(tmp);
		//var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
		//										(int)sprite.textureRect.y,
		//										(int)sprite.textureRect.width,
		//										(int)sprite.textureRect.height);
		//croppedTexture.SetPixels(pixels);
		//croppedTexture.Apply();

		//This works
		return AssetPreview.GetAssetPreview(tile);
	}
}
