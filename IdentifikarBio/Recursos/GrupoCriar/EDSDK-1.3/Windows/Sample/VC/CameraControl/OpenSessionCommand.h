/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : OpenSessionCommand.h	                                          *
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

#include "Command.h"
#include "Event.h"
#include "EDSDK.h"

class OpenSessionCommand : public Command
{

public:
	OpenSessionCommand(CameraModel *model) : Command(model){}


	// Execute command	
	virtual bool execute()
	{
		EdsError err = EDS_ERR_OK;
	
		//The communication with the camera begins
		err = EdsOpenSession(_model->getCameraObject());


		//Notification of error
		if(err != EDS_ERR_OK)
		{
			_model->notifyObservers(EVENT_ERROR, new StateEvent(err));
		}

		return true;
	}

};

