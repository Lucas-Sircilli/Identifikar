/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : Synchronized.h	                                              *
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

#include <windows.h>
#include <process.h>


class Synchronized 
{
protected:
	HANDLE _hMutex;

public:
	Synchronized(): _hMutex(NULL)
	{
		 _hMutex = CreateMutex(NULL, false, NULL);
		if(_hMutex == NULL)
		{
			CloseHandle(_hMutex);
		}
	}

    virtual ~Synchronized()
	{
		unlock();
		CloseHandle(_hMutex);	
	}


    virtual void lock() const
	{
		if(_hMutex != NULL)
		{
			WaitForSingleObject(_hMutex, INFINITE);	
		}
	}

    virtual void unlock() const
	{
		if(_hMutex != NULL)
		{
			ReleaseMutex(_hMutex);	
		}	
	}
};


