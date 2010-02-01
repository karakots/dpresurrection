#pragma once
#define _CRT_SECURE_NO_WARNINGS
// EventControl.h
//
// Copyright 2006	DecisionPower
//
// EventControl
//
// Author;	Steven S. Noble
// Date:	9/25/2006


#include <list>

using namespace std;

// A is typeof action object to be stored
// D is typeof object for ordering storage
// SSN 9/25/2006
// usage
// EventControl<ObjectType, DateType> sched;
// sched.push_back(object, date) 
//
//  makes a copy of object
//
// EventControl<ObjectType, DateType>::List* actions = sched.actionsOn(day)
//
// if actions are to be taken on that day
// actions is a list of ObjectTypes active on that day

// maintain lists of actions that occur on a given date
template<class A, class D> class EventControl {

public:
	// we return a ptr to a List of events
	typedef list<A> Actions;

private:

	class Event {
	public:
		Actions actions;
		D date;

		int operator<(const Event& other) const { return date < other.date; }
	};

	typedef list<Event> Sched;

	Sched sched;

public:
	EventControl() {}
	~EventControl()
	{
		sched.clear();
	}

	// return list of action objects active on that date
	Actions* firstActionOnOrBefore(const D& aDate)
	{
		Sched::iterator iter;

		iter = sched.begin();

		if (iter == sched.end())
			return 0;

		
		Event& event = *iter;
		if (event.date == aDate || event.date < aDate)
		{
				return &(event.actions);
		}

		// nothing found
		return 0;
	}

	Actions* actionsOn(const D& aDate)
	{
		Sched::iterator iter;

		for(iter = sched.begin(); iter != sched.end(); ++iter)
		{
			Event& event = *iter;
			if (event.date == aDate)
			{
				return &(event.actions);
			}
		}

		// nothing found
		return 0;
	}

	// sorts list with respect to date object
	void sort() { sched.sort(); }

	// remove actions at date
	void remove(const D& date)
	{
		Sched::iterator iter;

		for(iter = sched.begin(); iter != sched.end();  ++iter)
		{
			Event& event = *iter;
			if (event.date == date)
			{
				sched.erase(iter);
				return;
			}
		}
	}

	// remove first item in sched
	void removeActions()
	{
		Sched::iterator iter = sched.begin();

		if (iter != sched.end())
		{
			sched.erase(iter);
		}
	}

	// Add a new action to occur on a specified date
	void Add(const A& action, const D& aDate) 
	{
		Actions* actions = actionsOn(aDate);

		if(!actions)
		{
			Event dummy;
			sched.push_back(dummy);

			Event& event = sched.back();
			event.date = aDate;
			actions = &(event.actions);
			// sched.sort();
		}
			
		actions->push_back(action);
	}

	// Trim actions to date
	// actions on first date before this date get set to this date
	// this assumes the schedule is sorted
	void trimToDate(const D& aDate)
	{
		// first make sure the actions are sorted
		sort();

		// find first action before date
		list<Event>::iterator curr;
		list<Event>::iterator next;

		curr = sched.begin();

		// special cases
		// nothing in schedule - just leave
		if (curr == sched.end())
		{
			return;
		}

		// there is nothing before aDate
		// set first item to aDate and leave
		if (aDate < (*curr).date)
		{	
			(*curr).date = aDate;
			return;
		}

		next = curr;
		++next;

		// keep iterating until next is at end
		while(next != sched.end())
		{
			// if the next event is past the specified date
			// then we are done
			if (aDate < (*next).date)
			{
				break;
			}

			// erase the beginning of schedule
			sched.erase(sched.begin());

			// set curr to beginning of schedule
			curr = sched.begin();

			// next to next one in schedule
			next = curr;
			++next;
		}
	
		// update the currEvent
		
		(*curr).date = aDate;		
	}
};
