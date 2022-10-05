'/******************************************************************************
'*                                                                             *
'*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
'*      NAME : TakePictureCommand.vb                                           *
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

Public Class TakePictureCommand
    Inherits Command

    Public Sub New(ByVal model As CameraModel)
        MyBase.new(model)
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

        '// Take a picture.
        If err = EDS_ERR_OK Then

            err = EdsSendCommand(VBSample.m_model.getCameraObject(), EdsCameraCommand.kEdsCameraCommand_TakePicture, 0)

        End If


        If IsNothing(lock) = False Then

            err = EdsSendStatusCommand(VBSample.m_model.getCameraObject(), EdsCameraStatusCommand.kEdsCameraStatusCommand_UIUnLock, 0)

        End If

        '// Notify Error.
        If err <> EDS_ERR_OK Then

            '// Do not retry when the camera replys deviceBusy.
            If err = EDS_ERR_DEVICE_BUSY Then

                VBSample.m_model.notifyObservers(warn, err)
                Return True
            End If

            VBSample.m_model.notifyObservers(errr, err)
        End If

        Return True

    End Function

End Class
