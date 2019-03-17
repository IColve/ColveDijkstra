using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour
{
	private List<Node> allNodes;
	private Dictionary<int, float> disDic = new Dictionary<int, float>();
	private Dictionary<int, List<int>> pathDic = new Dictionary<int, List<int>>();
	private List<int> hasFindList = new List<int>();
	
	public void Find(Node startNode, Node endNode, List<Node> all)
	{
		allNodes = all;
		all.ForEach(x =>
		{
			disDic[x.id] = 999999;
			pathDic[x.id] = new List<int>();
		});
		disDic[startNode.id] = 0;
		pathDic[startNode.id].Add(startNode.id);
		FindNode(startNode);

		string path = "";
		pathDic[endNode.id].ForEach(x => path += x + "_");
		Debug.Log("start:" + startNode.id + " end:" + endNode.id +" path:" + path);
		
		disDic.Clear();
		pathDic.Clear();
		hasFindList.Clear();
	}

	public void FindNode(Node node)
	{
		hasFindList.Add(node.id);
		node.neighborList.ForEach(x =>
		{
			float dis = disDic[node.id] + Vector3.Distance(x.pos, node.pos);
			if (dis < disDic[x.id])
			{
				disDic[x.id] = dis;
				pathDic[x.id].Clear();
				pathDic[node.id].ForEach(y => pathDic[x.id].Add(y));
				pathDic[x.id].Add(x.id);
			}
		});
		Node nearNode = FindNearNode();
		if (nearNode == null)
		{
			return;
		}
		FindNode(nearNode);
	}

	private Node FindNearNode()
	{
		Node node = null;
		float dis = 999999;
		allNodes.ForEach(x =>
		{
			if (!hasFindList.Contains(x.id) && disDic[x.id] < dis)
			{
				node = x;
				dis = disDic[x.id];
			}
		});
		return node;
	}

}
