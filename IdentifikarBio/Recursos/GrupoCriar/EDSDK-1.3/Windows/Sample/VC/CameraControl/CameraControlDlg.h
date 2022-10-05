/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : CameraControlDlg.h                                              *
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

#include "Observer.h"
#include "CameraController.h"
#include "Event.h"
#include <map>


// CCameraControlDlg Dialog
class CCameraControlDlg : public CDialog, public Observer
{
	typedef std::map<EdsUInt32, const char *> PropertyList;
// Construction
private:

	// Update Property
	void UpdateProperty(EdsUInt32 prperytyID, EdsUInt32 data);

	// Update of value list that can set property
	void UpdatePropertyDesc(EdsUInt32 prperytyID, const EdsPropertyDesc* desc);

public:
	CCameraControlDlg(CWnd* pParent = NULL);	// standard constructor
	void setCameraController(CameraController* controller){_controller = controller;}


public:
	// Observer 
	virtual void update(Observable* from, unsigned int msg, Event *event);


	//Dialog data
	enum { IDD = IDD_CAMERACONTROL_DIALOG };
	CButton			_btnTakePicture;
	CProgressCtrl	_progress;
	CEdit			_edit;
	CComboBox		_comboAEMode;
	CComboBox		_comboTv;
	CComboBox		_comboAv;
	CComboBox		_comboIso;
	CComboBox		_comboMeteringMode;
	CComboBox		_comboExposureComp;
	

	// Property list
	PropertyList	_AEModeList;
	PropertyList	_TvList;
	PropertyList	_AvList;
	PropertyList	_IsoList;
	PropertyList	_MeteringModeList;
	PropertyList	_ExposureCompList;

	// Map of property ID and combo box
	std::map<EdsUInt32, CComboBox*> _ctrlMap;
	// Map of property ID and property list
	std::map<EdsUInt32, PropertyList >  _propertyMap;

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg LRESULT OnUpdate(WPARAM wParam, LPARAM lParam);

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButton1();


protected:
	CameraController* _controller;

public:
	afx_msg void OnCbnSelchangeCombo1();
	afx_msg void OnCbnSelchangeCombo2();
	afx_msg void OnCbnSelchangeCombo3();
	afx_msg void OnCbnSelchangeCombo4();
	afx_msg void OnCbnSelchangeCombo5();
	afx_msg void OnCbnSelchangeCombo6();
	afx_msg void OnClose();
};
