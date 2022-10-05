/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : GetPropertyCommand.h	                                          *
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

class GetPropertyCommand : public Command
{
private:
	EdsPropertyID _propertyID;


public:
	GetPropertyCommand(CameraModel *model, EdsPropertyID propertyID)
		:_propertyID(propertyID), Command(model){}


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
		
		//Get property value
		if(err == EDS_ERR_OK)
		{
			err = getProperty(_propertyID);
		}

		//It releases it when locked
		if(lock)
		{
			err = EdsSendStatusCommand(_model->getCameraObject(), kEdsCameraStatusCommand_UIUnLock, 0);
		}


		//Notification of error
		if(err != EDS_ERR_OK)
		{
			// It retries it at device busy
			if(err == EDS_ERR_DEVICE_BUSY)
			{
				_model->notifyObservers(EVENT_WARNING, new StateEvent(err));
				return false;
			}
			
			_model->notifyObservers(EVENT_ERROR, new StateEvent(err) );
		}

		return true;	
	
	}

private:
	EdsError getProperty(EdsPropertyID propertyID)
	{
		EdsError err = EDS_ERR_OK;
		EdsDataType	dataType = kEdsDataType_Unknown;
		EdsUInt32   dataSize = 0;


		if(propertyID == kEdsPropID_Unknown)
		{
			//If unknown is returned for the property ID , the required property must be retrieved again
			if(err == EDS_ERR_OK) err = getProperty(kEdsPropID_AEMode);
			if(err == EDS_ERR_OK) err = getProperty(kEdsPropID_Tv);
			if(err == EDS_ERR_OK) err = getProperty(kEdsPropID_Av);
			if(err == EDS_ERR_OK) err = getProperty(kEdsPropID_ISOSpeed);

			return err;
		}
	
		//Acquisition of the property size
		if(err == EDS_ERR_OK)
		{
			err = EdsGetPropertySize( _model->getCameraObject(),
									  propertyID,
									  0,
									  &dataType,
									  &dataSize );
		}

		if(err == EDS_ERR_OK)
		{
			EdsUInt32 data;
			if(dataType == kEdsDataType_UInt32)
			{
				//Acquisition of the property
				err = EdsGetPropertyData( _model->getCameraObject(),
										propertyID,
										0,
										dataSize,
										&data );

				//Acquired property value is set
				if(err == EDS_ERR_OK)
				{
					_model->setPropertyUInt32(propertyID, data);
				}
			}
			
			if(dataType == kEdsDataType_String)
			{
				
				EdsChar str[EDS_MAX_NAME];
				//Acquisition of the property
				err = EdsGetPropertyData( _model->getCameraObject(),
										propertyID,
										0,
										dataSize,
										str );

				//Acquired property value is set
				if(err == EDS_ERR_OK)
				{
					_model->setPropertyString(propertyID, str);
				}			
			}
		}


		//Update notification
		if(err == EDS_ERR_OK)
		{
			_model->notifyObservers(EVENT_PROPERTY_UPDATE, new PropertyEvent(propertyID) );
		}

		return err;
	}

};