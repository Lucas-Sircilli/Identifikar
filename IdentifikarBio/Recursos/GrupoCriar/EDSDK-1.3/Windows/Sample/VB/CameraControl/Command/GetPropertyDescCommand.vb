'/******************************************************************************
'*                                                                             *
'*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
'*      NAME : GetPropertyDescCommand.vb                                       *
'*                                                                             *
'*   Description: This is the Sample code to show the usage of EDSDK.          *
'*                                                                             *
'*                                                                             *
'*******************************************************************************
'*                                                                             *
'*   Written and developed by Camera Design Dept.53                            *
'*   Copyright Canon Inc. 2006 All Rights Reserved                             *
'*                                                                             *
'*******************************************************************************
'*   File Update Information:                                                  *
'*     DATE      Identify    Comment                                           *
'*   -----------------------------------------------------------------------   *
'*   06-03-22    F-001        create first version.                            *
'*                                                                             *
'******************************************************************************/

Option Explicit On
Imports System.Runtime.InteropServices

Public Class GetPropertyDescCommand
    Inherits Command

    Private _propertyID As Integer


    Public Sub New(ByVal model As CameraModel, ByVal propertyID As Integer)
        MyBase.new(model)
        _propertyID = propertyID
    End Sub


    '// Execute a command.
    Overrides Function execute() As Boolean

        Dim err As Integer = EDS_ERR_OK
        Dim Lock As Boolean = False

        '// You should do UILock when you send a command to camera models elder than EOS30D.
        If VBSample.m_model.isPTPEx() = False Then

            err = EdsSendStatusCommand(VBSample.m_model.getCameraObject(), EdsCameraStatusCommand.kEdsCameraStatusCommand_UILock, 0)

            If err = EDS_ERR_OK Then

                Lock = True
            End If
        End If

        '//Get an available property list.
        If err = EDS_ERR_OK Then

            err = getPropertyDesc(_propertyID)

        End If

        If Lock = True Then

            err = EdsSendStatusCommand(VBSample.m_model.getCameraObject(), EdsCameraStatusCommand.kEdsCameraStatusCommand_UIUnLock, 0)

        End If

        '// Notify Error.
        If err <> EDS_ERR_OK Then

            '// Retry when the camera replys deviceBusy.
            If err = EDS_ERR_DEVICE_BUSY Then

                VBSample.m_model.notifyObservers(warn, err)
                Return False
            End If

            VBSample.m_model.notifyObservers(errr, err)

        End If
        Return True

    End Function


    Private Function getPropertyDesc(ByVal propertyID As Integer) As Integer

        Dim err As Integer = EDS_ERR_OK
        Dim propertyDesc As New EdsPropertyDesc

        If propertyID = kEdsPropID_Unknown Then

            '// If the propertyID is invalidID,
            '// you should retry to get available property lists.
            '// InvalidID is able to be published for the models elder than EOS30D.

            If err = EDS_ERR_OK Then
                err = getPropertyDesc(kEdsPropID_AEMode)
            End If
            If err = EDS_ERR_OK Then
                err = getPropertyDesc(kEdsPropID_Tv)
            End If
            If err = EDS_ERR_OK Then
                err = getPropertyDesc(kEdsPropID_Av)
            End If
            If err = EDS_ERR_OK Then
                err = getPropertyDesc(kEdsPropID_ISOSpeed)
            End If

            Return err
        End If

        '// Get available property lists.
        If err = EDS_ERR_OK Then

            err = EdsGetPropertyDesc(VBSample.m_model.getCameraObject(), _
                  propertyID, _
                  propertyDesc)
        End If

        '// Stock the available property list.	
        If err = EDS_ERR_OK Then

            VBSample.m_model.setPropertyDesc(propertyID, propertyDesc)

        End If

        '// Notify updating.
        If err = EDS_ERR_OK Then

            VBSample.m_model.notifyObservers(upls, propertyID)

        End If
        Return err

    End Function

End Class
