'/******************************************************************************
'*                                                                             *
'*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
'*      NAME : CameraController.vb                                             *
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



Public Class CameraController

    '// Camera model
    Protected m_model As CameraModel

    '// Command processing
    Protected m_processor As New Processor


    '// Constractor
    Public Sub New()
        m_model = Nothing
    End Sub

    '// Destructor
    Protected Overrides Sub Finalize()
    End Sub


    Public Sub setCameraModel(ByVal inModel As CameraModel)
        m_model = inModel
    End Sub


    '// Start processor thread   
    Public Sub run()
        m_processor.start()
    End Sub


    Public Sub actionPerformed(ByVal strEvent As String, ByVal inObject As IntPtr)
        If strEvent = "download" Then
            accept(New DownloadCommand(m_model, inObject)) ' 
        End If
    End Sub


    Public Sub actionPerformed(ByVal strEvent As String)

        If strEvent = "init" Then

            '// Start communication with remote camera.
            accept(New OpenSessionCommand(m_model))

            '//Set saving-direction PC 
            accept(New SaveSettingCommand(m_model, EdsSaveTo.kEdsSaveTo_Host))

        ElseIf strEvent = "takepicture" Then
            accept(New TakePictureCommand(m_model))

        ElseIf strEvent = "close" Then
            m_model.notifyObservers(clse)
            m_processor.setCloseCommand(New CloseSessionCommand(m_model))
            m_processor.stopTh()
        End If
    End Sub


    Public Sub actionPerformed(ByVal strEvent As String, ByVal id As Integer, Optional ByVal data As Integer = 0)
        If strEvent = "get" Then
            accept(New GetPropertyCommand(m_model, id))

        ElseIf strEvent = "set" Then
            accept(New SetPropertyCommand(m_model, id, data))

        ElseIf strEvent = "getlist" Then
            accept(New GetPropertyDescCommand(m_model, id))

        End If

    End Sub


    '// Receive a command
    Protected Sub accept(ByVal command As Command)
        If IsNothing(command) = False Then
            m_processor.enqueue(command)

        End If

    End Sub


End Class
