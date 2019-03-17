using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TestController : MonoBehaviour
{
	public Material red;
	public Material green;
	public Material blue;

	public int startIndex;
	public int endIndex;
	
	
	[ContextMenu("Change")]
	public void ChangePos()
	{
		transform.Find("Nodes").GetComponentsInChildren<Node>().ToList().ForEach(x =>
			{
				x.pos = x.gameObject.transform.position;
			});
	}
	
	[ContextMenu("Connect")]
	public void Connect()
	{
		transform.Find("Nodes").GetComponentsInChildren<Node>().ToList().ForEach(x =>
		{
			x.neighborList.ForEach(y =>
			{
				if (!y.neighborList.Contains(x))
				{
					y.neighborList.Add(x);
				}
			});
			x.id = x.transform.GetSiblingIndex();
		});
	}

	[ContextMenu("DrawLine")]
	public void DrawLine()
	{
		for (int i = transform.Find("Lines").childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(transform.Find("Lines").GetChild(i).gameObject);
		}
		
		for (int i = transform.Find("Words").childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(transform.Find("Words").GetChild(i).gameObject);
		}
		
		List<string> nameList = new List<string>();
		transform.Find("Nodes").GetComponentsInChildren<Node>().ToList().ForEach(x =>
		{
			x.neighborList.ForEach(y =>
			{
				if (!nameList.Contains(y.id + "_" + x.id))
				{
					GameObject line = new GameObject();
					line.transform.SetParent(transform.Find("Lines"));
					line.AddComponent<LineRenderer>();
					line.GetComponent<LineRenderer>().SetPositions(new []{x.pos, y.pos});
					line.GetComponent<LineRenderer>().startWidth = 0.1f;
					line.GetComponent<LineRenderer>().endWidth = 0.1f;
					line.GetComponent<LineRenderer>().material = red;
					line.name = x.id + "_" + y.id;
					nameList.Add(line.name);
				
					GameObject word = new GameObject();
					word.transform.SetParent(transform.Find("Words"));
					word.AddComponent<TextMesh>();
					word.transform.position = x.pos + ((y.pos - x.pos) / 2);
					word.GetComponent<TextMesh>().text = Vector3.Distance(x.pos, y.pos).ToString("F");
					word.GetComponent<TextMesh>().characterSize = 0.1f;
					word.transform.Rotate(new Vector3(90,0,0));
				}
			});
			GameObject nameWord = new GameObject();
			nameWord.transform.SetParent(transform.Find("Words"));
			nameWord.AddComponent<TextMesh>();
			nameWord.transform.position = x.pos;
			nameWord.GetComponent<TextMesh>().text = x.id.ToString();
			nameWord.GetComponent<TextMesh>().characterSize = 0.1f;
			nameWord.GetComponent<TextMesh>().color = Color.red;
			nameWord.transform.Rotate(new Vector3(90,0,0));
		});
	}

	[ContextMenu("FindPath")]
	public void FindPath()
	{
		List<Node> nodes = transform.Find("Nodes").GetComponentsInChildren<Node>().ToList();
		Node startNode = nodes.Find(x => x.id == startIndex);
		Node endNode = nodes.Find(x => x.id == endIndex);
		GetComponent<FindPath>().Find(startNode, endNode, nodes);
	}
}
