/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : Event.h	                                                      *
*                                                                             *
*   Description: This is the Sample code to show the usage of EDSDK.          *
*                                                                             *
*                                                                             *
*******************************************************************************
*                                                                             *
*   Written and developed by Camera Design Dept.53                            *
*   Copyright Canon Inc. 2006 All Rights Reserved                             *
*                                                                             *
*******************************************************************************
*   File Update Information:                                                  *
*     DATE      Identify    Comment                                           *
*   -----------------------------------------------------------------------   *
*   06-03-22    F-001        create first version.                            *
*                                                                             *
******************************************************************************/


#pragma once

#include "EDSDK.h"

#define  EVENT_PROGRESS				WM_APP + 1
#define  EVENT_DOWNLOAD_START		WM_APP + 2
#define	 EVENT_DOWNLOAD_COMPLETE	WM_APP + 3
#define	 EVENT_PROPERTY_UPDATE		WM_APP + 4
#define	 EVENT_PROPERTY_LIST_UPDATE	WM_APP + 5
#define	 EVENT_WARNING				WM_APP + 6
#define	 EVENT_ERROR				WM_APP + 7
#define	 EVENT_CLOSE				WM_APP + 8


class Event
{
	
};


class PropertyEvent : public Event
{
	EdsUInt32	_propertyID;
	EdsUInt32	_propertyData;
public:
	PropertyEvent(EdsUInt32 propertyID, EdsUInt32 propertyData)
		: _propertyID(propertyID), _propertyData(propertyData){}

	PropertyEvent(EdsUInt32 propertyID)
		: _propertyID(propertyID){}



	EdsUInt32 getPropertyID() const {return _propertyID;}
	EdsUInt32 getPropertyData() const {return _propertyData;}
};

class ObjectEvent : public Event
{
	EdsBaseRef _object;
public:
	ObjectEvent(EdsBaseRef objecta)
		: _object(objecta){}
	

	EdsBaseRef getObject() const {return _object;}
};

class StateEvent : public Event
{
	EdsUInt32 _eventData;
public:
	StateEvent(EdsUInt32 eventData)
		: _eventData(eventData){}
	

	EdsUInt32 getEventData() const {return _eventData;}
};