'/******************************************************************************
'*                                                                             *
'*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
'*      NAME : Processor.vb                                                    *
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

Imports System.Object
Imports System.Threading


Public Class Processor

    Protected m_th As Thread


    '// Flag for this thread running or not
    Protected m_running As Boolean

    '// queue  
    Protected m_queue As New Queue

    '// End application command
    Protected m_closeCommand As Command

    '// Sync object
    Protected ReadOnly m_syncObject As New Object


    Public Sub New()
        m_running = False
        m_closeCommand = Nothing
        m_th = New Thread(AddressOf run)
    End Sub


    Protected Overrides Sub Finalize()
        clear()
    End Sub



    '// Set end application command
    Public Sub setCloseCommand(ByVal closeCommand As Command)
        m_closeCommand = closeCommand
    End Sub


    Public Sub enqueue(ByVal command As Command)
        If IsNothing(command) = True Then
            Console.WriteLine("Enqueing command is NULL.")
            End
        End If
        Try
            Monitor.Enter(m_syncObject)
            m_queue.Enqueue(command)
        Finally
            Monitor.Exit(m_syncObject)
        End Try
 
    End Sub



    Public Sub stopTh()
        Try
            Monitor.Enter(m_syncObject)
            m_running = False
        Finally
            Monitor.Exit(m_syncObject)
        End Try

    End Sub


    Public Sub clear()
        Monitor.Enter(m_syncObject)
        Try
            m_queue.Clear()
        Finally
            Monitor.Exit(m_syncObject)
        End Try
    End Sub


    Public Sub start()
        If System.Threading.Thread.CurrentThread.Name = Nothing Then
            System.Threading.Thread.CurrentThread.Name = "MainThread"
        End If
        m_th.Start()
    End Sub



    Public Sub run()
        If System.Threading.Thread.CurrentThread.Name = Nothing Then
            System.Threading.Thread.CurrentThread.Name = "ProcessorThread"
        End If

        m_running = True
        While IsNothing(m_running) = False
            Dim command As Command = take()
            If IsNothing(command) = False Then

                Dim complete As Boolean = command.execute()

                If complete = False Then
                    ' >Retry 
                    ' When a command execution failed with DeviceBusy or some errors,
                    ' and you want to set some retrying sequence, you should sleep _
                    ' about 500 micro seconds because it makes camera unstable.
                    Thread.Sleep(500)
                    enqueue(command)
                Else
                    command = Nothing
                End If
            End If
        End While

        '// Clear the queue.
        clear()

        '// End application
        If IsNothing(m_closeCommand) = False Then
            m_closeCommand.execute()
            m_closeCommand = Nothing
        End If

    End Sub


    '//take a command from the queue
    Protected Function take() As Command

        Dim command As Command = Nothing
        Dim iCnt As Integer

        '// Wait when the queue is empty.
        While True
            Try
                Monitor.Enter(m_syncObject)
                iCnt = m_queue.Count()
            Finally
                Monitor.Exit(m_syncObject)
            End Try
            If iCnt <> 0 Then
                Exit While
            End If

            If m_running = False Then
                Return Nothing
            End If
        End While

        Try
            Monitor.Enter(m_syncObject)

            command = m_queue.Dequeue()

        Finally
            Monitor.Exit(m_syncObject)
        End Try

        Return command

    End Function


End Class
