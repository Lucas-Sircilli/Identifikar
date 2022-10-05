/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : SaveSettingCommand.h	                                          *
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

class SaveSettingCommand : public Command
{
private:
	EdsSaveTo _saveTo;

public:
	SaveSettingCommand(CameraModel *model, EdsSaveTo saveTo) :_saveTo(saveTo), Command(model){}


	// Execute command	
	virtual bool execute()
	{
		EdsError err = EDS_ERR_OK;
		bool	 lock = false;
		
		// For cameras earlier than the 30D , the UI must be locked before commands are reissued
		if( _model->isLegacy() == true )
		{
			err = EdsSendStatusCommand(_model->getCameraObject(), kEdsCameraStatusCommand_UILock, 0);
		
			if(err == EDS_ERR_OK)
			{
				lock = true;
			}		
		}

		//It sets preserving ahead
		if(err == EDS_ERR_OK)
		{
			err = EdsSetPropertyData(_model->getCameraObject(), kEdsPropID_SaveTo, 0, sizeof(_saveTo) , &_saveTo);
		}

		//It releases it when locked
		if(lock)
		{
			err = EdsSendStatusCommand(_model->getCameraObject(), kEdsCameraStatusCommand_UIUnLock, 0);
		}


		//Notification of erro
		if(err != EDS_ERR_OK)
		{
			//It retries it at device busy
			if(err == EDS_ERR_DEVICE_BUSY)
			{
				_model->notifyObservers(EVENT_WARNING, new StateEvent(err) );
				Sleep(500);
				return false;
			}
			
			_model->notifyObservers(EVENT_ERROR, new StateEvent(err) );
		}

		return true;
	}


};