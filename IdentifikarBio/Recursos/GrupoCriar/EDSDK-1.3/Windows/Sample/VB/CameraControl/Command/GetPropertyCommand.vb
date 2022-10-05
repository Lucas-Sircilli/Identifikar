'/******************************************************************************
'*                                                                             *
'*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
'*      NAME : GetPropertyCommand.vb                                           *
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


Public Class GetPropertyCommand
    Inherits Command

    Private _propertyID As Integer


    Public Sub New(ByVal model As CameraModel, ByVal propertyID As Integer)
        MyBase.new(model)
        _propertyID = propertyID
    End Sub

    '// Execute a command.	
    Public Overrides Function execute() As Boolean

        Dim err As Integer = EDS_ERR_OK
        Dim lock As Boolean = False

        '// You should do UILock when you send a command to camera models elder than EOS30D.
        If VBSample.m_model.isPTPEx() = False Then
            err = EdsSendStatusCommand(VBSample.m_model.getCameraObject(), EdsCameraStatusCommand.kEdsCameraStatusCommand_UILock, 0)
            If err = EDS_ERR_OK Then

                lock = True

            End If
        End If

        '//Get a property.
        If err = EDS_ERR_OK Then

            err = getProperty(_propertyID)

        End If

        If lock Then

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


    Private Function getProperty(ByVal propertyID As Integer) As Integer
        Dim err As Integer = EDS_ERR_OK
        Dim dataType As EdsDataType = EdsDataType.kEdsDataType_Unknown
        Dim dataSize As Integer = 0


        If propertyID = kEdsPropID_Unknown Then
            '// If the propertyID is invalidID,
            '// you should retry to get properties.
            '// InvalidID is able to be published for the models elder than EOS30D.

            If err = EDS_ERR_OK Then
                err = getProperty(kEdsPropID_AEMode)
            End If
            If err = EDS_ERR_OK Then
                err = getProperty(kEdsPropID_Tv)
            End If
            If err = EDS_ERR_OK Then
                err = getProperty(kEdsPropID_Av)
            End If
            If err = EDS_ERR_OK Then
                err = getProperty(kEdsPropID_ISOSpeed)
            End If

            Return err
        End If

        '// Get propertysize.
        If err = EDS_ERR_OK Then

            err = EdsGetPropertySize( _
                    VBSample.m_model.getCameraObject(), _
                    propertyID, _
                    0, _
                    dataType, _
                    dataSize)

        End If

        If err = EDS_ERR_OK Then

            Dim data As Integer
            If dataType = EdsDataType.kEdsDataType_UInt32 Then
                '// Get a property.
                Dim wkIntPtr As IntPtr = Marshal.AllocHGlobal(dataSize)

                err = EdsGetPropertyData(VBSample.m_model.getCameraObject(), _
                      propertyID, _
                      0, _
                      dataSize, _
                      wkIntPtr)

                data = Marshal.PtrToStructure(wkIntptr, GetType(Integer))
                Marshal.FreeHGlobal(wkIntPtr)

                If err = EDS_ERR_OK Then

                    VBSample.m_model.setPropertyUInt32(propertyID, data)

                End If
            End If

            If dataType = EdsDataType.kEdsDataType_String Then

                Dim str As String 'char[EDS_MAX_NAME]
                Dim wkIntptr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(EDS_MAX_NAME))

                '// Get a property.
                err = EdsGetPropertyData(VBSample.m_model.getCameraObject(), _
                      propertyID, _
                      0, _
                      dataSize, _
                      wkIntptr)

                str = Marshal.PtrToStringAnsi(wkIntptr)
                Marshal.FreeHGlobal(wkIntPtr)

				'// Stock the property .
                If err = EDS_ERR_OK Then

                    VBSample.m_model.setPropertyString(propertyID, str)

                End If
            End If

        End If


        '// Notify updating.
        If err = EDS_ERR_OK Then

            VBSample.m_model.notifyObservers(updt, propertyID)

        End If

        Return err

    End Function

End Class
