using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;

public class ResourceManager
{
    Dictionary<string, Object> _resources = new Dictionary<string, Object>();

	public T Load<T>(string key) where T : Object
	{
		if (_resources.TryGetValue(key, out Object resource))
			return resource as T;

		return null;
	}

	public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
	{
		GameObject prefab = Load<GameObject>($"{key}");
		if (prefab == null)
		{
			Debug.Log($"Failed to load prefab : {key}");
			return null;
		}

		//// Pooling
		//if (pooling)
		//	return Managers.Pool.Pop(prefab);

		GameObject go = Object.Instantiate(prefab, parent);
		go.name = prefab.name;
		return go;
	}

	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

		//if (Managers.Pool.Push(go))
		//	return;

		Object.Destroy(go);
	}

	#region ��巹����
	public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
	{
		// ĳ�� Ȯ��.
		if (_resources.TryGetValue(key, out Object resource))
		{
			callback?.Invoke(resource as T);
			return;
		}

		string loadKey = key;
		if (key.Contains(".sprite"))
			loadKey = $"{key}[{key.Replace(".sprite", "")}]";

		// ���ҽ� �񵿱� �ε� ����.
		//var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
		//asyncOperation.Completed += (op) =>
		//{
		//	_resources.Add(key, op.Result);
		//	callback?.Invoke(op.Result);
		//};

		var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey); // ó���� ���, �ε���. ( �񵿱� ������� )
		asyncOperation.Completed += (op) =>
		{

			// ���⼭ Ÿ���� Ȯ���մϴ�.
			//Debug.Log($"key: {key}");
			//Debug.Log($"loadKey: {loadKey}");
			//Debug.Log($"op.Result.GetType: {op.Result.GetType().ToString()}");

			// Texture2D�� �ҷ����� ���, �̸� Sprite�� ��ȯ�մϴ�.
			if (op.Result is Texture2D texture)
			{
				// Texture2D�� Sprite�� ��ȯ�մϴ�.
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

				// ���ҽ��� ��������Ʈ�� �߰��ϰ� �ݹ��� ȣ���մϴ�.
				_resources.Add(key, sprite);
				callback.Invoke(sprite as T);
				return;
			}

			// �Ϲ����� ��� ���ҽ��� �״�� ����մϴ�.
			_resources.Add(key, op.Result);
			callback.Invoke(op.Result);
		};
	}

	public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
	{
		var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
		opHandle.Completed += (op) =>
		{
			int loadCount = 0;
			int totalCount = op.Result.Count;

			foreach (var result in op.Result)
			{
				LoadAsync<T>(result.PrimaryKey, (obj) =>
				{
					loadCount++;
					callback?.Invoke(result.PrimaryKey, loadCount, totalCount);//key���� �ް� count�� ������ ����
				});
			}
		};
	}
	#endregion
}
