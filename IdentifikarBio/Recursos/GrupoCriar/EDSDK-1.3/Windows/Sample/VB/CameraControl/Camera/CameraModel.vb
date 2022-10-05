'/******************************************************************************
'*                                                                             *
'*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
'*      NAME : CameraModel.vb                                                  *
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

Public Class CameraModel
    Inherits Observable


    Protected m_camera As IntPtr

    '// UIlock counter
    Protected m_lockCount As Integer

    '// Model name
    Protected m_modelName As String

    '// Parameters
    Protected m_AEMode As Integer
    Protected m_Av As Integer
    Protected m_Tv As Integer
    Protected m_Iso As Integer
    Protected m_MeteringMode As Integer
    Protected m_ExposureCompensation As Integer
    Protected m_availableShot As Integer

    '// Available parameter lists
    Protected m_AEModeDesc As EdsPropertyDesc
    Protected m_AvDesc As EdsPropertyDesc
    Protected m_TvDesc As EdsPropertyDesc
    Protected m_IsoDesc As EdsPropertyDesc
    Protected m_MeteringModeDesc As EdsPropertyDesc
    Protected m_ExposureCompensationDesc As EdsPropertyDesc


    '// Constructor
    Public Sub New(ByVal inCamera As IntPtr)
        m_lockCount = 0
        m_camera = inCamera
    End Sub

    '// Get a camera object
    Public Function getCameraObject() As IntPtr
        Return m_camera
    End Function


    '// -----------------------------------------------------------------
    '// Stock parameters ---------------------------------------------

    Private Sub setAEMode(ByVal value As Integer)
        m_AEMode = value
    End Sub

    Private Sub setTv(ByVal value As Integer)
        m_Tv = value
    End Sub

    Private Sub setAv(ByVal value As Integer)
        m_Av = value
    End Sub

    Private Sub setIso(ByVal value As Integer)
        m_Iso = value
    End Sub

    Private Sub setMeteringMode(ByVal value As Integer)
        m_MeteringMode = value
    End Sub

    Private Sub setExposureCompensation(ByVal value As Integer)
        m_ExposureCompensation = value
    End Sub

    Private Sub setModelName(ByVal modelName As String)
        m_modelName = modelName
    End Sub

    '// -----------------------------------------------------------------
    '// Give parameters ---------------------------------------------

    Private Function getAEMode() As Integer
        Return m_AEMode
    End Function

    Private Function getTv() As Integer
        Return m_Tv
    End Function

    Private Function getAv() As Integer
        Return m_Av
    End Function

    Private Function getIso() As Integer
        Return m_Iso
    End Function

    Private Function getMeteringMode() As Integer
        Return m_MeteringMode
    End Function

    Private Function getExposureCompensation() As Integer
        Return m_ExposureCompensation
    End Function


    '// -----------------------------------------------------------------
    '// Give available parameter lists ----------------------------------

    Private Function getAEModeDesc() As EdsPropertyDesc
        Return m_AEModeDesc
    End Function

    Private Function getAvDesc() As EdsPropertyDesc
        Return m_AvDesc
    End Function

    Private Function getTvDesc() As EdsPropertyDesc
        Return m_TvDesc
    End Function

    Private Function getIsoDesc() As EdsPropertyDesc
        Return m_IsoDesc
    End Function

    Private Function getMeteringModeDesc() As EdsPropertyDesc
        Return m_MeteringModeDesc
    End Function

    Private Function getExposureCompensationDesc() As EdsPropertyDesc
        Return m_ExposureCompensationDesc
    End Function


    '// -----------------------------------------------------------------
    '// Stock available parameter lists ---------------------------------

    Private Sub setAEModeDesc(ByVal desc As EdsPropertyDesc)
        m_AEModeDesc = desc
    End Sub

    Private Sub setAvDesc(ByVal desc As EdsPropertyDesc)
        m_AvDesc = desc
    End Sub

    Private Sub setTvDesc(ByVal desc As EdsPropertyDesc)
        m_TvDesc = desc
    End Sub

    Private Sub setIsoDesc(ByVal desc As EdsPropertyDesc)
        m_IsoDesc = desc
    End Sub

    Private Sub setMeteringModeDesc(ByVal desc As EdsPropertyDesc)
        m_MeteringModeDesc = desc
    End Sub

    Private Sub setExposureCompensationDesc(ByVal desc As EdsPropertyDesc)
        m_ExposureCompensationDesc = desc
    End Sub



    '// Set a property <UInt32>
    Public Sub setPropertyUInt32(ByVal propertyID As Integer, ByVal value As Integer)
        Select Case propertyID
            Case kEdsPropID_AEMode
                setAEMode(value)
            Case kEdsPropID_Tv
                setTv(value)
            Case kEdsPropID_Av
                setAv(value)
            Case kEdsPropID_ISOSpeed
                setIso(value)
            Case kEdsPropID_MeteringMode
                setMeteringMode(value)
            Case kEdsPropID_ExposureCompensation
                setExposureCompensation(value)
        End Select
    End Sub


    '// Get a property <UInt32>
    Public Function getPropertyUInt32(ByVal propertyID As Integer) As Integer
        Dim value As Integer = &HFFFFFFFF
        Select Case propertyID
            Case kEdsPropID_AEMode
                value = getAEMode()
            Case kEdsPropID_Tv
                value = getTv()
            Case kEdsPropID_Av
                value = getAv()
            Case kEdsPropID_ISOSpeed
                value = getIso()
            Case kEdsPropID_MeteringMode
                value = getMeteringMode()
            Case kEdsPropID_ExposureCompensation
                value = getExposureCompensation()
        End Select
        Return value

    End Function


    '// Get a property <String>
    Public Sub getPropertyString(ByVal propertyID As Integer, ByRef str As String)
        Select Case propertyID
            Case kEdsPropID_ProductName
                str = m_modelName
        End Select
    End Sub


    '// Set a property <String>
    Public Sub setPropertyString(ByVal propertyID As Integer, ByVal str As String)
        Select Case propertyID
            Case kEdsPropID_ProductName
                m_modelName = str
        End Select
    End Sub


    '// Set an available parameter list.
    Public Sub setPropertyDesc(ByVal propertyID As Integer, ByVal desc As EdsPropertyDesc)
        Select Case propertyID
            Case kEdsPropID_AEMode
                setAEModeDesc(desc)
            Case kEdsPropID_Tv
                setTvDesc(desc)
            Case kEdsPropID_Av
                setAvDesc(desc)
            Case kEdsPropID_ISOSpeed
                setIsoDesc(desc)
            Case kEdsPropID_MeteringMode
                setMeteringModeDesc(desc)
            Case kEdsPropID_ExposureCompensation
                setExposureCompensationDesc(desc)
        End Select
    End Sub

    '// Get an available parameter list.
    Public Function getPropertyDesc(ByVal propertyID As Integer) As EdsPropertyDesc
        Dim desc As EdsPropertyDesc = Nothing
        Select Case propertyID
            Case kEdsPropID_AEMode
                desc = getAEModeDesc()
            Case kEdsPropID_Tv
                desc = getTvDesc()
            Case kEdsPropID_Av
                desc = getAvDesc()
            Case kEdsPropID_ISOSpeed
                desc = getIsoDesc()
            Case kEdsPropID_MeteringMode
                desc = getMeteringModeDesc()
            Case kEdsPropID_ExposureCompensation
                desc = getExposureCompensationDesc()
        End Select
        Return desc
    End Function

    '// Check camera accessing flag.
    '// Connected camera is not a legacy one, this method will be called.
    Public Overridable Function isPTPEx() As Boolean
        Return True
    End Function

End Class
