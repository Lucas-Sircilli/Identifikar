/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : CameraEventListener.h	                                          *
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
#include "CameraController.h"


class CameraEventListener
{
public:
	static EdsError EDSCALLBACK  handleObjectEvent (
						EdsUInt32			inEvent,
						EdsBaseRef			inRef,
						EdsVoid *			inContext				
						)
	{

		CameraController*		controller = (CameraController *)inContext;

		switch(inEvent)
		{
		case kEdsObjectEvent_DirItemRequestTransfer:
				controller->actionPerformed("download", inRef, 0, 0 );
				break;
		
		default:
			//Object without the necessity is released
			if(inRef != NULL)
			{
				EdsRelease(inRef);
			}
            break;
		}

		return EDS_ERR_OK;		
	}	

	static EdsError EDSCALLBACK  handlePropertyEvent (
						EdsUInt32			inEvent,
						EdsUInt32			inPropertyID,
						EdsUInt32			inParam, 
						EdsVoid *			inContext				
						)
	{

		CameraController*		controller = (CameraController *)inContext;

		switch(inEvent)
		{
		case kEdsPropertyEvent_PropertyChanged:
				controller->actionPerformed("get", NULL, inPropertyID, 0);
				break;

		case kEdsPropertyEvent_PropertyDescChanged:
			controller->actionPerformed("getlist", NULL, inPropertyID, 0);
			break;
		}

		return EDS_ERR_OK;		
	}	

	static EdsError EDSCALLBACK  handleStateEvent (
						EdsUInt32			inEvent,
						EdsUInt32			inParam, 
						EdsVoid *			inContext				
						)
	{

		CameraController*		controller = (CameraController *)inContext;

		switch(inEvent)
		{
		case kEdsStateEvent_Shutdown:
				controller->actionPerformed("close");
				break;
		}

		return EDS_ERR_OK;		
	}	

};
