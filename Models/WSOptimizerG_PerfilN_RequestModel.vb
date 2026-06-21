Public Class WSOptimizerG_PerfilN_RequestModel
    Public Property CvePerfilN As Int64
    Public Property UsuAct As String
    Public Property Referencia As Integer
    Public Property Temperatura As Double
    Public Property Humedad As Double
    Public Property DesperdicioCrianza As Double
    Public Property DesperdicioPostura As Double
    Public Property EstatusConfort As String
    Public Property TipoInstalaciones As String
    Public Property EtapasModel As List(Of WSOptimizerG_PerfilN_RequestEtapasModel)
End Class
