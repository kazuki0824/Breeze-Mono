Public Class Zero_core

    Public Property Title As String
    Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
        url.Text = e.Uri.AbsoluteUri
    End Sub
    Public Shared ReadOnly TitleChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("TitleChanged", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(Zero_core))
    Custom Event TitleChanged As RoutedEventHandler
        AddHandler(value As RoutedEventHandler)
            Me.AddHandler(TitleChangedEvent, value)
        End AddHandler

        RemoveHandler(value As RoutedEventHandler)
            Me.RemoveHandler(TitleChangedEvent, value)
        End RemoveHandler

        RaiseEvent(sender As Object, e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Shared ReadOnly UriChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("UriChanged", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(Zero_core))
    Custom Event UriChanged As RoutedEventHandler
        AddHandler(value As RoutedEventHandler)
            Me.AddHandler(UriChangedEvent, value)
        End AddHandler

        RemoveHandler(value As RoutedEventHandler)
            Me.RemoveHandler(UriChangedEvent, value)
        End RemoveHandler

        RaiseEvent(sender As Object, e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    Public Property Uri As String
    Private Sub WebBrowser_LoadCompleted(sender As Object, e As NavigationEventArgs)
        Dim myDocument As mshtml.HTMLDocument = DirectCast(sender.Document, mshtml.HTMLDocument)
        Uri = e.Uri.AbsoluteUri
        RaiseEvent UriChanged(Me, New RoutedEventArgs(UriChangedEvent, sender))
        If Title <> myDocument.title Then
            Title = myDocument.title
            RaiseEvent TitleChanged(Me, New RoutedEventArgs(TitleChangedEvent, sender))
        End If
    End Sub
End Class
