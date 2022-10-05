/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : CameraControlDlg.cpp                                               *
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

#include "stdafx.h"
#include<map>

#include "CameraControl.h"
#include "CameraControlDlg.h"
#include "Property.h"
#include ".\CameraControldlg.h"

#include "EDSDK.h"
#include "EDSDKTypes.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define WM_USER_UPDATE	WM_APP+1

// CCameraControlDlg 

CCameraControlDlg::CCameraControlDlg(CWnd* pParent )
	: CDialog(CCameraControlDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CCameraControlDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);

	DDX_Control(pDX, IDC_BUTTON1, _btnTakePicture);
	DDX_Control(pDX, IDC_PROGRESS1, _progress);
	DDX_Control(pDX, IDC_EDIT1, _edit);
	DDX_Control(pDX, IDC_COMBO1, _comboAEMode);
	DDX_Control(pDX, IDC_COMBO2, _comboTv);
	DDX_Control(pDX, IDC_COMBO3, _comboAv);
	DDX_Control(pDX, IDC_COMBO4, _comboIso);
	DDX_Control(pDX, IDC_COMBO5, _comboMeteringMode);
	DDX_Control(pDX, IDC_COMBO6, _comboExposureComp);

}

BEGIN_MESSAGE_MAP(CCameraControlDlg, CDialog)
	ON_MESSAGE(WM_USER_UPDATE, OnUpdate)
	ON_WM_CLOSE()
	ON_BN_CLICKED(IDC_BUTTON1, OnBnClickedButton1)
	ON_CBN_SELCHANGE(IDC_COMBO1, OnCbnSelchangeCombo1)
	ON_CBN_SELCHANGE(IDC_COMBO2, OnCbnSelchangeCombo2)
	ON_CBN_SELCHANGE(IDC_COMBO3, OnCbnSelchangeCombo3)
	ON_CBN_SELCHANGE(IDC_COMBO4, OnCbnSelchangeCombo4)
	ON_CBN_SELCHANGE(IDC_COMBO5, OnCbnSelchangeCombo5)
	ON_CBN_SELCHANGE(IDC_COMBO6, OnCbnSelchangeCombo6)
END_MESSAGE_MAP()


// CCameraControlDlg message handlers

BOOL CCameraControlDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// The Complete list that each property can set is acquired. 
	_AEModeList =			Property::getAEModeMap();
	_TvList =				Property::getTvMap();
	_AvList =				Property::getAvMap();
	_IsoList =				Property::getIsoMap();
	_MeteringModeList =		Property::getMeteringModeMap();
	_ExposureCompList =		Property::getExposureCompMap();
	
	// The combo box is related to property ID. 
	_ctrlMap.insert( std::pair<EdsUInt32, CComboBox*>(kEdsPropID_AEMode,				&_comboAEMode) );
	_ctrlMap.insert( std::pair<EdsUInt32, CComboBox*>(kEdsPropID_Tv,					&_comboTv) );
	_ctrlMap.insert( std::pair<EdsUInt32, CComboBox*>(kEdsPropID_Av,					&_comboAv) );
	_ctrlMap.insert( std::pair<EdsUInt32, CComboBox*>(kEdsPropID_ISOSpeed,				&_comboIso) );
	_ctrlMap.insert( std::pair<EdsUInt32, CComboBox*>(kEdsPropID_MeteringMode,			&_comboMeteringMode) );
	_ctrlMap.insert( std::pair<EdsUInt32, CComboBox*>(kEdsPropID_ExposureCompensation,	&_comboExposureComp) );

	// The Complete list of each property is related to property ID. 
	_propertyMap.insert( std::pair<EdsUInt32, PropertyList >(kEdsPropID_AEMode,					_AEModeList) );
	_propertyMap.insert( std::pair<EdsUInt32, PropertyList >(kEdsPropID_Tv,						_TvList) );
	_propertyMap.insert( std::pair<EdsUInt32, PropertyList >(kEdsPropID_Av,						_AvList) );
	_propertyMap.insert( std::pair<EdsUInt32, PropertyList >(kEdsPropID_ISOSpeed,				_IsoList) );
	_propertyMap.insert( std::pair<EdsUInt32, PropertyList >(kEdsPropID_MeteringMode,			_MeteringModeList) );
	_propertyMap.insert( std::pair<EdsUInt32, PropertyList >(kEdsPropID_ExposureCompensation,	_ExposureCompList) );

	//Execute controller
	_controller->run();

	// Initialization of controller
	_controller->actionPerformed("init");

	// A set value of the camera is acquired. 
	_controller->actionPerformed("get", NULL, kEdsPropID_AEMode , 0);
	_controller->actionPerformed("get", NULL, kEdsPropID_Tv , 0);
	_controller->actionPerformed("get", NULL, kEdsPropID_Av , 0 );
	_controller->actionPerformed("get", NULL, kEdsPropID_ISOSpeed , 0 );
	_controller->actionPerformed("get", NULL, kEdsPropID_MeteringMode , 0 );
	_controller->actionPerformed("get", NULL, kEdsPropID_ExposureCompensation , 0 );


	// The value to which the camera can be set is acquired. 
	_controller->actionPerformed("getlist", NULL, kEdsPropID_AEMode , 0 );
	_controller->actionPerformed("getlist", NULL, kEdsPropID_Tv , 0 );
	_controller->actionPerformed("getlist", NULL, kEdsPropID_Av , 0 );
	_controller->actionPerformed("getlist", NULL, kEdsPropID_ISOSpeed , 0 );
	_controller->actionPerformed("getlist", NULL, kEdsPropID_MeteringMode , 0 );
	_controller->actionPerformed("getlist", NULL, kEdsPropID_ExposureCompensation , 0 );


	return TRUE;   // return TRUE  unless you set the focus to a control
}


void CCameraControlDlg::OnClose()
{
	// TODO : The control notification handler code is added here or Predetermined processing is called. 
	_controller->actionPerformed("close");

	__super::OnClose();
}

// Take Picture
void CCameraControlDlg::OnBnClickedButton1()
{
	// TODO : The control notification handler code is added here.
	_controller->actionPerformed("takepicture", NULL , 0, 0);
}

// Change AEMode
void CCameraControlDlg::OnCbnSelchangeCombo1()
{
	// TODO : The control notification handler code is added here
	DWORD_PTR data = _comboAEMode.GetItemData(_comboAEMode.GetCurSel());
	_controller->actionPerformed("set",  NULL, kEdsPropID_AEMode, (EdsUInt32)data );
}

// Change Tv
void CCameraControlDlg::OnCbnSelchangeCombo2()
{
	// TODO : The control notification handler code is added here
	DWORD_PTR data = _comboTv.GetItemData(_comboTv.GetCurSel());
	_controller->actionPerformed("set",  NULL, kEdsPropID_Tv, (EdsUInt32)data );
}

// Change Av
void CCameraControlDlg::OnCbnSelchangeCombo3()
{
	// TODO : The control notification handler code is added here
	DWORD_PTR data = _comboAv.GetItemData(_comboAv.GetCurSel());
	_controller->actionPerformed("set",  NULL, kEdsPropID_Av, (EdsUInt32)data );
}

// Change ISO
void CCameraControlDlg::OnCbnSelchangeCombo4()
{
	// TODO : The control notification handler code is added here
	DWORD_PTR data = _comboIso.GetItemData(_comboIso.GetCurSel());
	_controller->actionPerformed("set",  NULL, kEdsPropID_ISOSpeed, (EdsUInt32)data );
}

// Change MeteringMode
void CCameraControlDlg::OnCbnSelchangeCombo5()
{
	// TODO : The control notification handler code is added here
	DWORD_PTR data = _comboMeteringMode.GetItemData(_comboMeteringMode.GetCurSel());
	_controller->actionPerformed("set",  NULL, kEdsPropID_MeteringMode, (EdsUInt32)data );
}

// Change ExposureCompensation
void CCameraControlDlg::OnCbnSelchangeCombo6()
{
	// TODO : The control notification handler code is added here
	DWORD_PTR data = _comboExposureComp.GetItemData(_comboExposureComp.GetCurSel());
	_controller->actionPerformed("set",  NULL, kEdsPropID_ExposureCompensation, (EdsUInt32)data );
}



void CCameraControlDlg::update(Observable* from, unsigned int msg, Event *event)
{
	//The update processing can be executed from another thread. 
	::PostMessage(this->m_hWnd, WM_USER_UPDATE, (WPARAM)msg , (LPARAM)event);
}


LRESULT CCameraControlDlg::OnUpdate(WPARAM wParam, LPARAM lParam)
{
	Event *event = (Event *)lParam;

	switch(wParam)
	{
		//Progress of download of image
		case EVENT_PROGRESS:
			_progress.SetPos(((StateEvent *)event)->getEventData());
		break;
	
		// Beginning of download of image
		case EVENT_DOWNLOAD_START:
		break;
	
		//End of download of image
		case EVENT_DOWNLOAD_COMPLETE:
				_progress.SetPos(0);
			break;

		//Update property
		case EVENT_PROPERTY_UPDATE:
			{
				CameraModel *model = getCameraModel();
				EdsUInt32 propertyID = ((PropertyEvent *)event)->getPropertyID();
			
		
				EdsUInt32 data = model->getPropertyUInt32(propertyID);
				UpdateProperty(propertyID, data);
			
			}
			break;
	
		//Update of list that can set property
		case EVENT_PROPERTY_LIST_UPDATE:
			{
				CameraModel *model = getCameraModel();
				EdsUInt32 propertyID = ((PropertyEvent *)event)->getPropertyID();
				
				EdsPropertyDesc desc = model->getPropertyDesc(propertyID);
				UpdatePropertyDesc(propertyID, &desc);				
				
			}
			break;
	
		//Warning
		case EVENT_WARNING:
			_edit.SetWindowText("Device Busy");
			break;

		//Error
		case EVENT_ERROR:
			{
				//For the EOS 30D, it is normal for the SDK to return an error 
				// when the first property is retrieved; do not handle the error 
				CString ss;
				ss.Format("%x", ((StateEvent *)event)->getEventData());
				_edit.SetWindowText(ss);
			}
			break;

		//end
		case EVENT_CLOSE:
			_btnTakePicture.EnableWindow(FALSE);	
			_progress.EnableWindow(FALSE);
			_edit.EnableWindow(FALSE);
			_comboAEMode.EnableWindow(FALSE);
			_comboTv.EnableWindow(FALSE);
			_comboAv.EnableWindow(FALSE);
			_comboIso.EnableWindow(FALSE);
			break;

	}

	if(wParam != 'errr' && wParam != 'warn')
	{
		_edit.SetWindowText("");
	}

	if(event != NULL)
	{
		delete event;
	}

	return 0;
}


// Update of set value
void CCameraControlDlg::UpdateProperty(EdsUInt32 prperytyID, EdsUInt32 data)
{
	// Acquisition of combo box corresponding to property ID
	std::map<EdsUInt32, CComboBox*>::iterator cmb_it = _ctrlMap.find(prperytyID);

	// Acquisition of property list corresponding to property ID
	std::map<EdsUInt32, PropertyList>::iterator prop_it = _propertyMap.find(prperytyID);

	if (cmb_it != _ctrlMap.end() && prop_it != _propertyMap.end())
	{
		CComboBox* comboBox = cmb_it->second;
		PropertyList property = prop_it->second;

		// The character string corresponding to data is acquired. 
		std::map<EdsUInt32, const char*>::iterator itr = property.find(data);
		if (itr != property.end())
		{		
			// Set String combo box
			comboBox->SetWindowText(itr->second);
		}
	}
}


// Update of value list that can be set
void CCameraControlDlg::UpdatePropertyDesc(EdsUInt32 prperytyID, const EdsPropertyDesc* desc)
{
	// Acquisition of combo box corresponding to property ID
	std::map<EdsUInt32, CComboBox*>::iterator cmb_it = _ctrlMap.find(prperytyID);

	// Acquisition of property list corresponding to property ID
	std::map<EdsUInt32, PropertyList >::iterator prop_it = _propertyMap.find(prperytyID);

	if (cmb_it != _ctrlMap.end() && prop_it != _propertyMap.end())
	{
		CComboBox* comboBox = cmb_it->second;
		PropertyList property = prop_it->second;

		// The content of the list is deleted.
		// Current settings values are not changed in some cases even if the list changes, so leave the selected text as it is
		CString ss;
		comboBox->GetWindowText(ss);
		comboBox->ResetContent();
		comboBox->SetWindowText(ss);
	
		// It makes it to disable when there is no value list that can be set. 
		comboBox->EnableWindow( desc->numElements != 0 );


		for(int i = 0; i < desc->numElements; i++)
		{
			// The character string corresponding to data is acquired.
			PropertyList::iterator itr = property.find(desc->propDesc[i]);
			
			// Create list of combo box
			if (itr != property.end())
			{
				// Insert string
				int index = comboBox->InsertString(-1, itr->second);
				// Set data
				comboBox->SetItemData(index, itr->first);
			}
		}	
	}
}







