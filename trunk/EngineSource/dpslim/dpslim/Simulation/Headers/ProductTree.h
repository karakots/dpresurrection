#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ProductTree.h
//
// Copyright 2006	DecisionPower
//
// Product Tree structure encapsulated
// SSN 3/29/2006
//
// NOTES
// This is meant to be a singleton used by the consumers
// so that all conusmers can efficiently traverse the product structre
// should be constructed when we read in the the database
#include <vector>
#include <map>
using namespace std;

class ProductTree
{
public:
	// what we call a list
	typedef vector<int> List;
	typedef vector<int>::iterator Iter;

private:
	// not just anyone can create a new tree
	ProductTree();

	// all leaf nodes
	List leafnodes;

	// for each product what its leafnodes are
	vector<List*> children;

	// who belongs to whom
	vector<int> parent;

	map<int,List*> ancestors;

	void AddLeafNodes(List*, int);

public:
	
	// public deletion
	~ProductTree();

	class LeafNodeList : public List
	{
	public:
		LeafNodeList(int);

	private:
		void GatherLeafs(int); 
	};

	// creating the tree

	// first tell how may products in total there are
	// all products are leafs with no parent until told otherwise
	void SetSize(int);

	void SetParent(int, int);

	// querying the tree structure

	// returns the leafs of the tree
	List* LeafNodes();
	List* ChildNodes(int);
	List* Ancestors(int);

	List* Siblings(int);

	//Check if item is a leaf
	int IsLeaf(int);

	// checks if two items are in same forest
	int Siblings(int,int);

	// checks if two items are in same forest
	int Related(int,int);

	// returns parent of child, -1 if no parent exists
	int Parent(int);

	// Checks if item one is a ancestor of item two
	int isAncestor(int, int);

	static ProductTree* theTree;
};
