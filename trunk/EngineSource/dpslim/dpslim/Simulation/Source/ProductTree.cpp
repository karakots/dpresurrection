// ProductTree.cpp
//
// Copyright 2006	DecisionPower
//
// Product Tree structure encapsulated
// SSN 3/29/2006
//
// NOTES
// This is meant to be a singleton on the Microsegment class
// so that all conusmers can efficiently traverse the product structre

// only one of me
// I suppose in the future we may have more then one product tree
// but for now we only have one

#include "ProductTree.h"

ProductTree* ProductTree::theTree = new ProductTree();

// nothing doing
ProductTree::ProductTree()
{
}

ProductTree::~ProductTree()
{
	// remove all the lists
	for(size_t ii = 0; ii < children.size(); ++ii)
	{
		delete children[ii];
	}
}

// also reinitializes
void ProductTree::SetSize(int num)
{
	// TODO memory leak here
	children.resize(num);
	leafnodes.resize(num);
	parent.resize(num);

	for(int ii = 0; ii < num; ++ii)
	{
		children[ii] = 0;
		leafnodes[ii] = ii;
		parent[ii] = -1;
	}
}

// creating the tree
// note if child is already a child of a parent then nothing doing
void ProductTree::SetParent(int mom, int child)
{
	// check for index being out of bounds
	if (mom < 0 || mom >= (int)children.size())
		return;

	if (child < 0 || child >= (int)children.size())
		return;

	// check for child having a parent already
	if (parent[child] >= 0)
		return;

	if (children[mom] == 0)
	{
		// first time parent

		// mom should stop being a child
		for(Iter iter = leafnodes.begin(); iter != leafnodes.end(); ++iter)
		{
			if (*iter == mom)
			{
				leafnodes.erase(iter);
				break;
			}
		}

		children[mom] = new List;
	}

	// Add child to parent
	children[mom]->push_back(child);

	// mom is childs parent now
	parent[child] = mom;
}

// returns the leafs of the tree
ProductTree::List* ProductTree::LeafNodes()
{ 
	return &leafnodes;
}

ProductTree::List* ProductTree::ChildNodes(int top)
{
	return children[top];
}

ProductTree::LeafNodeList::LeafNodeList(int top)
{
	GatherLeafs(top);
}
void ProductTree::LeafNodeList::GatherLeafs(int node)
{
	if(theTree->IsLeaf(node))
	{
		this->push_back(node);
		return;
	}

	//Node is not a leaf so...
	for(Iter iter = theTree->children[node]->begin(); iter != theTree->children[node]->end(); ++iter)
	{
		GatherLeafs(*iter);		
	}
}



ProductTree::List* ProductTree::Ancestors(int bottom)
{
	if(ancestors.find(bottom) != ancestors.end())
	{
		return ancestors[bottom];
	}
	else
	{
		List* my_anc = new List();
		int i = bottom;
		my_anc->push_back(i);
		while(Parent(i) >= 0)
		{
			my_anc->push_back(Parent(i));
			i = Parent(i);
		}
		ancestors[bottom] = my_anc;
		return my_anc;
	}
}

int ProductTree::isAncestor(int ancestor, int child)
{
	if(ancestor == child)
	{
		return true;
	}
	if(Parent(child) < 0)
	{
		return false;
	}
	return isAncestor(ancestor, Parent(child));
}

int ProductTree::IsLeaf(int prodnum)
{
	if(children[prodnum] == 0)
	{
		return true;
	}

	return false;
}

ProductTree::List* ProductTree::Siblings(int leafNode)
{
	int parent = Parent(leafNode);

	if (parent < 0)
		return 0;

	List* rval = ChildNodes(parent);

	return rval;
}

// checks if two items have same parent
int ProductTree::Siblings(int a,int b)
{
	if (Parent(a) < 0)
		return false;

	return Parent(a) == Parent(b);
}

int ProductTree::Parent(int child)
{
	return parent[child];
}


// checks if two items are in same forest
int ProductTree::Related(int a,int b)
{
	if (a == b)
		return true;

	if (Parent(a) < 0 && Parent(b) < 0)
		return false;

	if (Parent(a) < 0)
		return Related(a, Parent(b));

	
	if (Parent(b) < 0)
		return Related(b, Parent(a));

	return Related(Parent(a), Parent(b));
}
