/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : CameraController.h	                                          *
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
#include "CameraModel.h"
#include "Processor.h"

#include "OpenSessionCommand.h"
#include "CloseSessionCommand.h"
#include "SaveSettingCommand.h"
#include "TakePictureCommand.h"
#include "DownloadCommand.h"
#include "GetPropertyCommand.h"
#include "GetPropertyDescCommand.h"
#include "SetPropertyCommand.h"
#include "SetCapacityCommand.h"


class CameraController 
{

protected:
	// Camera model
	CameraModel* _model;
	
	// Command processing
	Processor _processor;

public:
	// Constructor
	CameraController(): _model(){}

	// Destoracta
	virtual ~CameraController(){}


	void setCameraModel(CameraModel* model) {_model = model;}

	//Execution beginning
	void run()
	{
		_processor.start();
	}

public:

	void actionPerformed(std::string strEvent, void *arg1 = NULL, EdsUInt32 arg2 = 0, EdsUInt32 arg3 = 0)
	{
		if(  strEvent == "init")
		{

			//The communication with the camera begins
			StoreAsync(new OpenSessionCommand(_model));

			//Preservation ahead is set to PC
			StoreAsync(new SaveSettingCommand(_model, kEdsSaveTo_Host));
			
			//Setting of PC capacity
			SetCapacityCommand* command = new SetCapacityCommand(_model);
			EdsCapacity capacity = {0x7FFFFFFF, 0x1000, 1};
			command->setCapacity(capacity);
			StoreAsync(command);

		}
		else if ( strEvent == "takepicture" )
		{
			StoreAsync(new TakePictureCommand(_model));
		}
		else if ( strEvent == "download" )
		{
			StoreAsync(new DownloadCommand(_model, (EdsBaseRef)arg1) );
		}
		else if ( strEvent == "capacity" )
		{
			SetCapacityCommand* command = new SetCapacityCommand(_model);
			EdsCapacity capacity = {0x7FFFFFFF, 0x1000, 1};
			command->setCapacity(capacity);
			StoreAsync(command);
		}
		else if ( strEvent == "get" )
		{ 
			StoreAsync(new GetPropertyCommand(_model, arg2));
		}
		else if( strEvent == "set" )
		{
			StoreAsync(new SetPropertyCommand(_model, arg2, arg3));
		}
		else if( strEvent == "getlist" ) 
		{
			StoreAsync(new GetPropertyDescCommand(_model, arg2 ));
		}
		else if ( strEvent == "close" )
		{
			_model->notifyObservers(EVENT_CLOSE);
			_processor.setCloseCommand(new CloseSessionCommand(_model));
			_processor.stop();
			_processor.join();
		}	

	}

protected:
	//The command is received
	void StoreAsync( Command *command )
	{
		if ( command != NULL )
		{
			_processor.enqueue( command );
		}
	}


};
