using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
public class AdderEditor : EditorWindow
{
	public Font Font;
	public Texture2D Texture;
	public Material Material;
	public AnimatorController Parent;
	public AnimationClip Child;
	[MenuItem("Deko/Adder")]
	static public void Init()
	{
		EditorWindow.GetWindow(typeof(AdderEditor)).Show();
	}
	void OnGUI()
	{
		Font = EditorGUILayout.ObjectField(Font, typeof(Font), false) as Font;
		Texture = EditorGUILayout.ObjectField(Texture, typeof(Texture2D), false) as Texture2D;
		Material = EditorGUILayout.ObjectField(Material, typeof(Material), false) as Material;
		if (GUILayout.Button("Add Font"))
		{
			AddFont();
		}
		if (GUILayout.Button("Extract Font Texture"))
		{
			ExtractFontTexture();
		}
		Parent = EditorGUILayout.ObjectField(Parent, typeof(AnimatorController), false) as AnimatorController;
		Child = EditorGUILayout.ObjectField(Child, typeof(AnimationClip), false) as AnimationClip;
		if (GUILayout.Button("Add Animation"))
		{
			AddAnimations();
		}
		if (GUILayout.Button("Add Selected Animations"))
		{
			AddSelectedAnimations();
		}
	}
	void AddFont()
	{
		var name = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(Font));
		var newTexture = Object.Instantiate(Texture) as Texture2D;
		newTexture.name = name;
		var newMaterial = Object.Instantiate(Material) as Material;
		newMaterial.name = name;
		newMaterial.mainTexture = newTexture;
		Font.material = newMaterial;
		AssetDatabase.AddObjectToAsset(newTexture, Font);
		AssetDatabase.AddObjectToAsset(newMaterial, Font);
		AssetDatabase.SaveAssets();
	}
	void ExtractFontTexture()
	{
		var bytes = Texture.EncodeToPNG();
		File.WriteAllBytes(EditorUtility.SaveFilePanel("Save PNG", Application.dataPath + "/../", "Font", "png"), bytes);
	}
	void AddAnimations()
	{
		var newAnimation = Object.Instantiate(Child) as AnimationClip;
		newAnimation.name = Child.name;
		AssetDatabase.AddObjectToAsset(newAnimation, Parent);
		AssetDatabase.SaveAssets();
	}
	void AddSelectedAnimations()
	{
		var clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Unfiltered);
		if (clips != null && clips.Length > 0)
		{
			foreach (var clip in clips)
			{
				var newAnimation = Object.Instantiate(clip) as AnimationClip;
				newAnimation.name = clip.name;
				AssetDatabase.AddObjectToAsset(newAnimation, Parent);
			}
			AssetDatabase.SaveAssets();
			Selection.objects = new Object[0];
		}
	}
}
